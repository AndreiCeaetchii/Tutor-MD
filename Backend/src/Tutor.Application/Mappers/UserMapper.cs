using System;
using Tutor.Application.Features.Users.Dtos;
using Tutor.Domain.Entities;

public static class UserMapper
{
    public static User ToEntity(RegisterUserDto dto, string hashedPassword)
    {
        return new User
        {
            Email = dto.Email,
            PasswordHash = hashedPassword,
            IsActive = true,
            LastLoginAt = DateTime.UtcNow
        };
    }
}