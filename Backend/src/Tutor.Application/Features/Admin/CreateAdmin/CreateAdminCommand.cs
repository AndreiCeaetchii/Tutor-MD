using Ardalis.Result;
using MediatR;

namespace Tutor.Application.Features.Admin.CreateAdmin;

public record CreateAdminCommand(int UserId): IRequest<Result> ;