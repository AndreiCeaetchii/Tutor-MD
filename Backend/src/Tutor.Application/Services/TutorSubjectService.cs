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

    public TutorSubjectService(
        IGenericRepository2<TutorSubject> tutorSubjectRepository,
        ISubjectService subjectService)
    {
        _tutorSubjectRepository = tutorSubjectRepository;
        _subjectService = subjectService;
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