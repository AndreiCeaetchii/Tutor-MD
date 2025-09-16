using Ardalis.Result;
using MediatR;
using System.Collections.Generic;
using Tutor.Application.Features.Tutors.Dto;

namespace Tutor.Application.Features.Tutors.GetTutorAvailability;

public record GetTutorAvailabilityCommand(int UserId) : IRequest<Result<List<AvailabilityDto>>>;
