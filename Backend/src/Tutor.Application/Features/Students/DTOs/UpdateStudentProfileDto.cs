using System;
using System.ComponentModel.DataAnnotations;
using Tutor.Application.Features.Tutors.Dto;

namespace Tutor.Application.Features.Students.DTOs;

public class UpdateStudentProfileDto
{
    [Phone(ErrorMessage = "Invalid phone number format")]
    [StringLength(20, ErrorMessage = "Phone number cannot exceed 20 characters")]
    public string? Phone { get; set; }

    [StringLength(50, MinimumLength = 3, ErrorMessage = "Username must be between 3 and 50 characters")]
    [RegularExpression(@"^[a-zA-Z0-9_\.]+$", ErrorMessage = "Username can only contain letters, numbers, underscores, and dots")]
    public string? Username { get; set; }

    [StringLength(100, ErrorMessage = "First name cannot exceed 100 characters")]
    [RegularExpression(@"^[a-zA-Z\s\-'\.]+$", ErrorMessage = "First name can only contain letters, spaces, hyphens, apostrophes, and dots")]
    public string? FirstName { get; set; }

    [StringLength(100, ErrorMessage = "Last name cannot exceed 100 characters")]
    [RegularExpression(@"^[a-zA-Z\s\-'\.]+$", ErrorMessage = "Last name can only contain letters, spaces, hyphens, apostrophes, and dots")]
    public string? LastName { get; set; }

    [StringLength(1000, ErrorMessage = "Bio cannot exceed 1000 characters")]
    public string? Bio { get; set; }

    [DataType(DataType.Date, ErrorMessage = "Invalid date format")]
    public DateTime? Birthdate { get; set; }

    [StringLength(100, ErrorMessage = "City name cannot exceed 100 characters")]
    public string? City { get; set; }

    [StringLength(100, ErrorMessage = "Country name cannot exceed 100 characters")]
    public string? Country { get; set; }
    [Range(1, 12, ErrorMessage = "Grade must be between 1 and 12")]
    public int? Grade { get; set; }

    [Range(1, 20, ErrorMessage = "Class must be between 1 and 20")]
    public int? Class { get; set; }
}
