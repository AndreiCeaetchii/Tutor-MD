using Ardalis.Result;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Tutor.Application.Interfaces;

namespace Tutor.Application.Features.Users.VerifyMFA;

public class VerifyMfaHandler :  IRequestHandler<VerifyMfaCommand, Result<bool>>
{
    private readonly IAuthService _authService;

    public VerifyMfaHandler(IAuthService authService)
    {
        _authService = authService;
    }
    public async Task<Result<bool>> Handle(VerifyMfaCommand request, CancellationToken cancellationToken)
    {
        if(await _authService.VerifyMFAAsync(request.userId,request.verifyMfaCodeRequest.Code))
            return Result<bool>.Success(true);
        return Result<bool>.Error("invalid MFA code");
    }
}