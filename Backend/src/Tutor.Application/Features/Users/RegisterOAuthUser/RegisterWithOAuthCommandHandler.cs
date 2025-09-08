using Ardalis.Result;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using Tutor.Application.Features.Users.Dtos;
using Tutor.Application.Interfaces;
using Tutor.Application.Mappers;
using Tutor.Domain.Entities;

namespace Tutor.Application.Features.Users.RegisterOAuthUser;

public class RegisterUserWithOAuthCommandHandler 
    : IRequestHandler<RegisterUserWithOAuthCommand, Result<UserResponseDto>>
{
    private readonly IUserService _userService;
    private readonly ITokenService _jwtTokenService;
    private readonly IOAuthService _oauthService;

    public RegisterUserWithOAuthCommandHandler(
        IUserService userService,
        ITokenService jwtTokenService,
        IOAuthService oauthService)
    {
        _userService = userService;
        _jwtTokenService = jwtTokenService;
        _oauthService = oauthService;
    }

    public async Task<Result<UserResponseDto>> Handle(RegisterUserWithOAuthCommand requestDto, CancellationToken cancellationToken)
    {
        var request=requestDto.registerUserAuthDto;
        // Validate OAuth token
        OAuthUserInfo? userInfo;
        try
        {
            userInfo = request.Provider.ToLower() switch
            {
                "google" => await _oauthService.ValidateGoogleTokenAsync(request.AccessToken),
                _ => null
            };

            if (userInfo == null)
                return Result<UserResponseDto>.Error($"Unsupported OAuth provider: {request.Provider}");
        }
        catch (Exception ex)
        {
            return Result<UserResponseDto>.Error($"Invalid OAuth token: {ex.Message}");
        }

        // Verify email matches
        if (userInfo.Email != request.Email)
            return Result<UserResponseDto>.Error("Email does not match OAuth account");

        // Check if user already exists by provider
        var existingUser = await _userService.GetUserByOAuthIdAsync(request.Provider, userInfo.ProviderId);
        if (existingUser != null)
            return Result<UserResponseDto>.Error("User already registered with this OAuth provider");

        // Check if email already exists
        var existingUserByEmail = await _userService.GetUserByEmailAsync(request.Email);
        if (existingUserByEmail != null)
            return Result<UserResponseDto>.Error("Email already registered");

        // Create new user with OAuth data
        var user = await _userService.CreateUserFromOAuthAsync(
            provider: request.Provider,
            providerId: userInfo.ProviderId,
            email: request.Email
        );

        // Generate JWT token
        var token = _jwtTokenService.GenerateToken(user);

        return Result.Success( UserMapper.ToResponseDto(user, token));
    }
}
