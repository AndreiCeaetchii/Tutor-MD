using Ardalis.Result;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Tutor.Application.Features.Tutors.Dto;
using Tutor.Application.Interfaces;

namespace Tutor.Application.Features.Tutors.GetTutorAvailability;

public class GetTutorAvailabilityHandler :  IRequestHandler<GetTutorAvailabilityCommand, Result<List<AvailabilityDto>>>
{
    private readonly IAvailabilityService  _availabilityService;

    public GetTutorAvailabilityHandler(IAvailabilityService availabilityService)
    {
        _availabilityService = availabilityService;
    }
    public async Task<Result<List<AvailabilityDto>>> Handle(GetTutorAvailabilityCommand request, CancellationToken cancellationToken)
    {
        return await _availabilityService.GetAvailabilitiesByTutor(request.UserId);
    }
}