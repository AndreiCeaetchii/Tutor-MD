using System;
using Tutor.Application.Features.Users.Dtos;
using Tutor.Domain.Entities;

namespace Tutor.Application.Mappers;

public static class UserMapper
{
    public static User ToEntity(this RegisterUserDto dto)
    {
        return new User { Email = dto.Email, LastLoginAt = DateTime.UtcNow };
    }

    public static UserResponseDto ToResponseDto(this User user, string token)
    {
        return new UserResponseDto { Email = user.Email, Token = token, Username = user.Username, Id = user.Id };
    }
    public static UserResponseDto ToResponseDto(this User user, string token, int roleId)
    {
        return new UserResponseDto { Email = user.Email, Token = token, Username = user.Username, Id = user.Id,  RoleId = roleId };
    }
}