using System.Collections.Generic;
using Tutor.Domain.Entities.Common;

namespace Tutor.Domain.Entities;

public class SubjectCatalog : Entity<int>
{
    public string Name { get; set; }
    public string Slug { get; set; }

    // Navigation properties
    public virtual ICollection<TutorSubject> TutorSubjects { get; set; } = new List<TutorSubject>();
    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();
}