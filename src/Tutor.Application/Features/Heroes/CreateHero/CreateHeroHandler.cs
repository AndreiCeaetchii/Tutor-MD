using Ardalis.Result;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Tutor.Application.Common;

namespace Tutor.Application.Features.Heroes.CreateHero;

public class CreateHeroHandler : IRequestHandler<CreateHeroRequest, Result<GetHeroResponse>>
{
    private readonly IContext _context;
    
    
    public CreateHeroHandler(IContext context)
    {
        _context = context;
    }

    public async Task<Result<GetHeroResponse>> Handle(CreateHeroRequest request, CancellationToken cancellationToken)
    {
        var created = Tutor.Application.Features.Heroes.Mapper.ToHeroEntity(request);
        _context.Heroes.Add(created);
        await _context.SaveChangesAsync(cancellationToken);
        return Tutor.Application.Features.Heroes.Mapper.ToHeroDto(created);
    }
}