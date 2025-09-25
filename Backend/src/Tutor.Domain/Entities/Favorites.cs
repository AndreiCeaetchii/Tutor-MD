using Tutor.Domain.Entities.Common;

namespace Tutor.Domain.Entities;

public class Favorites : Entity<int>
{
    public int TutorUserId { get; set; }
    public int StudentUserId { get; set; }
 
    // Navigation properties
    public virtual TutorProfile TutorProfile { get; set; }
    public virtual Student Student { get; set; }
}