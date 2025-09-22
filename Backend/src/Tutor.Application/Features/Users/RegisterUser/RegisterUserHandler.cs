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
    private readonly IAuthService _authService;

    public RegisterUserCommandHandler(IAuthService authService)
    {
        _authService = authService;
    }

    public async Task<Result<UserResponseDto>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        if (request.RegisterUserDto.RoleId == 1)
            return Result<UserResponseDto>.Error("You cannot create Admin account");
        return await _authService.RegisterAsync(request.RegisterUserDto);
    }
}