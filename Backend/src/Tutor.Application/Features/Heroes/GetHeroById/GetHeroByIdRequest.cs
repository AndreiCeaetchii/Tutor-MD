using Ardalis.Result;
using MediatR;
using Tutor.Domain.Entities.Common;

namespace Tutor.Application.Features.Heroes.GetHeroById;

public record GetHeroByIdRequest(HeroId Id) : IRequest<Result<GetHeroResponse>>;