using System;

namespace Tutor.Application.Features.Users.Dtos;

public record RegisterUserAuthDto
{
    public required string Provider { get; init; }
    public required string AccessToken { get; init; }
    public required string Email { get; init; }
    
    public required int RoleId { get; init; }
}