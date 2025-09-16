using Ardalis.Result;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Tutor.Application.Interfaces;

namespace Tutor.Application.Features.Tutors.DeleteAvilability;

public class DeleteAvailabilityHandler :  IRequestHandler<DeleteAvailabilityCommand, Result>
{
    private readonly IAvailabilityService  _availabilityService;

    public DeleteAvailabilityHandler(IAvailabilityService availabilityService)
    {
        _availabilityService = availabilityService;
    }
    public async Task<Result> Handle(DeleteAvailabilityCommand request, CancellationToken cancellationToken)
    {
        return await _availabilityService.DeleteAvailability(request.UserId, request.Id);
    }
}