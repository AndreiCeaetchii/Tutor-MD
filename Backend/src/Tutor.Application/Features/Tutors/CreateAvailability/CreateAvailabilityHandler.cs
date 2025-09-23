using Ardalis.Result;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Tutor.Application.Features.Tutors.Dto;
using Tutor.Application.Interfaces;

namespace Tutor.Application.Features.Tutors.CreateAvailability;

public class CreateAvailabilityHandler : IRequestHandler<CreateAvailabilityCommand, Result<AvailabilityDto>>
{
    private readonly IAvailabilityService  _availabilityService;

    public CreateAvailabilityHandler(IAvailabilityService availabilityService)
    {
        _availabilityService = availabilityService;
    }
    public async Task<Result<AvailabilityDto>> Handle(CreateAvailabilityCommand request, CancellationToken cancellationToken)
    {
        return await _availabilityService.CreateAvailability(request.UserId, request.CreateAvailabilityDto);
    }
}