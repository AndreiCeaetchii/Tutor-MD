using System.ComponentModel.DataAnnotations;

namespace Tutor.Application.Features.Tutors.Dto;

public class TutorSubjectRequestDto
{
    [Required]
    public string SubjectName { get; set; }
    
    public string? SubjectSlug { get; set; }
    
    [Required]
    [Range(0, 1000, ErrorMessage = "Price must be between 0 and 1000")]
    public decimal PricePerHour { get; set; }
    
    [Required]
    [MaxLength(3,ErrorMessage ="Wrong currency type")]
    public string Currency { get; set; }
}