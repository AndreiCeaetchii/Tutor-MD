using System;
using Tutor.Application.Features.Users.Dtos;
using Tutor.Domain.Entities;

namespace Tutor.Application.Mappers;
public static class UserMapper
{
    public static User ToEntity(RegisterUserDto dto)
    {
        return new User
        {
            Email = dto.Email,
            IsActive = true,
            LastLoginAt = DateTime.UtcNow
        };
    }

    public static UserResponseDto ToResponseDto(User user, string token)
    {
        return new UserResponseDto { Email = user.Email, Token = token, Username = user.Username, Id = user.Id };
    }
    
}