using Ardalis.Result;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tutor.Application.Features.Tutors.Dto;
using Tutor.Application.Features.Users.Dtos;
using Tutor.Application.Interfaces;
using Tutor.Domain.Entities;
using Tutor.Domain.Interfaces;

namespace Tutor.Application.Services;

public class TutorService : ITutorService
{
    private readonly IGenericRepository<User, int> _userRepository;
    private readonly IGenericRepository2<TutorProfile> _tutorProfileRepository;
    private readonly IUserRoleService _userRoleService;
    private readonly ITutorSubjectService _tutorSubjectService;
    private readonly IUserService _userService;
    private readonly IMapper _mapper;
    private readonly IGenericRepository<Favorites, int> _favoritesRepository;
    private readonly ICacheService _cacheService;
    private readonly ILogger<TutorService> _logger;


    public TutorService(
        IGenericRepository<User, int> userRepository,
        IUserService userService,
        IGenericRepository2<TutorProfile> tutorProfileRepository,
        IUserRoleService userRoleService,
        ITutorSubjectService tutorSubjectService,
        IGenericRepository<Favorites, int> favoritesRepository,
        ICacheService cacheService,
        ILogger<TutorService> logger,
        IMapper mapper)
    {
        _userRepository = userRepository;
        _userService = userService;
        _tutorProfileRepository = tutorProfileRepository;
        _userRoleService = userRoleService;
        _tutorSubjectService = tutorSubjectService;
        _favoritesRepository = favoritesRepository;
        _mapper = mapper;
        _cacheService = cacheService;
        _logger = logger;
    }

    private const string TutorByIdKey = "tutor:{0}";
    private const string TutorListKey = "tutors:list";

    public async Task<Result<TutorProfileDto>> CreateTutorProfileAsync(CreateTutorProfileDto createTutorProfileDto,
        int userId)
    {
        var user = await _userRepository.GetById(userId);
        if (user is null)
            return Result<TutorProfileDto>.NotFound("User not found");

        var existingProfile = await _tutorProfileRepository.FindAsyncDefault(tp => tp.UserId == userId);
        if (existingProfile is not null)
            return Result<TutorProfileDto>.Error("Tutor profile already exists");


        var tutorProfile = new TutorProfile
        {
            UserId = userId,
            VerificationStatus = createTutorProfileDto.VerificationStatus,
            ExperienceYears = createTutorProfileDto.ExperienceYears,
            WorkingLocation = createTutorProfileDto.WorkingLocation
        };

        await _tutorProfileRepository.Create(tutorProfile);


        await _tutorSubjectService.AddTutorSubjectsAsync(userId, createTutorProfileDto.Subjects);
        await _cacheService.RemoveByPatternAsync("tutors:filtered:");
        var result = await GetTutorProfileAsync(userId);
        return result;
    }

    public async Task<Result<TutorProfileDto>> GetTutorProfileAsync(int userId)
    {
        var cacheKey = string.Format(TutorByIdKey, userId);
        var cachedTutor = await _cacheService.GetAsync<TutorProfileDto>(cacheKey);
        if (cachedTutor != null)
        {
            return Result<TutorProfileDto>.Success(cachedTutor);
        }

        var profile = await _tutorProfileRepository.FindAsyncDefault(tp => tp.UserId == userId);
        if (profile is null)
            return Result<TutorProfileDto>.NotFound("Tutor profile not found");
        var profileDto = _mapper.Map<TutorProfileDto>(profile);
        await _cacheService.SetAsync(cacheKey, profileDto, TimeSpan.FromHours(1));

        return profileDto;
    }

    public async Task<Result<List<TutorProfileDto>>> GetAllTutorProfileAsync(
        int userId,
        string? city = null,
        string? country = null,
        int[]? subjectIds = null,
        int[]? ratings = null,
        decimal? minPrice = null,
        decimal? maxPrice = null,
        string? sortBy = null,
        bool sortDescending = false
    )
    {
        bool isGetAllRequest = string.IsNullOrEmpty(city) &&
                               string.IsNullOrEmpty(country) &&
                               (subjectIds == null || subjectIds.Length == 0) &&
                               (ratings == null || ratings.Length == 0) &&
                               !minPrice.HasValue &&
                               !maxPrice.HasValue;
        if (isGetAllRequest)
        {
            var cachedTutors = await _cacheService.GetAsync<List<TutorProfileDto>>(TutorListKey);
            if (cachedTutors != null && cachedTutors.Any())
            {
                var tutorIdsCache = cachedTutors.Select(t => t.UserId).ToList();
                var favoritesCache =
                    await _favoritesRepository.FindAsync(f =>
                        f.StudentUserId == userId && tutorIdsCache.Contains(f.TutorUserId));

                var favoritesIdsCache = favoritesCache.Select(t => t.TutorUserId).ToList();
                foreach (var tutorDto in cachedTutors)
                {
                    tutorDto.IsFavorite = favoritesIdsCache.Contains(tutorDto.UserId);
                    var cacheKey = string.Format(TutorByIdKey, tutorDto.UserId);
                    await _cacheService.SetAsync(cacheKey, tutorDto, TimeSpan.FromHours(1));
                }

                return Result<List<TutorProfileDto>>.Success(cachedTutors);
            }
        }

        var profiles = await _tutorProfileRepository.GetAll();
        var verifiedProfiles = profiles
            .Where(profile => profile.VerificationStatus == VerificationStatus.Verified &&
                              profile.User.IsActive == true)
            .ToList();
        var filteredProfiles = verifiedProfiles.AsEnumerable();
        if (!string.IsNullOrEmpty(city))
        {
            filteredProfiles = filteredProfiles.Where(profile =>
                profile.User?.City != null &&
                profile.User.City.Equals(city, StringComparison.OrdinalIgnoreCase));
        }

        if (!string.IsNullOrEmpty(country))
        {
            filteredProfiles = filteredProfiles.Where(profile =>
                profile.User?.Country != null &&
                profile.User.Country.Equals(country, StringComparison.OrdinalIgnoreCase));
        }

        if (subjectIds != null && subjectIds.Length > 0)
        {
            filteredProfiles = filteredProfiles.Where(profile =>
                profile.TutorSubjects != null &&
                profile.TutorSubjects.Any(ts => subjectIds.Contains(ts.SubjectId)));
        }

        if (minPrice.HasValue)
        {
            filteredProfiles = filteredProfiles.Where(profile =>
                profile.TutorSubjects != null &&
                profile.TutorSubjects.Any(ts => ts.Price >= minPrice.Value));
        }

        if (maxPrice.HasValue)
        {
            filteredProfiles = filteredProfiles.Where(profile =>
                profile.TutorSubjects != null &&
                profile.TutorSubjects.Any(ts => ts.Price <= maxPrice.Value));
        }

        if (ratings != null && ratings.Length > 0)
        {
            filteredProfiles = filteredProfiles.Where(profile =>
                profile.Reviews != null &&
                profile.Reviews.Any() &&
                ratings.Contains((int)Math.Floor(profile.Reviews.Average(r => r.Rating))));
        }

        IOrderedEnumerable<TutorProfile> orderedProfiles;
        switch (sortBy?.ToLower())
        {
            case "price":
                orderedProfiles = sortDescending
                    ? filteredProfiles.OrderByDescending(profile =>
                        profile.TutorSubjects != null && profile.TutorSubjects.Any()
                            ? profile.TutorSubjects.Min(ts => ts.Price)
                            : decimal.MaxValue)
                    : filteredProfiles.OrderBy(profile =>
                        profile.TutorSubjects != null && profile.TutorSubjects.Any()
                            ? profile.TutorSubjects.Min(ts => ts.Price)
                            : decimal.MaxValue);
                break;

            case "experience":
                orderedProfiles = sortDescending
                    ? filteredProfiles.OrderByDescending(profile => profile.ExperienceYears ?? 0)
                    : filteredProfiles.OrderBy(profile => profile.ExperienceYears ?? 0);
                break;

            case "rating":
                orderedProfiles = sortDescending
                    ? filteredProfiles.OrderByDescending(profile =>
                        profile.Reviews != null && profile.Reviews.Any()
                            ? profile.Reviews.Average(r => r.Rating)
                            : 0)
                    : filteredProfiles.OrderBy(profile =>
                        profile.Reviews != null && profile.Reviews.Any()
                            ? profile.Reviews.Average(r => r.Rating)
                            : 0);
                break;

            default:
                orderedProfiles = filteredProfiles.OrderBy(profile => profile.UserId);
                break;
        }

        var resultList = orderedProfiles.ToList();

        var result = _mapper.Map<List<TutorProfileDto>>(resultList);
        var tutorIds = result.Select(t => t.UserId).ToList();
        var favorites =
            await _favoritesRepository.FindAsync(f =>
                f.StudentUserId == userId && tutorIds.Contains(f.TutorUserId));
        var favoritesIds = favorites.Select(t => t.TutorUserId).ToList();
        foreach (var tutorDto in result)
        {
            tutorDto.IsFavorite = favoritesIds.Contains(tutorDto.UserId);
            var cacheKey = string.Format(TutorByIdKey, tutorDto.UserId);
            await _cacheService.SetAsync(cacheKey, tutorDto, TimeSpan.FromHours(1));
        }

        if (isGetAllRequest)
        {
            await _cacheService.SetAsync(TutorListKey, result, TimeSpan.FromHours(1));
        }

        return Result<List<TutorProfileDto>>.Success(result);
    }

    public async Task<Result<List<TutorProfileDto>>> GetAllTutorProfileAsyncForAdmin()
    {
        var profiles = await _tutorProfileRepository.GetAll();
        var filteredProfiles = new List<TutorProfile>();
        foreach (var profile in profiles)
        {
            int? role = await _userRoleService.GetRoleIdAsync(profile.UserId);
            if (role == 3)
                filteredProfiles.Add(profile);
        }

        var result = _mapper.Map<List<TutorProfileDto>>(filteredProfiles);
        foreach (var tutorDto in result)
        {
            var cacheKey = string.Format(TutorByIdKey, tutorDto.UserId);
            await _cacheService.SetAsync(cacheKey, tutorDto, TimeSpan.FromHours(1));
        }

        return Result<List<TutorProfileDto>>.Success(result);
    }

    public async Task<Result<TutorProfileDto>> ApproveTutorAsync(int userId)
    {
        var profile = await _tutorProfileRepository.FindAsyncDefault(tp => tp.UserId == userId);
        if (profile is null)
            return Result<TutorProfileDto>.NotFound("Tutor profile not found");
        profile.VerificationStatus = VerificationStatus.Verified;
        await _tutorProfileRepository.Update(profile);
        var cacheKey = string.Format(TutorByIdKey, userId);
        await _cacheService.RemoveAsync(cacheKey);
        await _cacheService.RemoveAsync(TutorListKey);
        return _mapper.Map<TutorProfileDto>(profile);
    }

    public async Task<Result<TutorProfileDto>> DeclineTutorAsync(int userId)
    {
        var profile = await _tutorProfileRepository.FindAsyncDefault(tp => tp.UserId == userId);
        if (profile is null)
            return Result<TutorProfileDto>.NotFound("Tutor profile not found");

        profile.VerificationStatus = VerificationStatus.Rejected;
        await _tutorProfileRepository.Update(profile);
        var cacheKey = string.Format(TutorByIdKey, userId);
        await _cacheService.RemoveAsync(cacheKey);
        await _cacheService.RemoveAsync(TutorListKey);
        return _mapper.Map<TutorProfileDto>(profile);
    }

    public async Task<Result<TutorProfileDto>> UpdateTutorAsync(int userId,
        UpdateTutorProfileDto updateTutorProfileDto)
    {
        var userProfileDto = _mapper.Map<CreateProfileDto>(updateTutorProfileDto);

        var result = await _userService.UpdateProfileAsync(userId, userProfileDto);
        if (result.Errors.Any())
            return Result<TutorProfileDto>.Error(result.Errors.ToString());
        var tutorProfile = await _tutorProfileRepository.FindAsyncDefault(tp => tp.UserId == userId);
        if (tutorProfile is null)
            return Result<TutorProfileDto>.NotFound("Tutor profile not found");
        tutorProfile.ExperienceYears = updateTutorProfileDto.ExperienceYears;
        tutorProfile.WorkingLocation = updateTutorProfileDto.WorkingLocation;
        await _tutorProfileRepository.Update(tutorProfile);
        var cacheKey = string.Format(TutorByIdKey, userId);
        await _cacheService.RemoveAsync(cacheKey);
        await _cacheService.RemoveAsync(TutorListKey);
        return Result<TutorProfileDto>.Success(_mapper.Map<TutorProfileDto>(tutorProfile));
    }

    public async Task<Result<bool>> AddToFavorite(int userId, int tutorProfileId)
    {
        var tutorProfile = await _tutorProfileRepository.FindAsyncDefault(tp => tp.UserId == tutorProfileId);
        if (tutorProfile == null)
        {
            return Result<bool>.Error("Tutor profile not found.");
        }

        var existingFavorite = await _favoritesRepository.FindAsyncDefault(f =>
            f.StudentUserId == userId && f.TutorUserId == tutorProfileId);
        if (existingFavorite != null)
        {
            return Result<bool>.Error("This tutor is already in your favorites.");
        }

        var favorite = new Favorites { StudentUserId = userId, TutorUserId = tutorProfileId, };
        await _favoritesRepository.Create(favorite);
        return Result<bool>.Success(true, "Tutor added to favorites successfully.");
    }

    public async Task<Result<bool>> DeleteFavorite(int userId, int tutorProfileId)
    {
        var tutorProfile = await _tutorProfileRepository.FindAsyncDefault(tp => tp.UserId == tutorProfileId);
        if (tutorProfile == null)
        {
            return Result<bool>.Error("Tutor profile not found.");
        }

        var existingFavorite = await _favoritesRepository.FindAsyncDefault(f =>
            f.StudentUserId == userId && f.TutorUserId == tutorProfileId);
        if (existingFavorite == null)
        {
            return Result<bool>.Error("Your favorite does not exist.");
        }

        await _favoritesRepository.Delete(existingFavorite);
        return Result<bool>.Success(true, "Tutor deleted from favorites successfully.");
    }
}