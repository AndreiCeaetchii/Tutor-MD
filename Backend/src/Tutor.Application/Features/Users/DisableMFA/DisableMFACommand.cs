using Ardalis.Result;
using MediatR;

namespace Tutor.Application.Features.Users.DisableMFA;

public record DisableMFACommand(int userId) : IRequest<Result<bool>>;