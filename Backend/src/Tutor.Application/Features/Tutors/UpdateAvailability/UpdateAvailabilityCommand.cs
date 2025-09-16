using Ardalis.Result;
using MediatR;
using Tutor.Application.Features.Tutors.Dto;

namespace Tutor.Application.Features.Tutors.UpdateAvailability;

public record UpdateAvailabilityCommand(int UserId, AvailabilityDto AvailabilityDto): IRequest<Result<AvailabilityDto>>;