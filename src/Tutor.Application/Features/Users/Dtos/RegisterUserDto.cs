using System;

namespace Tutor.Application.Features.Users.Dtos;

public record RegisterUserDto
{
    public required string Email { get; set; }
    public required string Password { get; set; }
    public required string Username { get; set; }
    public required string Name { get; set; }
    public required string Surname { get; set; }
    public required string PhoneNumber { get; set; }
    public required DateOnly DateOfBirth { get; set; }
    public string? Description { get; set; }
}