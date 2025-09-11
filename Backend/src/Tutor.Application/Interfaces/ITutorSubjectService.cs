using Ardalis.Result;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tutor.Application.Features.Tutors.Dto;
using Tutor.Domain.Entities;

namespace Tutor.Application.Interfaces;

public interface ITutorSubjectService
{
    Task AddTutorSubjectsAsync(int tutorUserId, List<TutorSubjectRequestDto> subjectDtos);
    Task<List<TutorSubject>> GetTutorSubjectsAsync(int tutorUserId);
    Task RemoveTutorSubjectsAsync(int tutorUserId);
    Task<Result<TutorSubjectDto>> AddTutorSubjectAsync(int tutorUserId, TutorSubjectRequestDto subjectDto);
    Task<Result<List<TutorSubjectDto>>> DeleteTutorSubjectAsync(int userId, int subjectId);
}