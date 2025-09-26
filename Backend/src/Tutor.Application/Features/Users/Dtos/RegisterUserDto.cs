using System;
using System.ComponentModel.DataAnnotations;

namespace Tutor.Application.Features.Users.Dtos;

public record RegisterUserDto
{
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email address format")]
    [StringLength(254, ErrorMessage = "Email cannot exceed 254 characters")]
    [DataType(DataType.EmailAddress, ErrorMessage = "Invalid email format")]
    public required string Email { get; set; }

    [Required(ErrorMessage = "Password is required")]
    [StringLength(100, MinimumLength = 8, ErrorMessage = "Password must be between 8 and 100 characters")]
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,}$", 
        ErrorMessage = "Password must contain at least one uppercase letter, one lowercase letter, one number, and one special character")]
    [DataType(DataType.Password)]
    public required string Password { get; set; }
    
    [Required(ErrorMessage = "Role ID is required")]
    [Range(1, 3, ErrorMessage = "Role ID must be a positive number")]
    public required int RoleId { get; set; }
}