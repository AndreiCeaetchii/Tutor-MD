using Ardalis.Result;
using MediatR;
using Tutor.Application.Features.Users.Dtos;

namespace Tutor.Application.Features.Users.EnableMfa;

public record EnableMfaCommand(int userId) : IRequest<Result<EnableMFAResponse>>;