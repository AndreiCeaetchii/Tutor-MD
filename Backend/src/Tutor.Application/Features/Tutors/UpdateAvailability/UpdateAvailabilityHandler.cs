using Ardalis.Result;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Tutor.Application.Features.Tutors.Dto;
using Tutor.Application.Interfaces;

namespace Tutor.Application.Features.Tutors.UpdateAvailability;

public class UpdateAvailabilityHandler : IRequestHandler<UpdateAvailabilityCommand, Result<AvailabilityDto>>
{
    private readonly IAvailabilityService  _availabilityService;

    public UpdateAvailabilityHandler(IAvailabilityService availabilityService)
    {
        _availabilityService = availabilityService;
    }
    public async Task<Result<AvailabilityDto>> Handle(UpdateAvailabilityCommand request, CancellationToken cancellationToken)
    {
        return await _availabilityService.UpdateAvailability(request.UserId, request.AvailabilityDto);
    }
}