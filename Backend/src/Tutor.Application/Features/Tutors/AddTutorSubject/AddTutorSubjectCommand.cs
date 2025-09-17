using Ardalis.Result;
using MediatR;
using Tutor.Application.Features.Tutors.Dto;

namespace Tutor.Application.Features.Tutors.AddTutorSubject;

public record AddTutorSubjectCommand(TutorSubjectRequestDto tutorSubjectRequestDto, int userId) : IRequest<Result<TutorSubjectDto>>;
