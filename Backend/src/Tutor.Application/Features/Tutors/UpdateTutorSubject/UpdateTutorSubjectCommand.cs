using Ardalis.Result;
using MediatR;
using Tutor.Application.Features.Tutors.Dto;

namespace Tutor.Application.Features.Tutors.UpdateTutorSubject;

public record UpdateTutorSubjectCommand(int userId, TutorSubjectDto tutorSubjectDto): IRequest<Result<TutorSubjectDto>>;
