using Ardalis.Result;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tutor.Application.Features.Tutors.Dto;
using Tutor.Application.Interfaces;
using Tutor.Domain.Entities;
using Tutor.Domain.Interfaces;

public class TutorService : ITutorService
{
    private readonly IGenericRepository<User, int> _userRepository;
    private readonly IGenericRepository2<TutorProfile> _tutorProfileRepository;
    private readonly IUserRoleService _userRoleService;
    private readonly ITutorSubjectService _tutorSubjectService;
    private readonly IMapper _mapper;

    public TutorService(
        IGenericRepository<User, int> userRepository,
        IGenericRepository2<TutorProfile> tutorProfileRepository,
        IUserRoleService userRoleService,
        ITutorSubjectService tutorSubjectService,
        IMapper mapper)
    {
        _userRepository = userRepository;
        _tutorProfileRepository = tutorProfileRepository;
        _userRoleService = userRoleService;
        _tutorSubjectService = tutorSubjectService;
        _mapper = mapper;
    }

    public async Task<Result<TutorProfileDto>> CreateTutorProfileAsync(CreateTutorProfileDto createTutorProfileDto, int userId)
    {
        var user = await _userRepository.GetById(userId);
        if (user is null) 
            return Result<TutorProfileDto>.NotFound("User not found");

        var existingProfile = await _tutorProfileRepository.FindAsyncDefault(tp => tp.UserId == userId);
        if (existingProfile is not null)
            return Result<TutorProfileDto>.Error("Tutor profile already exists");

        if (await _userRoleService.HasAnyRoleAsync(userId))
            return Result<TutorProfileDto>.Error("User already has a role assigned");

        var tutorProfile = new TutorProfile
        {
            UserId = userId,
            VerificationStatus = createTutorProfileDto.VerificationStatus,
            ExperienceYears = createTutorProfileDto.ExperienceYears,
        };

        await _tutorProfileRepository.Create(tutorProfile);
        
        var roleResult = await _userRoleService.AssignTutorRoleAsync(userId);
        if (!roleResult.IsSuccess)
            return Result<TutorProfileDto>.Error(roleResult.Errors.FirstOrDefault());

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

    public async Task<Result<List<TutorProfileDto>>> GetAllTutorProfileAsync()
    {
        var profiles = await _tutorProfileRepository.GetAll();
        if (profiles is null)
            return Result<List<TutorProfileDto>>.NotFound("Tutor profiles not found");
        return _mapper.Map<List<TutorProfileDto>>(profiles);
    }

    
}