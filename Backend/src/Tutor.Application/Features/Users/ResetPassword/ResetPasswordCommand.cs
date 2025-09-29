using Ardalis.Result;
using MediatR;
using Tutor.Application.Features.Users.Dtos;

namespace Tutor.Application.Features.Users.ResetPassword;

public record ResetPasswordCommand(ResetPasswordDto ResetPasswordDto) : IRequest<Result>;