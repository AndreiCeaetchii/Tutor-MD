using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Tutor.Domain.Entities;

public class TutorProfile
{
    public int UserId { get; set; } // 1:1 with users
    public VerificationStatus VerificationStatus { get; set; }

    public int? ExperienceYears { get; set; }

    // Navigation properties
    public virtual User User { get; set; }
    public virtual ICollection<TutorSubject> TutorSubjects { get; set; } = new List<TutorSubject>();
    public virtual ICollection<TutorAvailabilityRule> AvailabilityRules { get; set; } = new List<TutorAvailabilityRule>();
    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();
    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();
}

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum VerificationStatus
{
    Pending,
    Verified,
    Rejected
}