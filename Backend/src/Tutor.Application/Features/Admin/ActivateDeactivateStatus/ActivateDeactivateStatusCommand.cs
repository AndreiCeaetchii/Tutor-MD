using Ardalis.Result;
using MediatR;
using Tutor.Application.Features.Admin.Dto.Activate;

namespace Tutor.Application.Features.Admin.ActivateDeactivateStatus;

public record ActivateDeactivateStatusCommand(ActivateDeactivateDto ActivateDeactivateDto) : IRequest<Result>;
