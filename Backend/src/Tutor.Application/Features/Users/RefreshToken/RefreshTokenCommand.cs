using Ardalis.Result;
using MediatR;
using Tutor.Application.Features.Users.Dtos;

namespace Tutor.Application.Features.Users.RefreshToken;

public record RefreshTokenCommand(string Token, string RefreshToken) : IRequest<Result<TokenResponseDto>>;