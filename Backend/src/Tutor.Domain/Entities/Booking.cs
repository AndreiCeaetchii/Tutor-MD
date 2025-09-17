using System;
using Tutor.Domain.Entities.Common;

namespace Tutor.Domain.Entities;

public class Booking : Entity<int>
{
    public int TutorUserId { get; set; }
    public int StudentUserId { get; set; }
    public int SubjectId { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    
    public string? Description { get; set; }
    public BookingStatus Status { get; set; }

    // Navigation properties
    public virtual TutorProfile TutorProfile { get; set; }
    public virtual Student Student { get; set; }
    public virtual SubjectCatalog Subject { get; set; }
    public virtual Review Review { get; set; }
}

public enum BookingStatus
{
    Pending,
    Confirmed,
    Cancelled,
    Completed,
    Finished
}