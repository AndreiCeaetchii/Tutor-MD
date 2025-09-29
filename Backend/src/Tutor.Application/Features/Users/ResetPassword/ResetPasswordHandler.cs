using Ardalis.Result;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Tutor.Application.Features.Users.Dtos;
using Tutor.Application.Interfaces;

namespace Tutor.Application.Features.Users.ResetPassword;

public class ResetPasswordHandler :  IRequestHandler<ResetPasswordCommand, Result>
{
    private readonly IUserService _userService;

    public ResetPasswordHandler(IUserService userService)
    {
        _userService = userService;
    }
    public async Task<Result> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
    {
        return await _userService.ResetPasswordAsync(request.ResetPasswordDto.Token, request.ResetPasswordDto.NewPassword);
    }
}