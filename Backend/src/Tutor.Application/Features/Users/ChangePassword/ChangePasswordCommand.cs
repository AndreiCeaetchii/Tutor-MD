using Ardalis.Result;
using MediatR;
using Tutor.Application.Features.Users.Dtos;

namespace Tutor.Application.Features.Users.ChangePassword;

public record ChangePasswordCommand(ChangePasswordDto ChangePasswordDto) : IRequest<Result>;
