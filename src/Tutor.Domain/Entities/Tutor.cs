using System;
using System.Collections.Generic;

namespace Tutor.Domain.Entities;
public  class Tutor
{
    public int UserId { get; set; } // 1:1 with users
    public VerificationStatus VerificationStatus { get; set; }
    public int? ExperienceYears { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    // Navigation properties
    public User User { get; set; }
    public ICollection<TutorSubject> TutorSubjects { get; set; } = new List<TutorSubject>();
    public ICollection<TutorAvailabilityRule> AvailabilityRules { get; set; } = new List<TutorAvailabilityRule>();
    public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
    public ICollection<Review> Reviews { get; set; } = new List<Review>();
}
public enum VerificationStatus
{
    Pending,
    Verified,
    Rejected
}