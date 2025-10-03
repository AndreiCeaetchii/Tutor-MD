using System;
using System.ComponentModel.DataAnnotations;

namespace Tutor.Application.Features.Tutors.Dto;

public class UpdateTutorProfileDto
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
    [CustomValidation(typeof(UpdateTutorProfileDto), nameof(ValidateBirthdate))]
    public DateTime? Birthdate { get; set; }

    [StringLength(100, ErrorMessage = "City name cannot exceed 100 characters")]
    public string? City { get; set; }

    [StringLength(100, ErrorMessage = "Country name cannot exceed 100 characters")]
    public string? Country { get; set; }

    [Range(0, 60, ErrorMessage = "Experience years must be between 0 and 60")]
    public int? ExperienceYears { get; set; }

    [Range(1,7, ErrorMessage = "Invalid working location value")]
    public int WorkingLocation { get; set; }

    public static ValidationResult? ValidateBirthdate(DateTime? birthdate, ValidationContext context)
    {
        if (!birthdate.HasValue)
            return ValidationResult.Success;

        if (birthdate.Value > DateTime.Now.AddYears(-18))
        {
            return new ValidationResult("Tutor must be at least 18 years old");
        }

        if (birthdate.Value < DateTime.Now.AddYears(-100))
        {
            return new ValidationResult("Tutor age seems unrealistic");
        }

        return ValidationResult.Success;
    }
}
