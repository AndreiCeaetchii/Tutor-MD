using Hangfire;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using Tutor.Application.Interfaces;
using Tutor.Domain.Entities;
using Tutor.Domain.Interfaces;

namespace Tutor.Application.Services.Background;

public class BookingNotificationService:IBookingNotificationService
{
    private readonly ILogger<BookingNotificationService> _logger;
    private readonly IServiceProvider _serviceProvider;
    private readonly TimeSpan _checkInterval = TimeSpan.FromMinutes(5); 
    private readonly IGenericRepository<Notification,int> _notificationRepository;

    public BookingNotificationService(
        ILogger<BookingNotificationService> logger,
        IGenericRepository<Notification,int> notificationRepository,
        IServiceProvider serviceProvider)
    {
        _logger = logger;
        _notificationRepository = notificationRepository;
        _serviceProvider = serviceProvider;
    }
    [AutomaticRetry(Attempts = 3)]
    public async Task CheckUpcomingBookings()
    {
        using var scope = _serviceProvider.CreateScope();
        var bookingService = scope.ServiceProvider.GetRequiredService<IBookingService>();
        
        var now = DateTime.Now;
        var startWindow = now.AddHours(0.2);
        var endWindow = now.AddHours(2);

        var upcomingBookings = await bookingService.GetBookingsStartingBetweenAsync(
            startWindow, endWindow, BookingStatus.Confirmed);

        foreach (var booking in upcomingBookings)
        {
            await CreateBookingReminderNotification(bookingService, booking);
        }
    }
    private async Task CreateBookingReminderNotification(IBookingService bookingService, Booking booking)
    {
        try
        {
            var notificationExists = await bookingService.NotificationExistsAsync(
                booking.Id, "BookingReminder", booking.StudentUserId);

            if (!notificationExists)
            {
                var studentNotification = new Notification
                {
                    RecipientUserId = booking.StudentUserId,
                    ActorUserId = booking.TutorUserId,
                    Type = "BookingReminder",
                    Payload = System.Text.Json.JsonSerializer.Serialize(new
                    {
                        BookingId = booking.Id,
                        StartTime = booking.TutorAvailabilityRule.StartTime,
                        EndTime = booking.TutorAvailabilityRule.EndTime,
                        Date = booking.TutorAvailabilityRule.Date,
                        Subject = booking.Subject?.Name,
                        TutorName =
                            $"{booking.TutorProfile?.User?.FirstName} {booking.TutorProfile?.User?.LastName}",
                        Status = booking.Status
                    }),
                    Status = NotificationStatus.Unread,
                    CreatedAt = DateTime.UtcNow
                };

                await bookingService.CreateNotificationAsync(studentNotification);
                
                var tutorNotification = new Notification
                {
                    RecipientUserId = booking.TutorUserId,
                    ActorUserId = booking.StudentUserId,
                    Type = "BookingReminder",
                    Payload = System.Text.Json.JsonSerializer.Serialize(new
                    {
                        BookingId = booking.Id,
                        StartTime = booking.TutorAvailabilityRule.StartTime,
                        EndTime = booking.TutorAvailabilityRule.EndTime,
                        Subject = booking.Subject?.Name,
                        Date = booking.TutorAvailabilityRule.Date,
                        StudentName = $"{booking.Student?.User?.FirstName} {booking.Student?.User?.LastName}",
                        Status = booking.Status
                    }),
                    Status = NotificationStatus.Unread,
                    CreatedAt = DateTime.UtcNow
                };
                await bookingService.CreateNotificationAsync(tutorNotification);

                _logger.LogInformation("Created reminder notifications for booking {BookingId}", booking.Id);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating notification for booking {BookingId}", booking.Id);
        }
    }
    public async Task CreateBookingChangeNotification(Booking booking)
    {
        try
        {
                var studentNotification = new Notification
                {
                    RecipientUserId = booking.StudentUserId,
                    ActorUserId = booking.TutorUserId,
                    Type = "BookingUpdatedStatus",
                    Payload = System.Text.Json.JsonSerializer.Serialize(new
                    {
                        BookingId = booking.Id,
                        StartTime = booking.TutorAvailabilityRule.StartTime,
                        EndTime = booking.TutorAvailabilityRule.EndTime,
                        Date = booking.TutorAvailabilityRule.Date,
                        Subject = booking.Subject?.Name,
                        TutorName =
                            $"{booking.TutorProfile?.User?.FirstName} {booking.TutorProfile?.User?.LastName}",
                        Status = booking.Status
                    }),
                    Status = NotificationStatus.Unread,
                    CreatedAt = DateTime.UtcNow
                };

                await _notificationRepository.Create(studentNotification);
                
                var tutorNotification = new Notification
                {
                    RecipientUserId = booking.TutorUserId,
                    ActorUserId = booking.StudentUserId,
                    Type = "BookingUpdatedStatus",
                    Payload = System.Text.Json.JsonSerializer.Serialize(new
                    {
                        BookingId = booking.Id,
                        StartTime = booking.TutorAvailabilityRule.StartTime,
                        EndTime = booking.TutorAvailabilityRule.EndTime,
                        Subject = booking.Subject?.Name,
                        Date = booking.TutorAvailabilityRule.Date,
                        StudentName = $"{booking.Student?.User?.FirstName} {booking.Student?.User?.LastName}",
                        Status = booking.Status
                    }),
                    Status = NotificationStatus.Unread,
                    CreatedAt = DateTime.UtcNow
                };
                await _notificationRepository.Create(tutorNotification);

                _logger.LogInformation("Status updated for booking {BookingId}", booking.Id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating notification for booking {BookingId}", booking.Id);
        }
    }
    public async Task NewBookingNotification(Booking booking)
    {
        try
        {
                
                var tutorNotification = new Notification
                {
                    RecipientUserId = booking.TutorUserId,
                    ActorUserId = booking.StudentUserId,
                    Type = "NewBooking",
                    Payload = System.Text.Json.JsonSerializer.Serialize(new
                    {
                        BookingId = booking.Id,
                        StartTime = booking.TutorAvailabilityRule.StartTime,
                        EndTime = booking.TutorAvailabilityRule.EndTime,
                        Subject = booking.Subject?.Name,
                        Date = booking.TutorAvailabilityRule.Date,
                        StudentName = $"{booking.Student?.User?.FirstName} {booking.Student?.User?.LastName}",
                        Status = booking.Status
                    }),
                    Status = NotificationStatus.Unread,
                    CreatedAt = DateTime.UtcNow
                };
                await _notificationRepository.Create(tutorNotification);

                _logger.LogInformation("New Booking notifications for booking {BookingId}", booking.Id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating notification for booking {BookingId}", booking.Id);
        }
    }
}