using Ardalis.Result;
using System.Threading.Tasks;
using Tutor.Application.Features.Booking.Dto;

namespace Tutor.Application.Interfaces;

public interface IGoogleCalendarService
{
    Task<Result<string>> CreateEventAsync(BookingDto booking, string accessToken);
    Task<Result> UpdateEventAsync(string eventId, BookingDto booking, string accessToken);
    Task<Result> DeleteEventAsync(string eventId, string accessToken);
}