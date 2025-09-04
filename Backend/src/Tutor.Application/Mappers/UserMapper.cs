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
            Username = dto.Username,
            FirstName = dto.Name,
            LastName = dto.Surname,
            Phone = dto.PhoneNumber,
            Birthdate = dto.DateOfBirth.ToDateTime(TimeOnly.MinValue),
            Bio = dto.Description,
            IsActive = true,
            LastLoginAt = DateTime.UtcNow
        };
    }
}