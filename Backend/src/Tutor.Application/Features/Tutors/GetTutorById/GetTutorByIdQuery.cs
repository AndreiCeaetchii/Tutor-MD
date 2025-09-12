using Ardalis.Result;
using MediatR;
using Tutor.Application.Features.Tutors.Dto;

namespace Tutor.Application.Features.Tutors.GetTutorById;

public record GetTutorByIdQuery(int UserId)
    : IRequest<Result<TutorProfileDto>>;