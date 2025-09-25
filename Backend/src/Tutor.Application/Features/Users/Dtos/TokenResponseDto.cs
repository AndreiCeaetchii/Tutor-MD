using System;

namespace Tutor.Application.Features.Users.Dtos;

public class TokenResponseDto
{
    public string Token { get; set; }
    public string RefreshToken { get; set; }
    public DateTime RefreshTokenExpiryTime { get; set; }
}