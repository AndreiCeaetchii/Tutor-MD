using System;
using System.ComponentModel.DataAnnotations;
using Tutor.Domain.Entities;

namespace Tutor.Application.Features.Booking.Dto;

public class CreateBookingDto
{
    [Required(ErrorMessage = "Tutor ID is required")]
    [Range(1, int.MaxValue, ErrorMessage = "Tutor ID must be a positive number")]
    public int TutorUserId {get; set;}
    [Required(ErrorMessage = "Subject ID is required")]
    [Range(1, int.MaxValue, ErrorMessage = "Subject ID must be a positive number")]
    public int SubjectId {get; set;}
    [Required(ErrorMessage = "Availability ID is required")]
    [Range(1, int.MaxValue, ErrorMessage = "Availability ID must be a positive number")]
    public int AvailabilityRuleId {get; set;}
    
    [StringLength(200, ErrorMessage = "Description cannot exceed 1000 characters")]
    [DataType(DataType.MultilineText)]
    public string? Description { get; set; }
}