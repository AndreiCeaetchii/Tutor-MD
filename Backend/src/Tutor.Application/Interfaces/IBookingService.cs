using Ardalis.Result;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tutor.Application.Features.Booking.Dto;
using Tutor.Domain.Entities;

namespace Tutor.Application.Interfaces;

public interface IBookingService
{
    Task<Result<BookingDto>> CreateBooking(CreateBookingDto createBookingDto, int userId);
    Task<Result<BookingDto>> GetBookingById(int bookingId, int userId);
    Task<Result<BookingDto>> UpdateBookingStatus(int bookingId, int userId, BookingStatus newStatus);
    Task<Result<List<BookingDto>>> GetBookingsByUSer(int userId);
    Task<List<Booking>> GetBookingsStartingBetweenAsync(DateTime startWindow, DateTime endWindow, BookingStatus status);
    Task<bool> NotificationExistsAsync(int bookingId, string notificationType, int recipientUserId);
    Task CreateNotificationAsync(Notification notification);
    Task<Result> AddToCalendar(int bookingId, int userId, string googleAccessToken);
}