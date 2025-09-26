using Ardalis.Result;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Http;
using Google.Apis.Services;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tutor.Application.Features.Booking.Dto;
using Tutor.Application.Interfaces;

namespace Tutor.Infrastructure.Services;

public class GoogleCalendarService : IGoogleCalendarService
{
    private readonly ILogger<GoogleCalendarService> _logger;

    public GoogleCalendarService(ILogger<GoogleCalendarService> logger)
    {
        _logger = logger;
    }

    public async Task<Result<string>> CreateEventAsync(BookingDto booking, string accessToken)
    {
        try
        {
            // Create the calendar service using the access token
            var service = new CalendarService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = new GoogleAccessTokenInitializer(accessToken),
                ApplicationName = "Tutor Application"
            });
            var startDateTime = new DateTime(
                booking.Date.Year, 
                booking.Date.Month, 
                booking.Date.Day,
                booking.StartTime.Hour,
                booking.StartTime.Minute,
                booking.StartTime.Second
            );

            var endDateTime = new DateTime(
                booking.Date.Year,
                booking.Date.Month,
                booking.Date.Day,
                booking.EndTime.Hour,
                booking.EndTime.Minute, 
                booking.EndTime.Second
            );
            var calendarEvent = new Event
            {
                Summary = $"Tutoring Session - {booking.SubjectName}",
                Description = $"Tutoring session between {booking.TutorName} and {booking.StudentName} for {booking.SubjectName}",
                Start = new EventDateTime
                {
                    DateTime = startDateTime,
                    TimeZone = "UTC"
                },
                End = new EventDateTime
                {
                    DateTime = endDateTime,
                    TimeZone = "UTC"
                },
                Reminders = new Event.RemindersData
                {
                    UseDefault = false,
                    Overrides = new List<EventReminder>
                    {
                        new EventReminder { Method = "email", Minutes = 24 * 60 }, // 24 hours before
                        new EventReminder { Method = "popup", Minutes = 30 } // 30 minutes before
                    }
                }
            };

            var request = service.Events.Insert(calendarEvent, "primary");
            var createdEvent = await request.ExecuteAsync();

            _logger.LogInformation("Google Calendar event created: {EventId} for booking {BookingId}", 
                createdEvent.Id, booking.Id);

            return Result<string>.Success(createdEvent.Id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to create Google Calendar event for booking {BookingId}", booking.Id);
            return Result<string>.Error($"Failed to create calendar event: {ex.Message}");
        }
    }

    public async Task<Result> UpdateEventAsync(string eventId, BookingDto booking, string accessToken)
    {
        try
        {
            var service = new CalendarService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = new GoogleAccessTokenInitializer(accessToken),
                ApplicationName = "Tutor Application"
            });

            var existingEvent = await service.Events.Get("primary", eventId).ExecuteAsync();
            var startDateTime = new DateTime(
                booking.Date.Year, 
                booking.Date.Month, 
                booking.Date.Day,
                booking.StartTime.Hour,
                booking.StartTime.Minute,
                booking.StartTime.Second
            );

            var endDateTime = new DateTime(
                booking.Date.Year,
                booking.Date.Month,
                booking.Date.Day,
                booking.EndTime.Hour,
                booking.EndTime.Minute, 
                booking.EndTime.Second
            );
            existingEvent.Summary = $"Tutoring Session - {booking.SubjectName}";
            existingEvent.Description = $"Tutoring session between {booking.TutorName} and {booking.StudentName} for {booking.SubjectName}";
            existingEvent.Start = new EventDateTime { DateTime =startDateTime, TimeZone = "UTC" };
            existingEvent.End = new EventDateTime { DateTime = endDateTime, TimeZone = "UTC" };

            var request = service.Events.Update(existingEvent, "primary", eventId);
            await request.ExecuteAsync();

            _logger.LogInformation("Google Calendar event updated: {EventId} for booking {BookingId}", 
                eventId, booking.Id);

            return Result.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to update Google Calendar event {EventId} for booking {BookingId}", 
                eventId, booking.Id);
            return Result.Error($"Failed to update calendar event: {ex.Message}");
        }
    }

    public async Task<Result> DeleteEventAsync(string eventId, string accessToken)
    {
        try
        {
            var service = new CalendarService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = new GoogleAccessTokenInitializer(accessToken),
                ApplicationName = "Tutor Application"
            });

            await service.Events.Delete("primary", eventId).ExecuteAsync();

            _logger.LogInformation("Google Calendar event deleted: {EventId}", eventId);
            return Result.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to delete Google Calendar event {EventId}", eventId);
            return Result.Error($"Failed to delete calendar event: {ex.Message}");
        }
    }
}

// Helper class to initialize the service with access token
public class GoogleAccessTokenInitializer : IConfigurableHttpClientInitializer
{
    private readonly string _accessToken;

    public GoogleAccessTokenInitializer(string accessToken)
    {
        _accessToken = accessToken;
    }

    public void Initialize(ConfigurableHttpClient httpClient)
    {
        httpClient.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _accessToken);
    }
}