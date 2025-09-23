using Ardalis.Result;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Tutor.Application.Interfaces;

namespace Tutor.Application.Features.Users.DisableMFA;

public class DisableMFAHandler : IRequestHandler<DisableMFACommand, Result<bool>>
{
    private readonly IAuthService _authService;

    public DisableMFAHandler(IAuthService authService)
    {
        _authService = authService;
    }
    public async Task<Result<bool>> Handle(DisableMFACommand request, CancellationToken cancellationToken)
    {
        return await _authService.DisableMFAAsync(request.userId);
    }
}