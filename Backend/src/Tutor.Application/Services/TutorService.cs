using Ardalis.Result;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
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

    public TutorService(
        IGenericRepository<User, int> userRepository,
        IUserService userService,
        IGenericRepository2<TutorProfile> tutorProfileRepository,
        IUserRoleService userRoleService,
        ITutorSubjectService tutorSubjectService,
        IMapper mapper)
    {
        _userRepository = userRepository;
        _userService = userService;
        _tutorProfileRepository = tutorProfileRepository;
        _userRoleService = userRoleService;
        _tutorSubjectService = tutorSubjectService;
        _mapper = mapper;
    }

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

        var result = await GetTutorProfileAsync(userId);
        return result;
    }

    public async Task<Result<TutorProfileDto>> GetTutorProfileAsync(int userId)
    {
        var profile = await _tutorProfileRepository.FindAsyncDefault(tp => tp.UserId == userId);
        if (profile is null)
            return Result<TutorProfileDto>.NotFound("Tutor profile not found");

        return _mapper.Map<TutorProfileDto>(profile);
    }

    public async Task<Result<List<TutorProfileDto>>> GetAllTutorProfileAsync(
        string? city = null,
        string? country = null,
        int[]? subjectIds = null,
        int[]? ratings = null,
        decimal? minPrice = null,
        decimal? maxPrice = null,
        string? sortBy = null,
        bool sortDescending = false)
    {
        var profiles = await _tutorProfileRepository.GetAll();
        if (profiles is null || profiles.Count() == 0)
            return Result<List<TutorProfileDto>>.NotFound("Tutor profiles not found");

        // Filter verified profiles first
        var verifiedProfiles = profiles
            .Where(profile => profile.VerificationStatus == VerificationStatus.Verified &&
                              profile.User.IsActive == true)
            .ToList();

        // Apply all filters client-side
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

        // Apply sorting
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

        if (resultList.Count == 0)
            return Result<List<TutorProfileDto>>.NotFound("No tutors found matching the criteria");

        var result = _mapper.Map<List<TutorProfileDto>>(resultList);
        return Result<List<TutorProfileDto>>.Success(result);
    }

    public async Task<Result<List<TutorProfileDto>>> GetAllTutorProfileAsyncForAdmin()
    {
        var profiles = await _tutorProfileRepository.GetAll();
        if (profiles is null || profiles.Count() == 0)
            return Result<List<TutorProfileDto>>.NotFound("Tutor profiles not found");
        var result = _mapper.Map<List<TutorProfileDto>>(profiles);
        return Result<List<TutorProfileDto>>.Success(result);
    }

    public async Task<Result<TutorProfileDto>> ApproveTutorAsync(int userId)
    {
        var profile = await _tutorProfileRepository.FindAsyncDefault(tp => tp.UserId == userId);
        if (profile is null)
            return Result<TutorProfileDto>.NotFound("Tutor profile not found");
        profile.VerificationStatus = VerificationStatus.Verified;
        await _tutorProfileRepository.Update(profile);

        return _mapper.Map<TutorProfileDto>(profile);
    }

    public async Task<Result<TutorProfileDto>> DeclineTutorAsync(int userId)
    {
        var profile = await _tutorProfileRepository.FindAsyncDefault(tp => tp.UserId == userId);
        if (profile is null)
            return Result<TutorProfileDto>.NotFound("Tutor profile not found");

        profile.VerificationStatus = VerificationStatus.Rejected;
        await _tutorProfileRepository.Update(profile);

        return _mapper.Map<TutorProfileDto>(profile);
    }

    public async Task<Result<TutorProfileDto>> UpdateTutorAsync(int userId, UpdateTutorProfileDto updateTutorProfileDto)
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

        return Result<TutorProfileDto>.Success(_mapper.Map<TutorProfileDto>(tutorProfile));
    }
}