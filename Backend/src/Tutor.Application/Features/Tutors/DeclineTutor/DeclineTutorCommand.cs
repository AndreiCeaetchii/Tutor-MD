using Ardalis.Result;
using MediatR;
using Tutor.Application.Features.Tutors.Dto;

namespace Tutor.Application.Features.Tutors.DeclineTutor;

public record DeclineTutorCommand(int userId) : IRequest<Result<TutorProfileDto>>;