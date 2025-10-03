namespace Tutor.Application.Features.Users.Dtos;

public class ResetPasswordDto
{
    public required string NewPassword {get;set;}
    public required string Token {get;set;}
}