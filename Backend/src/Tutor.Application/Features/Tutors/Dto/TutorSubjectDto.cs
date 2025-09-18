namespace Tutor.Application.Features.Tutors.Dto;

public class TutorSubjectDto
{
    public int SubjectId { get; set; }
    public string SubjectName { get; set; }
    public string SubjectSlug { get; set; }
    public decimal Price { get; set; }
    public string Currency { get; set; }
}