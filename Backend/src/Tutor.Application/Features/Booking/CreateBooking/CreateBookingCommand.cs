using Ardalis.Result;
using MediatR;
using Tutor.Application.Features.Booking.Dto;

namespace Tutor.Application.Features.Booking.CreateBooking;

public record CreateBookingCommand(int UserId, CreateBookingDto CreateBookingDto) : IRequest<Result<BookingDto>>;
