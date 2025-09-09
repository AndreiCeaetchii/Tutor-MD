using System;

namespace Tutor.Application.Features.Users.Dtos;

public record RegisterUserDto
{
    public required string Email { get; set; }
    public required string Password { get; set; }
}