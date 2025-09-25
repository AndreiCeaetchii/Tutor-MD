using System;
using System.Security.Claims;
using Tutor.Domain.Entities;

namespace Tutor.Application.Interfaces;

public interface ITokenService
{
    public string GenerateToken(User user, Role role);
    public string GenerateToken(User user);
    string GenerateRefreshToken();
    DateTime GetRefreshTokenExpiryTime();
    string HashRefreshToken(string refreshToken);
    bool VerifyRefreshToken(string providedToken, string storedHash);
    ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
}