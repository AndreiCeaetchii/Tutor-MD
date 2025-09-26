using Ardalis.Result;
using MediatR;

namespace Tutor.Application.Features.Booking.AddToCalendar;

public record AddToCalendarCommand(int UserId, int BookingId, string AccessToken): IRequest<Result>;