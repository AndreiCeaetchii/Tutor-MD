using Ardalis.Result;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using Tutor.Application.Features.Users.Dtos;
using Tutor.Application.Interfaces;
using Tutor.Application.Mappers;
using Tutor.Domain.Entities;
using Tutor.Domain.Interfaces;

namespace Tutor.Application.Features.Users.LoginOAuthUser;

public class LoginOAuthUserCommandHandler
    : IRequestHandler<LoginOAuthUserCommand, Result<UserResponseDto>>
{
    private readonly IUserService _userService;
    private readonly ITokenService _jwtTokenService;
    private readonly IOAuthService _oauthService;
    private readonly IUserRoleService _roleService;
    private const string Google = "google";
    private readonly IGenericRepository<Role, int> _roleRepository;
    private readonly IMFAService _mfaService;

    public LoginOAuthUserCommandHandler(
        IUserRoleService roleService,
        IUserService userService,
        ITokenService jwtTokenService,
        IGenericRepository<Role, int> roleRepository,
        IMFAService mfaService,
        IOAuthService oauthService)
    {
        _userService = userService;
        _roleService = roleService;
        _jwtTokenService = jwtTokenService;
        _roleRepository = roleRepository;
        _mfaService = mfaService;
        _oauthService = oauthService;
    }

    public async Task<Result<UserResponseDto>> Handle(LoginOAuthUserCommand requestDto,
        CancellationToken cancellationToken)
    {
        var request = requestDto.loginUserAuthDto;

        // Validate OAuth token
        OAuthUserInfo? userInfo;
        try
        {
            userInfo = request.Provider.ToLower() switch
            {
                Google => await _oauthService.ValidateGoogleTokenAsync(request.AccessToken),
                _ => null
            };

            if (userInfo == null)
                return Result<UserResponseDto>.Error($"Unsupported OAuth provider: {request.Provider}");
        }
        catch (Exception ex)
        {
            return Result<UserResponseDto>.Error($"Invalid OAuth token: {ex.Message}");
        }

        if (userInfo.Email != request.Email)
            return Result<UserResponseDto>.Error("Email does not match OAuth account");

        var user = await _userService.GetUserByOAuthIdAsync(request.Provider, userInfo.ProviderId);


        if (user != null)
        {
            var usersRole = await _roleService.GetRoleIdAsync(user.Id);
            if (usersRole == null)
                return Result<UserResponseDto>.Error($"No role found for user ");
            var role = await _roleRepository.FindAsyncDefault(u => u.Id == usersRole);

            if (user.TwoFactorEnabled)
            {
                if (string.IsNullOrEmpty(request.MfaCode))
                    return Result<UserResponseDto>.Error("MFA_REQUIRED");

                var decryptedSecret = _mfaService.Decrypt(user.TwoFactorSecret);
                if (!_mfaService.VerifyCode(decryptedSecret, request.MfaCode))
                    return Result<UserResponseDto>.Error("Invalid Mfa code");
            }

            var token = _jwtTokenService.GenerateToken(user, role);

            return Result.Success(user.ToResponseDto(token));
        }

        return Result<UserResponseDto>.Error("Invalid OAuth user");
    }
}