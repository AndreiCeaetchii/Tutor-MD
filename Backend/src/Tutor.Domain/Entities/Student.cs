using System;
using System.Collections.Generic;

namespace Tutor.Domain.Entities;

public class Student
{
    public int UserId { get; set; } // 1:1 with users
    public int? Grade { get; set; }
    // Navigation properties
    public virtual User User { get; set; }
    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();
    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();
}
