using System;
using Tutor.Domain.Entities.Common;

namespace Tutor.Application.Features.Users.Dtos;

public record UserResponseDto
{
    public int Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string? Username { get; set; } = string.Empty;

    public required string Token { get; set; }
    
}