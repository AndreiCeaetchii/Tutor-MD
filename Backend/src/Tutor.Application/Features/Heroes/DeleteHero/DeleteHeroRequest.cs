using Ardalis.Result;
using MediatR;
using Tutor.Domain.Entities.Common;

namespace Tutor.Application.Features.Heroes.DeleteHero;

public record DeleteHeroRequest(HeroId Id) : IRequest<Result>;