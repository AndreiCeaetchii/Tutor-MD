using MediatR;
using Tutor.Application.Common.Responses;
using Tutor.Domain.Entities.Enums;

namespace Tutor.Application.Features.Heroes.GetAllHeroes;

public record GetAllHeroesRequest
    (string? Name = null, string? Nickname = null, int? Age = null, string? Individuality = null, HeroType? HeroType = null, string? Team = null, int CurrentPage = 1, int PageSize = 15) : IRequest<PaginatedList<GetHeroResponse>>;