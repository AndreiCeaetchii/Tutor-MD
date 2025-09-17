using Ardalis.Result;
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

}