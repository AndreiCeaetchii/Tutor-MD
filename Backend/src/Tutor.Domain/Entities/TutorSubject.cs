namespace Tutor.Domain.Entities;

public class TutorSubject
{
    public int TutorUserId { get; set; }
    public int SubjectId { get; set; }
    public decimal Price { get; set; }
    public string Currency { get; set; } // ISO 4217 (e.g., EUR, USD)

    // Navigation properties
    public virtual TutorProfile TutorProfile { get; set; }
    public virtual SubjectCatalog Subject { get; set; }
}