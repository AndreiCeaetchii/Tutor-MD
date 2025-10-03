using System;
using System.ComponentModel.DataAnnotations;

namespace Tutor.Application.Features.Reviews.Dto;

public class CreateReviewDto
{
    [Required(ErrorMessage = "Booking ID is required")]
    [Range(1, int.MaxValue, ErrorMessage = "Booking ID must be a positive number")]
    public required int BookingId { get; set; }

    [Required(ErrorMessage = "Rating is required")]
    [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5")]
    public required int Rating { get; set; }

    [StringLength(500, ErrorMessage = "Description cannot exceed 1000 characters")]
    [DataType(DataType.MultilineText)]
    public string? Description { get; set; }
}