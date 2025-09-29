using Ardalis.Result;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Tutor.Application.Interfaces;

namespace Tutor.Application.Features.Users.GetResetPassword;

public class GetResetPasswordHandler : IRequestHandler<GetResetPasswordCommand, Result>
{
    private readonly IUserService _userService;

    public GetResetPasswordHandler(IUserService userService)
    {
        _userService = userService;
    }
    public async Task<Result> Handle(GetResetPasswordCommand request, CancellationToken cancellationToken)
    {
        return await _userService.RequestPasswordResetAsync(request.GetResetTokenDto.Email);
    }
}