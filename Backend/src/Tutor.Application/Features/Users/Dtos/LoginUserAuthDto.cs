namespace Tutor.Application.Features.Users.Dtos;

public class LoginUserAuthDto
{
    public required string Provider { get; init; }
    public required string AccessToken { get; init; }
    public required string Email { get; init; }
    
    public string? MfaCode { get; init; }
}