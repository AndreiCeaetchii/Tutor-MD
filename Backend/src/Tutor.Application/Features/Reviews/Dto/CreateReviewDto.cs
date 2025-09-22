namespace Tutor.Application.Features.Reviews.Dto;

public class CreateReviewDto
{
    public required int BookingId { get; set; } 
    public required int Rating { get; set; }
    public string? Description { get; set; }
}