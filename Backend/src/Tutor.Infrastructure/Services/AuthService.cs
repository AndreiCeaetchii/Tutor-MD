using Ardalis.Result;
using System.Threading.Tasks;
using Tutor.Application.Features.Users.Dtos;
using Tutor.Application.Interfaces;
using Tutor.Application.Mappers;
using Tutor.Domain.Entities;
using Tutor.Domain.Interfaces;

namespace Tutor.Infrastructure.Services;

public class AuthService : IAuthService
{
    private readonly IGenericRepository<User, int> _userRepository;
    private readonly IGenericRepository<Password, int> _passwordRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly ITokenService _tokenService;
    private readonly IGenericRepository2<UserRole>  _userRoleRepository;

    public AuthService(
        IGenericRepository<User, int> userRepository,
        IGenericRepository2<UserRole> userRoleRepository,
        IPasswordHasher passwordHasher,
        ITokenService tokenService,
        IGenericRepository<Password, int> passwordRepository)
    {
        _userRepository = userRepository;
        _userRoleRepository = userRoleRepository;
        _passwordHasher = passwordHasher;
        _tokenService = tokenService;
        _passwordRepository = passwordRepository;
    }

    public async Task<Result<UserResponseDto>> LoginAsync(LoginUserDto loginDto)
    {
        var user = await _userRepository.FindAsyncDefault(u => u.Email == loginDto.Email);
        if (user == null) return Result<UserResponseDto>.Error("Invalid Email");

        var password = await _passwordRepository.FindAsyncDefault(u => u.UserId == user.Id);
        if (password == null) return Result<UserResponseDto>.Error("No password found for this email ");

        if (!_passwordHasher.VerifyPassword(loginDto.Password, password.PasswordHash))
            return Result<UserResponseDto>.Error("Invalid Password");

        var token = _tokenService.GenerateToken(user);
        
        var usersRole =  await _userRoleRepository.FindAsyncDefault(u => u.UserId == user.Id);
        if (usersRole == null)
            return Result<UserResponseDto>.Error("No roles found for this email");
        var role = usersRole.RoleId;
        var response = user.ToResponseDto(token,role);

        return Result<UserResponseDto>.Success(response);
    }

    public async Task<Result<UserResponseDto>> RegisterAsync(RegisterUserDto registerDto)
    {
        // Map DTO to User entity
        var user = registerDto.ToEntity();

        // Persist user first (to get Id)
        await _userRepository.Create(user);

        // Hash password
        var hashedPassword = _passwordHasher.HashPassword(registerDto.Password);

        // Create Password entity
        var password = new Password { UserId = user.Id, PasswordHash = hashedPassword };

        await _passwordRepository.Create(password);

        // Generate token
        var token = _tokenService.GenerateToken(user);

        // Map entity to response DTO
        var response = user.ToResponseDto(token);

        return Result<UserResponseDto>.Success(response);
    }
}