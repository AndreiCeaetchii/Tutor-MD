using System;
using System.ComponentModel.DataAnnotations;

namespace Tutor.Application.Features.Tutors.Dto;

public class CreateAvailabilityDto
{
    [Required(ErrorMessage = "Date is required")]
    [DataType(DataType.Date, ErrorMessage = "Invalid date format")]
    public DateOnly Date { get; set; }

    [Required(ErrorMessage = "Start time is required")]
    [DataType(DataType.Time, ErrorMessage = "Invalid time format")]
    public TimeOnly StartTime { get; set; }

    [Required(ErrorMessage = "End time is required")]
    [DataType(DataType.Time, ErrorMessage = "Invalid time format")]
    public TimeOnly EndTime { get; set; }
    
    [StringLength(50, ErrorMessage = "Timezone cannot exceed 50 characters")]
    [RegularExpression(@"^[A-Za-z_/]+$", ErrorMessage = "Invalid timezone format")]
    public string? Timezone { get; set; }

    public bool ActiveStatus { get; set; } = false;
}

