using Ardalis.Result;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.Json;
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
    private readonly IMFAService _mfaService;

    public AuthService(
        IGenericRepository<User, int> userRepository,
        IGenericRepository2<UserRole> userRoleRepository,
        IPasswordHasher passwordHasher,
        ITokenService tokenService,
        IGenericRepository<Role, int> roleRepository,
        IMFAService mfaService,
        IGenericRepository<Password, int> passwordRepository)
    {
        _userRepository = userRepository;
        _roleRepository = roleRepository;
        _userRoleRepository = userRoleRepository;
        _passwordHasher = passwordHasher;
        _tokenService = tokenService;
        _passwordRepository = passwordRepository;
        _mfaService = mfaService;
    }

    public async Task<Result<UserResponseDto>> LoginAsync(LoginUserDto loginDto, string mfaCode = null)
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
        if (user.TwoFactorEnabled)
        {
            if (string.IsNullOrEmpty(mfaCode))
                return Result<UserResponseDto>.Error("MFA_REQUIRED");

            //  Decriptează cheia înainte
            var decryptedSecret = _mfaService.Decrypt(user.TwoFactorSecret);

            //  Folosește cheia decriptată
            if (!_mfaService.VerifyCode(decryptedSecret, mfaCode))
                return Result<UserResponseDto>.Error("Invalid Mfa code");
        }


        var token = _tokenService.GenerateToken(user, role);
        var refreshTokenInitial = _tokenService.GenerateRefreshToken();
        var refreshTokenExpiry = _tokenService.GetRefreshTokenExpiryTime();

        // Update user with hashed refresh token
        user.RefreshToken = _tokenService.HashRefreshToken(refreshTokenInitial);
        user.RefreshTokenExpiryTime = refreshTokenExpiry;
        await _userRepository.Update(user);

        var response = user.ToResponseDto(token);
        response.RefreshToken = refreshTokenInitial;


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
        var currentUserRole = new UserRole { UserId = user.Id, RoleId = registerDto.RoleId, AssignedAt = DateTime.Now };
        await _userRoleRepository.Create(currentUserRole);

        var role = await _roleRepository.FindAsyncDefault(u => u.Id == currentUserRole.RoleId);

        var token = _tokenService.GenerateToken(user, role);
        if (string.IsNullOrEmpty(token))
            return Result<UserResponseDto>.Error("Token generation failed");
        var refreshTokenInitial = _tokenService.GenerateRefreshToken();
        var refreshTokenExpiry = _tokenService.GetRefreshTokenExpiryTime();
      
        user.RefreshToken = _tokenService.HashRefreshToken(refreshTokenInitial);
        user.RefreshTokenExpiryTime = refreshTokenExpiry;
        await _userRepository.Update(user);

        var response = user.ToResponseDto(token);
        response.RefreshToken = refreshTokenInitial;


        return Result<UserResponseDto>.Success(response);
    }

    public async Task<Result<EnableMFAResponse>> EnableMFAAsync(int userId)
    {
        var user = await _userRepository.GetById(userId);
        var (secretKey, qrCodeImage) = _mfaService.GenerateSetup(user.Email);
        var recoveryCodes = _mfaService.GenerateRecoveryCodes();

        user.TwoFactorSecret = _mfaService.Encrypt(secretKey);
        List<string> hashedCodes = new List<string>();
        foreach (var recoveryCode in recoveryCodes)
            hashedCodes.Add(_mfaService.Hash(recoveryCode));
        user.TwoFactorRecoveryCodes = JsonSerializer.Serialize(hashedCodes);
        await _userRepository.Update(user);

        return Result.Success(new EnableMFAResponse
        {
            QrCodeImage = qrCodeImage, SecretKey = secretKey, RecoveryCodes = recoveryCodes
        });
    }

    public async Task<bool> VerifyMFAAsync(int userId, string code)
    {
        var user = await _userRepository.GetById(userId);
        var decryptedSecret = _mfaService.Decrypt(user.TwoFactorSecret);

        var result = _mfaService.VerifyCode(decryptedSecret, code);
        if (result)
        {
            user.TwoFactorEnabled = true;
            await _userRepository.Update(user);
        }

        return result;
    }

    public async Task<bool> DisableMFAAsync(int userId)
    {
        var user = await _userRepository.GetById(userId);
        user.TwoFactorEnabled = false;
        user.TwoFactorSecret = null;
        user.TwoFactorRecoveryCodes = null;

        await _userRepository.Update(user);
        return true;
    }
    public async Task<Result<TokenResponseDto>> RefreshTokenAsync(string accessToken, string refreshToken)
{
    if (string.IsNullOrEmpty(accessToken) || string.IsNullOrEmpty(refreshToken))
        return Result<TokenResponseDto>.Error("Invalid tokens");

    try
    { var principal = _tokenService.GetPrincipalFromExpiredToken(accessToken);
        var userIdClaim = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        
        if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
            return Result<TokenResponseDto>.Error("Invalid token");

        var user = await _userRepository.FindAsyncDefault(u => u.Id == userId);
        if (user == null)
            return Result<TokenResponseDto>.Error("User not found");
        if (user.RefreshTokenExpiryTime < DateTime.UtcNow)
            return Result<TokenResponseDto>.Error("Refresh token expired");

        if (!_tokenService.VerifyRefreshToken(refreshToken, user.RefreshToken))
            return Result<TokenResponseDto>.Error("Invalid refresh token");

        var userRole = await _userRoleRepository.FindAsyncDefault(u => u.UserId == user.Id);
        if (userRole == null)
            return Result<TokenResponseDto>.Error("User role not found");

        var role = await _roleRepository.FindAsyncDefault(r => r.Id == userRole.RoleId);
        if (role == null)
            return Result<TokenResponseDto>.Error("Role not found");

        var newAccessToken = _tokenService.GenerateToken(user, role);
        var newRefreshToken = _tokenService.GenerateRefreshToken();
        var newRefreshTokenExpiry = _tokenService.GetRefreshTokenExpiryTime();
        user.RefreshToken = _tokenService.HashRefreshToken(newRefreshToken);
        user.RefreshTokenExpiryTime = newRefreshTokenExpiry;
        await _userRepository.Update(user);

        var response = new TokenResponseDto
        {
            Token = newAccessToken,
            RefreshToken = newRefreshToken,
            RefreshTokenExpiryTime = newRefreshTokenExpiry
        };

        return Result<TokenResponseDto>.Success(response);
    }
    catch (SecurityTokenException)
    {
        return Result<TokenResponseDto>.Error("Invalid access token");
    }
    catch (Exception ex)
    {
        return Result<TokenResponseDto>.Error($"Token refresh failed: {ex.Message}");
    }
}
}