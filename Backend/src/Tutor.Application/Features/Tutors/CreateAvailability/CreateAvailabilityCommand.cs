using Ardalis.Result;
using MediatR;
using Tutor.Application.Features.Tutors.Dto;

namespace Tutor.Application.Features.Tutors.CreateAvailability;

public record CreateAvailabilityCommand(int UserId, CreateAvailabilityDto CreateAvailabilityDto): IRequest<Result<AvailabilityDto>>;
