using System;

namespace Tutor.Application.Features.Reviews.Dto;

public class ReviewDto
{
    public required int Id { get; set; }
    public required int StudentUserId { get; set; }
    public required string UserName { get; set; } 
    public  required int Rating { get; set; }
    public string? Description { get; set; }
    public DateTime CreatedAt { get; set; }

}