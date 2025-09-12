using Ardalis.Result;
using MediatR;
using Tutor.Application.Features.Tutors.Dto;

namespace Tutor.Application.Features.Tutors.Approve_Tutor;

public record ApproveTutorCommand(int userId) : IRequest<Result<TutorProfileDto>>;
