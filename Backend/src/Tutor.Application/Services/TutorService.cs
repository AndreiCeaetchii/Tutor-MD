using Ardalis.Result;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tutor.Application.Features.Tutors.Dto;
using Tutor.Application.Interfaces;
using Tutor.Domain.Entities;
using Tutor.Domain.Interfaces;

namespace Tutor.Application.Services;

public class TutorService : ITutorService
{
    private readonly IGenericRepository<User, int> _userRepository;
    private readonly IGenericRepository2<UserRole> _roleRepository;

    private readonly IGenericRepository2<TutorProfile> _tutorProfileRepository;
    private readonly IGenericRepository2<TutorSubject> _tutorSubjectRepository;
    private readonly IGenericRepository<SubjectCatalog, int> _subjectRepository;
    private readonly IMapper _mapper;

    public TutorService(
        IGenericRepository<User, int> userRepository,
        IGenericRepository2<UserRole> roleRepository,
        IGenericRepository2<TutorProfile> tutorProfileRepository,
        IGenericRepository2<TutorSubject> tutorSubjectRepository,
        IGenericRepository<SubjectCatalog, int> subjectRepository,
        IMapper mapper)
    {
        _userRepository = userRepository;
        _roleRepository = roleRepository;
        _tutorProfileRepository = tutorProfileRepository;
        _tutorSubjectRepository = tutorSubjectRepository;
        _subjectRepository = subjectRepository;
        _mapper = mapper;
    }
    public async Task<Result<TutorProfileDto>> CreateTutorProfileAsync(CreateTutorProfileDto createTutorProfileDto, int UserId)
    {
    var user = await _userRepository.FindAsyncDefault(u => u.Id == UserId);
        if (user is null) return Result<TutorProfileDto>.NotFound();

        var existingProfile = await _tutorProfileRepository.FindAsyncDefault(tp => tp.UserId == UserId);
        if (existingProfile is not null)
            return Result<TutorProfileDto>.Error("Tutor profile already exists");

        var subjectMappings = new List<(int SubjectId, decimal PricePerHour, string Currency)>();
        foreach (var subjectRequest in createTutorProfileDto.Subjects)
        {
            var subjectSlug = string.IsNullOrEmpty(subjectRequest.SubjectSlug)
                ? GenerateSlug(subjectRequest.SubjectName)
                : subjectRequest.SubjectSlug;

            var existingSubject = await _subjectRepository.FindAsyncDefault(s =>
                s.Name.ToLower() == subjectRequest.SubjectName.ToLower()
                || s.Slug.ToLower() == subjectSlug.ToLower());

            int subjectId;
            if (existingSubject is not null)
            {
                subjectId = existingSubject.Id;
            }
            else
            {
                var newSubject = new SubjectCatalog() { Name = subjectRequest.SubjectName, Slug = subjectSlug, };
                await _subjectRepository.Create(newSubject);
                subjectId = newSubject.Id;
            }

            subjectMappings.Add((subjectId, subjectRequest.PricePerHour, subjectRequest.Currency));
        }

        var tutorProfile = new TutorProfile
        {
            UserId = UserId,
            VerificationStatus = createTutorProfileDto.VerificationStatus,
            ExperienceYears = createTutorProfileDto.ExperienceYears,
        };
        await _tutorProfileRepository.Create(tutorProfile);

        var existingRole = await _roleRepository.FindAsyncDefault(tp => tp.UserId == UserId);
        if (existingRole is not null)
            return Result<TutorProfileDto>.Error("This user already has other role");


        var userRole = new UserRole() { UserId = UserId, RoleId = 2, AssignedAt = DateTime.UtcNow};
        await _roleRepository.Create(userRole);

        foreach (var (subjectId, pricePerHour, currency) in subjectMappings)
        {
            await _tutorSubjectRepository.Create(new TutorSubject
            {
                TutorUserId = tutorProfile.UserId, SubjectId = subjectId, Price = pricePerHour, Currency = currency,
            });
        }

        var result = await _tutorProfileRepository.FindAsyncDefault(tp => tp.UserId == tutorProfile.UserId);
        return _mapper.Map<TutorProfileDto>(result);
    }
    private string GenerateSlug(string name) =>
        name.ToLower().Replace(" ", "-").Replace("--", "-").Trim('-');
}