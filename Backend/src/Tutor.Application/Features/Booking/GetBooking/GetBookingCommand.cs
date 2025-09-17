using Ardalis.Result;
using MediatR;
using Tutor.Application.Features.Booking.Dto;

namespace Tutor.Application.Features.Booking.GetBooking;

public record GetBookingCommand(int UserId, int BookingId) : IRequest<Result<BookingDto>>;