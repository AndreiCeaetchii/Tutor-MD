using Ardalis.Result;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Tutor.Application.Features.Booking.Dto;
using Tutor.Application.Interfaces;

namespace Tutor.Application.Features.Booking.UpdateBookingStatus;

public class UpdateBookingStatusHandler : IRequestHandler<UpdateBookingStatusCommand, Result<BookingDto>>
{
    private readonly IBookingService  _bookingService;

    public UpdateBookingStatusHandler(IBookingService bookingService)
    {
        _bookingService = bookingService;
    }
    
    public async Task<Result<BookingDto>> Handle(UpdateBookingStatusCommand request, CancellationToken cancellationToken)
    {
        return await _bookingService.UpdateBookingStatus(request.BookingId,request.UserId, request.Status);
    }
}