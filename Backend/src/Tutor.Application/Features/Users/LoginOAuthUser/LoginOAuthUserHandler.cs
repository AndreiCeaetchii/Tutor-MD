using Ardalis.Result;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using Tutor.Application.Features.Users.Dtos;
using Tutor.Application.Interfaces;
using Tutor.Domain.Entities;

namespace Tutor.Application.Features.Users.LoginOAuthUser;

public class LoginOAuthUserCommandHandler 
    : IRequestHandler<LoginOAuthUserCommand, Result<UserResponseDto>>
{
    private readonly IUserService _userService;
    private readonly ITokenService _jwtTokenService;
    private readonly IOAuthService _oauthService;

    public LoginOAuthUserCommandHandler(
        IUserService userService,
        ITokenService jwtTokenService,
        IOAuthService oauthService)
    {
        _userService = userService;
        _jwtTokenService = jwtTokenService;
        _oauthService = oauthService;
    }

    public async Task<Result<UserResponseDto>> Handle(LoginOAuthUserCommand requestDto, CancellationToken cancellationToken)
    {
        var request = requestDto.loginUserAuthDto;
        
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

        // Try to find user by OAuth provider ID first (most direct way)
        var user = await _userService.GetUserByOAuthIdAsync(request.Provider, userInfo.ProviderId);
        
        if (user != null)
        {
            // User found with OAuth ID - generate token and login
            var token = _jwtTokenService.GenerateToken(user);
            return Result.Success(CreateUserResponse(user, token));
        }
        
        return Result<UserResponseDto>.Error("Invalid OAuth user");
    }

    private UserResponseDto CreateUserResponse(User user, string token)
    {
        return new UserResponseDto
        {
            Token = token,
            Id = user.Id,
            Email = user.Email,
            Username = user.Username
        };
    }
}