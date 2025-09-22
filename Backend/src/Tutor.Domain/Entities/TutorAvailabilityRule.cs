using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.JavaScript;
using Tutor.Domain.Entities.Common;

namespace Tutor.Domain.Entities;

public class TutorAvailabilityRule : Entity<int>
{
    public int TutorUserId { get; set; }
    public DateOnly Date { get; set; } 
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }
    public bool ActiveStatus { get; set; }
    // Navigation properties
    public virtual TutorProfile TutorProfile { get; set; }
    
    public virtual ICollection<Booking> Bookings { get; set; }
}