using Ardalis.Result;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Tutor.Application.Features.Users.Dtos;
using Tutor.Application.Interfaces;
using Tutor.Domain.Entities;
using Tutor.Domain.Interfaces;

namespace Tutor.Application.Features.Users.RegisterUser;

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

        // Check if user already exists
        var existingUser = await _userRepository.FindAsyncDefault(u => 
            u.Email == registerDto.Email || u.Username == registerDto.Username);

        if (existingUser != null)
        {
            return Result<UserResponseDto>.Error("User with this email or username already exists");
        }

        // Hash password (salt is embedded internally)
        var hashedPassword = _passwordHasher.HashPassword(registerDto.Password);

        // Map DTO to User entity using the mapper
        var user = UserMapper.ToEntity(registerDto, hashedPassword);

        // Add user to database
        await _userRepository.Create(user);

        // Map User entity to response DTO
        var response = new UserResponseDto
        {
            Id = user.Id, // auto-assigned by DB
            Email = user.Email,
            Username = user.Username,
            Name = user.FirstName,
            Surname = user.LastName,
            PhoneNumber = user.Phone,
            DateOfBirth = user.Birthdate,
            Description = user.Bio,
            LastLoginAt = user.LastLoginAt
        };

        return Result<UserResponseDto>.Success(response);
    }
}
