using Ardalis.Result;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Tutor.Application.Interfaces;

namespace Tutor.Application.Features.Booking.AddToCalendar;

public class AddToCalendarHandler : IRequestHandler<AddToCalendarCommand, Result>
{
    private readonly IBookingService  _bookingService;

    public AddToCalendarHandler(IBookingService bookingService)
    {
        _bookingService = bookingService;
    }
    public async Task<Result> Handle(AddToCalendarCommand request, CancellationToken cancellationToken)
    {
        return await _bookingService.AddToCalendar(request.BookingId, request.UserId,request.AccessToken);
    }
}