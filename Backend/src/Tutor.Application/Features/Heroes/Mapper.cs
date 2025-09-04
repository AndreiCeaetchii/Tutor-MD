using Riok.Mapperly.Abstractions;
using System.Linq;
using Tutor.Application.Features.Heroes.CreateHero;
using Tutor.Domain.Entities;

namespace Tutor.Application.Features.Heroes;

[Mapper]
public static partial class Mapper
{
    public static partial Hero ToHeroEntity(CreateHeroRequest dto);
    public static partial GetHeroResponse ToHeroDto(Hero entity);
    
    public static partial IQueryable<GetHeroResponse> ProjectToResponse(this IQueryable<Hero> source);

}