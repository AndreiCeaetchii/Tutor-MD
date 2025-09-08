using Ardalis.Result;
using MediatR;
using Tutor.Application.Features.Users.Dtos;

namespace Tutor.Application.Features.Users.LoginUser;

    public record LoginUserCommand(LoginUserDto LoginUserDto) : IRequest<Result<UserResponseDto>>;
