using System;
using Tutor.Domain.Entities;

namespace Tutor.Application.Features.Booking.Dto;

public class BookingDto
{
    public int Id { get; set; }
    public int TutorUserId { get; set; }
    public string TutorName { get; set; }
    public int StudentUserId { get; set; }
    public string StudentName { get; set; }
    public decimal Price { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public string Description { get; set; }
    public BookingStatus Status { get; set; }
    public string StudentPhoto { get; set; }
    public string TutorPhoto { get; set; }
}