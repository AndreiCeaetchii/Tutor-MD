using System;
using Tutor.Domain.Entities.Common;

namespace Tutor.Application.Features.Users.Dtos;

public record UserResponseDto
{
    public UserId Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Surname { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public DateOnly DateOfBirth { get; set; }
    public string? Description { get; set; }
    public DateTime LastLoginAt { get; set; }
}