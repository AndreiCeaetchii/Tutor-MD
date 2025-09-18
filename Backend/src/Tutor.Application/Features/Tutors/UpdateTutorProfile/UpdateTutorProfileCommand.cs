using Ardalis.Result;
using MediatR;
using Tutor.Application.Features.Tutors.Dto;

namespace Tutor.Application.Features.Tutors.UpdateTutorProfile;

public record UpdateTutorProfileCommand(UpdateTutorProfileDto updateTutorProfileDto, int userId) : IRequest<Result<TutorProfileDto>>;