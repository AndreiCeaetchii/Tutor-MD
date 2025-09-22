using Ardalis.Result;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Tutor.Application.Features.Booking.Dto;
using Tutor.Application.Interfaces;

namespace Tutor.Application.Features.Booking.GetBooking;

public class GetBookingHandler : IRequestHandler<GetBookingCommand, Result<BookingDto>>
{
    private readonly IBookingService  _bookingService;

    public GetBookingHandler(IBookingService bookingService)
    {
        _bookingService = bookingService;
    }
    public async Task<Result<BookingDto>> Handle(GetBookingCommand request, CancellationToken cancellationToken)
    {
        return await _bookingService.GetBookingById(request.BookingId,request.UserId);
    }
}