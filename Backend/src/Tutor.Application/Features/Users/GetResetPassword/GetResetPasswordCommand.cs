using Ardalis.Result;
using MediatR;
using Tutor.Application.Features.Users.Dtos;

namespace Tutor.Application.Features.Users.GetResetPassword;

public record GetResetPasswordCommand(GetResetTokenDto GetResetTokenDto) : IRequest<Result>;