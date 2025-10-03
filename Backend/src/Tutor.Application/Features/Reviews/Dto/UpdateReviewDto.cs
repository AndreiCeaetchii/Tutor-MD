using System.ComponentModel.DataAnnotations;

namespace Tutor.Application.Features.Reviews.Dto;

public class UpdateReviewDto
{
    [Required(ErrorMessage = "Rating is required")]
    [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5")]
    public int? Rating { get; set; }

    [StringLength(1000, ErrorMessage = "Description cannot exceed 1000 characters")]
    [MinLength(10, ErrorMessage = "Description must be at least 10 characters long")]
    [DataType(DataType.MultilineText)]
    public string? Description { get; set; }
}
