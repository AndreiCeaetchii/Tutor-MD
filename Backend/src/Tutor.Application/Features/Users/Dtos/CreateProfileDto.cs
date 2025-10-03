using System;
using System.ComponentModel.DataAnnotations;

namespace Tutor.Application.Features.Users.Dtos;

public class CreateProfileDto
{
    [Phone(ErrorMessage = "Invalid phone number format")]
    [StringLength(20, ErrorMessage = "Phone number cannot exceed 20 characters")]
    public string? Phone { get; set; }

    [Required(ErrorMessage = "Username is required")]
    [StringLength(50, MinimumLength = 3, ErrorMessage = "Username must be between 3 and 50 characters")]
    [RegularExpression(@"^[a-zA-Z0-9_]+$", ErrorMessage = "Username can only contain letters, numbers, and underscores")]
    public string? Username { get; set; }

    [Required(ErrorMessage = "First name is required")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "First name must be between 2 and 100 characters")]
    [RegularExpression(@"^[a-zA-Z\s\-']+$", ErrorMessage = "First name can only contain letters, spaces, hyphens, and apostrophes")]
    public string? FirstName { get; set; }

    [Required(ErrorMessage = "Last name is required")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Last name must be between 2 and 100 characters")]
    [RegularExpression(@"^[a-zA-Z\s\-']+$", ErrorMessage = "Last name can only contain letters, spaces, hyphens, and apostrophes")]
    public string? LastName { get; set; }

    [StringLength(500, ErrorMessage = "Bio cannot exceed 500 characters")]
    public string? Bio { get; set; }

    [StringLength(100, ErrorMessage = "City name cannot exceed 100 characters")]
    [RegularExpression(@"^[a-zA-Z\s\-']+$", ErrorMessage = "City name can only contain letters, spaces, hyphens, and apostrophes")]
    public string? City { get; set; }

    [StringLength(100, ErrorMessage = "Country name cannot exceed 100 characters")]
    [RegularExpression(@"^[a-zA-Z\s\-']+$", ErrorMessage = "Country name can only contain letters, spaces, hyphens, and apostrophes")]
    public string? Country { get; set; }

    [DataType(DataType.Date, ErrorMessage = "Invalid date format")]
    [Range(typeof(DateTime), "1/1/1900", "1/1/2100", ErrorMessage = "Birthdate must be between 1900 and 2100")]
    public DateTime? Birthdate { get; set; }
    
    public bool? IsActive { get; set; }
    
    public string? Email { get; set; }
    
    [DataType(DataType.DateTime, ErrorMessage = "Invalid datetime format")]
    public DateTime CreatedAt { get; set; }

    public bool TwoFactorEnabled { get; set; }
}