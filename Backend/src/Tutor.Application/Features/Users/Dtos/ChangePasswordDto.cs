namespace Tutor.Application.Features.Users.Dtos;

public class ChangePasswordDto
{
    public required string CurrentPassword { get; set; }
    public required string NewPassword { get; set; }
}