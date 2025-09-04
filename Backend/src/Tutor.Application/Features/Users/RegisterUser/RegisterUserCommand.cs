using Ardalis.Result;
using MediatR;
using Tutor.Application.Features.Users.Dtos;

namespace Tutor.Application.Features.Users;

public record RegisterUserCommand(RegisterUserDto RegisterUserDto) : IRequest<Result<UserResponseDto>>;