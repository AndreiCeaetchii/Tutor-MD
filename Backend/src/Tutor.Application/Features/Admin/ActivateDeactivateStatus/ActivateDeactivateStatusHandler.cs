using Ardalis.Result;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Tutor.Application.Interfaces;

namespace Tutor.Application.Features.Admin.ActivateDeactivateStatus;

public class ActivateDeactivateStatusHandler : IRequestHandler<ActivateDeactivateStatusCommand, Result>
{
    private readonly IUserService  _userService;

    public ActivateDeactivateStatusHandler(IUserService userService)
    {
        _userService = userService;
    }
    public async Task<Result> Handle(ActivateDeactivateStatusCommand request, CancellationToken cancellationToken)
    {
        if (request.ActivateDeactivateDto.IsActive)
            return await _userService.ActivateUserAsync(request.ActivateDeactivateDto.UserId);
        else
            return await _userService.DeactivateUserAsync(request.ActivateDeactivateDto.UserId);
    }
}