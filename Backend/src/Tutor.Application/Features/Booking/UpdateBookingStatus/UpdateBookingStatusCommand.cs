using Ardalis.Result;
using MediatR;
using Tutor.Application.Features.Booking.Dto;
using Tutor.Domain.Entities;

namespace Tutor.Application.Features.Booking.UpdateBookingStatus;

public record UpdateBookingStatusCommand(int UserId, int BookingId, BookingStatus Status) : IRequest<Result<BookingDto>>;
