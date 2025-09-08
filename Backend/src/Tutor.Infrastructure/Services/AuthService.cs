using Ardalis.Result;
using System.Threading.Tasks;
using Tutor.Application.Features.Users.Dtos;
using Tutor.Application.Interfaces;
using Tutor.Domain.Entities;
using Tutor.Domain.Interfaces;

namespace Tutor.Infrastructure.Services;

public class AuthService : IAuthService
{
    private readonly IGenericRepository<User, int> _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly ITokenService _tokenService;

    public AuthService(
        IGenericRepository<User, int> userRepository,
        IPasswordHasher passwordHasher,
        ITokenService tokenService)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _tokenService = tokenService;
    }

    public async Task<Result<UserResponseDto>> LoginAsync(LoginUserDto loginDto)
    {
        var user = await _userRepository.FindAsyncDefault(u => u.Email == loginDto.Email);
        if (user == null) return Result<UserResponseDto>.Error("Invalid Email");

        if (!_passwordHasher.VerifyPassword(loginDto.Password, user.PasswordHash))
            return Result<UserResponseDto>.Error("Invalid Password");

        var token = _tokenService.GenerateToken(user);

        var response = new UserResponseDto
        {
            Id = user.Id,
            Email = user.Email,
            Username = user.Username,
            Token = token
        };

        return Result<UserResponseDto>.Success(response);
    }

    public async Task<Result<UserResponseDto>> RegisterAsync(RegisterUserDto registerDto)
    {
        // Hash password
        var hashedPassword = _passwordHasher.HashPassword(registerDto.Password);

        // Map DTO to entity
        var user = UserMapper.ToEntity(registerDto, hashedPassword);

        // Persist user
        await _userRepository.Create(user);

        // Generate token
        var token = _tokenService.GenerateToken(user);

        // Map entity to response DTO
        var response = new UserResponseDto
        {
            Id = user.Id,
            Email = user.Email,
            Token = token
        };

        return Result<UserResponseDto>.Success(response);
    }
}