using System;
using Tutor.Domain.Entities;

namespace Tutor.Application.Features.Booking.Dto;

public class CreateBookingDto
{
    public int TutorUserId {get; set;}
    public int SubjectId {get; set;}
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    
    public string? Description { get; set; }
}