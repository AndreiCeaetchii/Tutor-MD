using Ardalis.Result;
using MediatR;
using Tutor.Application.Features.Users.Dtos;

namespace Tutor.Application.Features.Users.LoginOAuthUser;

public record LoginOAuthUserCommand(LoginUserAuthDto loginUserAuthDto) : IRequest<Result<UserResponseDto>>;