using Ardalis.Result;
using MediatR;
using Tutor.Application.Features.Users.Dtos;

namespace Tutor.Application.Features.Users.VerifyMFA;

public record VerifyMfaCommand(int userId,VerifyMFACodeRequest verifyMfaCodeRequest ) : IRequest<Result<bool>>;
