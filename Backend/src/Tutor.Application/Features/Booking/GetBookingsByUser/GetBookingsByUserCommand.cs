using Ardalis.Result;
using MediatR;
using System.Collections.Generic;
using Tutor.Application.Features.Booking.Dto;

namespace Tutor.Application.Features.Booking.GetBookingsByUser;

public record GetBookingsByUserCommand(int UserId) : IRequest<Result<List<BookingDto>>>;