using Ardalis.Result;
using MediatR;
using System;
using Tutor.Application.Features.Users.Dtos;

namespace Tutor.Application.Features.Users.RegisterOAuthUser;

public record RegisterUserWithOAuthCommand(RegisterUserAuthDto registerUserAuthDto) : IRequest<Result<UserResponseDto>>;