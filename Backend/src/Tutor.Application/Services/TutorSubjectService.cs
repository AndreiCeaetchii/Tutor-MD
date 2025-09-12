using Ardalis.Result;
using AutoMapper;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tutor.Application.Features.Tutors.Dto;
using Tutor.Application.Interfaces;
using Tutor.Domain.Entities;
using Tutor.Domain.Interfaces;

namespace Tutor.Application.Services;

public class TutorSubjectService : ITutorSubjectService
{
    private readonly IGenericRepository2<TutorSubject> _tutorSubjectRepository;
    private readonly ISubjectService _subjectService;
    private readonly IMapper _mapper;

    public TutorSubjectService(
        IMapper mapper,
        IGenericRepository2<TutorSubject> tutorSubjectRepository,
        ISubjectService subjectService)
    {
        _tutorSubjectRepository = tutorSubjectRepository;
        _subjectService = subjectService;
        _mapper = mapper;
    }

    public async Task AddTutorSubjectsAsync(int tutorUserId, List<TutorSubjectRequestDto> subjectDtos)
    {
        foreach (var subjectDto in subjectDtos)
        {
            var subjectIdResult = await _subjectService.GetOrCreateSubjectIdAsync(
                subjectDto.SubjectName,
                subjectDto.SubjectSlug);

            if (subjectIdResult.IsSuccess)
            {
                await _tutorSubjectRepository.Create(new TutorSubject
                {
                    TutorUserId = tutorUserId,
                    SubjectId = subjectIdResult.Value,
                    Price = subjectDto.PricePerHour,
                    Currency = subjectDto.Currency
                });
            }
        }
    }

    public async Task<Result<TutorSubjectDto>> AddTutorSubjectAsync(int tutorUserId, TutorSubjectRequestDto subjectDto)
    {
        var subjectIdResult = await _subjectService.GetOrCreateSubjectIdAsync(
            subjectDto.SubjectName,
            subjectDto.SubjectSlug);

        if (!subjectIdResult.IsSuccess)
        {
            return Result<TutorSubjectDto>.Error("Could not create or find subject");
        }

        var tutorSubject = new TutorSubject
        {
            TutorUserId = tutorUserId,
            SubjectId = subjectIdResult.Value,
            Price = subjectDto.PricePerHour,
            Currency = subjectDto.Currency
        };
        await _tutorSubjectRepository.Create(tutorSubject);


        var dto = _mapper.Map<TutorSubjectDto>(tutorSubject);

        return Result<TutorSubjectDto>.Success(dto);
    }

    public async Task<Result<List<TutorSubjectDto>>> DeleteTutorSubjectAsync(int userId, int subjectId)
    {
        var tutorSubject =
            await _tutorSubjectRepository.FindAsyncDefault(sr => sr.SubjectId == subjectId && sr.TutorUserId == userId);

        if (tutorSubject is null)
            return Result<List<TutorSubjectDto>>.Error("Subject not found in tutor subjects");

        await _tutorSubjectRepository.Delete(tutorSubject);

       var tutorSubjects = await _tutorSubjectRepository.FindAsync(sr => sr.TutorUserId == userId );
       
       return _mapper.Map<List<TutorSubjectDto>>(tutorSubjects);
    }
    
    public async Task<Result<TutorSubjectDto>> UpdateTutorSubjectAsync(int userId,TutorSubjectDto tutorSubjectDto)
    {
        var tutorSubject =
            await _tutorSubjectRepository.FindAsyncDefault(sr => sr.SubjectId == tutorSubjectDto.SubjectId && sr.TutorUserId == userId);

        if (tutorSubject is null)
            return Result<TutorSubjectDto>.Error("Subject not found in tutor subjects");
        
        tutorSubject.Price = tutorSubjectDto.Price;
        tutorSubject.Currency = tutorSubjectDto.Currency;
        

        await _tutorSubjectRepository.Update(tutorSubject);
        
       
        return _mapper.Map<TutorSubjectDto>(tutorSubject);
    }

    
    public async Task<List<TutorSubject>> GetTutorSubjectsAsync(int tutorUserId) =>
        await _tutorSubjectRepository.FindAsync(ts => ts.TutorUserId == tutorUserId);

    public async Task RemoveTutorSubjectsAsync(int tutorUserId)
    {
        var subjects = await GetTutorSubjectsAsync(tutorUserId);
        foreach (var subject in subjects)
        {
            await _tutorSubjectRepository.Delete(subject);
        }
    }
}