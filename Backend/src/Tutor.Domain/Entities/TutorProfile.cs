using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Tutor.Domain.Entities;

public class TutorProfile
{
    public int UserId { get; set; } // 1:1 with users
    public VerificationStatus VerificationStatus { get; set; }

    public int? ExperienceYears { get; set; }

    // Navigation properties
    public User User { get; set; }
    public ICollection<TutorSubject> TutorSubjects { get; set; } = new List<TutorSubject>();
    public ICollection<TutorAvailabilityRule> AvailabilityRules { get; set; } = new List<TutorAvailabilityRule>();
    public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
    public ICollection<Review> Reviews { get; set; } = new List<Review>();
}

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum VerificationStatus
{
    Pending,
    Verified,
    Rejected
}