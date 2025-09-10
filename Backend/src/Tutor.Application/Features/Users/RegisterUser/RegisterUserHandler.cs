using Ardalis.Result;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using Tutor.Application.Features.Users.Dtos;
using Tutor.Application.Interfaces;
using Tutor.Domain.Entities;
using Tutor.Domain.Entities.Common;
using Tutor.Domain.Interfaces;

namespace Tutor.Application.Features.Users;

public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, Result<UserResponseDto>>
{
    private readonly IGenericRepository<User, int> _userRepository;  
    private readonly IPasswordHasher _passwordHasher;

    public RegisterUserCommandHandler(
        IGenericRepository<User, int> userRepository,
        IPasswordHasher passwordHasher)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
    }


    public async Task<Result<UserResponseDto>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var registerDto = request.RegisterUserDto;

        // Check if user already exists using the repository
        var existingUser = await _userRepository.FindAsyncDefault(u => 
            u.Email == registerDto.Email || u.Username == registerDto.Username);

        if (existingUser != null)
        {
            return Result<UserResponseDto>.Failure("User with this email or username already exists");
        }

        // Hash password
        var salt = _passwordHasher.GenerateSalt();
        var hashedPassword = _passwordHasher.HashPassword(registerDto.Password, salt);

        // Create user entity
        var user = new User
        {
            Id = Guid.NewGuid().ToString(),
            Email = registerDto.Email,
            HashedPassword = hashedPassword,
            SaltPassword = salt,
            Username = registerDto.Username,
            Name = registerDto.Name,
            Surname = registerDto.Surname,
            PhoneNumber = registerDto.PhoneNumber,
            DateOfBirth = registerDto.DateOfBirth,
            Description = registerDto.Description,
            IsActive = true,
            LastLoginAt = DateTime.UtcNow
        };

        // Add to database using repository
        await _userRepository.Create(user);
        
        // Note: You might need to call SaveChanges if your repository doesn't do it automatically
        // This depends on your repository implementation

        // Return response
        var response = new UserResponseDto
        {
            Id = user.Id.Value,
            Email = user.Email,
            Username = user.Username,
            Name = user.Name,
            Surname = user.Surname,
            PhoneNumber = user.PhoneNumber,
            DateOfBirth = user.DateOfBirth,
            Description = user.Description,
            LastLoginAt = user.LastLoginAt
        };

        return Result<UserResponseDto>.Success(response);
    }
}