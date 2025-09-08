namespace Tutor.Application.Features.Users.Dtos;

public class AuthResponseDto
{
    public required int UserId { get; set; }
    public required string Email { get; set; } 
    public required string Token { get; set; } 
}