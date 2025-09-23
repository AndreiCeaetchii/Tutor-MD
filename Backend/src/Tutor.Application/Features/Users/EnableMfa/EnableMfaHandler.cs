using Ardalis.Result;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Tutor.Application.Features.Users.Dtos;
using Tutor.Application.Interfaces;

namespace Tutor.Application.Features.Users.EnableMfa;

public class EnableMfaHandler : IRequestHandler<EnableMfaCommand,Result<EnableMFAResponse>>
{
    private readonly IAuthService _authService;

    public EnableMfaHandler(IAuthService authService)
    {
        _authService = authService;
    }
    public async Task<Result<EnableMFAResponse>> Handle(EnableMfaCommand request, CancellationToken cancellationToken)
    {
       return await _authService.EnableMFAAsync(request.userId);
    }
}