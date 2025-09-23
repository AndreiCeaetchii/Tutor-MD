using Ardalis.Result;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Tutor.Application.Features.Booking.Dto;
using Tutor.Application.Interfaces;

namespace Tutor.Application.Features.Booking.CreateBooking;

public class CreateBookingHandler :  IRequestHandler<CreateBookingCommand, Result<BookingDto>>
{
    private readonly IBookingService  _bookingService;

    public CreateBookingHandler(IBookingService bookingService)
    {
        _bookingService = bookingService;
    }
    public async Task<Result<BookingDto>> Handle(CreateBookingCommand request, CancellationToken cancellationToken)
    {
        return await _bookingService.CreateBooking(request.CreateBookingDto, request.UserId);
    }
}