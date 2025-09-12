using Tutor.Domain.Entities.Common;

namespace Tutor.Domain.Entities;

public class Review : Entity<int>
{
    public int BookingId { get; set; } // one review per booking
    public int TutorUserId { get; set; }
    public int StudentUserId { get; set; }
    public int Rating { get; set; }
    public string Description { get; set; }

    // Navigation properties
    public virtual Booking Booking { get; set; }
    public virtual TutorProfile TutorProfile { get; set; }
    public virtual Student Student { get; set; }
}