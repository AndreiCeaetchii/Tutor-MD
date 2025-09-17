using Ardalis.Result;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Tutor.Application.Features.Booking.Dto;
using Tutor.Application.Interfaces;

namespace Tutor.Application.Features.Booking.GetBookingsByUser;

public class GetBookingsByUserHandler : IRequestHandler<GetBookingsByUserCommand, Result<List<BookingDto>>>
{
    private readonly IBookingService  _bookingService;

    public GetBookingsByUserHandler(IBookingService bookingService)
    {
        _bookingService = bookingService;
    }
    public async Task<Result<List<BookingDto>>> Handle(GetBookingsByUserCommand request, CancellationToken cancellationToken)
    {
        return await _bookingService.GetBookingsByUSer(request.UserId);
    }
}