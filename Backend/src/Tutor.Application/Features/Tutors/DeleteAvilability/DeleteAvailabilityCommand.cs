using Ardalis.Result;
using MediatR;

namespace Tutor.Application.Features.Tutors.DeleteAvilability;

public record DeleteAvailabilityCommand(int UserId, int Id) : IRequest<Result>;
