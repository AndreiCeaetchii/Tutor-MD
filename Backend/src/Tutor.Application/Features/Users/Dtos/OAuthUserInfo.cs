namespace Tutor.Application.Features.Users.Dtos;
public class OAuthUserInfo
{
    public string ProviderId { get; set; } = string.Empty;
    public string Provider { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? FullName { get; set; }
    public string? PictureUrl { get; set; }
}