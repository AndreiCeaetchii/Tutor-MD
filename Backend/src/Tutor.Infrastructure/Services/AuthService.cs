using Ardalis.Result;
using System;
using System.Threading.Tasks;
using Tutor.Application.Features.Users.Dtos;
using Tutor.Application.Interfaces;
using Tutor.Application.Mappers;
using Tutor.Domain.Entities;
using Tutor.Domain.Entities.Common;
using Tutor.Domain.Interfaces;

namespace Tutor.Infrastructure.Services;

public class AuthService : IAuthService
{
    private readonly IGenericRepository<User, int> _userRepository;
    private readonly IGenericRepository<Password, int> _passwordRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly ITokenService _tokenService;
    private readonly IGenericRepository2<UserRole> _userRoleRepository;
    private readonly IGenericRepository<Role, int> _roleRepository;

    public AuthService(
        IGenericRepository<User, int> userRepository,
        IGenericRepository2<UserRole> userRoleRepository,
        IPasswordHasher passwordHasher,
        ITokenService tokenService,
        IGenericRepository<Role,int> roleRepository,
        IGenericRepository<Password, int> passwordRepository)
    {
        _userRepository = userRepository;
        _roleRepository = roleRepository;
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

    

        var usersRole = await _userRoleRepository.FindAsyncDefault(u => u.UserId == user.Id);
        if (usersRole == null)
            return Result<UserResponseDto>.Error("No roles found for this email");
        
        var role = await _roleRepository.FindAsyncDefault(u => u.Id == usersRole.RoleId);
        
        var token = _tokenService.GenerateToken(user, role);
        var response = user.ToResponseDto(token);

        return Result<UserResponseDto>.Success(response);
    }

    public async Task<Result<UserResponseDto>> RegisterAsync(RegisterUserDto registerDto)
    {
        if (registerDto == null)
            return Result<UserResponseDto>.Error("Invalid request");
        if (string.IsNullOrWhiteSpace(registerDto.Email))
            return Result<UserResponseDto>.Error("Email is required");
        if (string.IsNullOrWhiteSpace(registerDto.Password))
            return Result<UserResponseDto>.Error("Password is required");
        var existingUser = await _userRepository.FindAsyncDefault(u => u.Email == registerDto.Email);
        if (existingUser != null)
            return Result<UserResponseDto>.Error("User with this email already exists");

        var user = registerDto.ToEntity();
        await _userRepository.Create(user);

        var hashedPassword = _passwordHasher.HashPassword(registerDto.Password);
        if (string.IsNullOrEmpty(hashedPassword))
            return Result<UserResponseDto>.Error("Password hashing failed");

        var password = new Password { UserId = user.Id, PasswordHash = hashedPassword };
        await _passwordRepository.Create(password);

        var usersRole = await _userRoleRepository.FindAsyncDefault(u => u.UserId == user.Id);
        if (usersRole != null)
            return Result<UserResponseDto>.Error("User already has an role");
        var currentUserRole = new UserRole
        {
            UserId = user.Id,
            RoleId = registerDto.RoleId,
            AssignedAt = DateTime.Now
        };
        await _userRoleRepository.Create(currentUserRole);
        
        var role = await _roleRepository.FindAsyncDefault(u => u.Id == currentUserRole.RoleId);
        
        var token = _tokenService.GenerateToken(user, role);
        if (string.IsNullOrEmpty(token))
            return Result<UserResponseDto>.Error("Token generation failed");

        var response = user.ToResponseDto(token);

        return Result<UserResponseDto>.Success(response);
    }
}