using Ardalis.Result;
using MediatR;
using Tutor.Application.Interfaces;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Tutor.Application.Features.Users.ChangePassword;

public class ChangePasswordHandler : IRequestHandler<ChangePasswordCommand, Result>
{
    private readonly IUserService _userService;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ChangePasswordHandler(IUserService userService, IHttpContextAccessor httpContextAccessor)
    {
        _userService = userService;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<Result> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
    {
        var userId = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        
        if (string.IsNullOrEmpty(userId))
            return Result.Error("User not authenticated");

        return await _userService.ChangePasswordAsync(
            int.Parse(userId), 
            request.ChangePasswordDto.CurrentPassword, 
            request.ChangePasswordDto.NewPassword
        );
    }
}
