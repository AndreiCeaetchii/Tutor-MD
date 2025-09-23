using Ardalis.Result;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Tutor.Application.Features.Users.Dtos;
using Tutor.Application.Interfaces;
using Tutor.Domain.Entities;
using Tutor.Domain.Interfaces;

namespace Tutor.Application.Features.Users.LoginUser;

public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, Result<UserResponseDto>>
{
    private readonly IAuthService _authService;

    public LoginUserCommandHandler(IAuthService authService)
    {
        _authService = authService;
    }

    public async Task<Result<UserResponseDto>> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        // Just delegate to the service
        if (request.LoginUserDto.MfaCode is not null)
            return await _authService.LoginAsync(request.LoginUserDto, request.LoginUserDto.MfaCode);
        return await _authService.LoginAsync(request.LoginUserDto);
    }
}