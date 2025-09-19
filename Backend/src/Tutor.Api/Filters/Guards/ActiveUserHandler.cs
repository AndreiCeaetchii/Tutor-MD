using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using Tutor.Domain.Entities;
using Tutor.Domain.Interfaces;

namespace Tutor.Api.Filters.Guards;

public class ActiveUserHandler : AuthorizationHandler<ActiveUserRequirement>
{
    private readonly IGenericRepository<User,int> _userRepository;

    public ActiveUserHandler(IGenericRepository<User,int> userRepository)
    {
        _userRepository = userRepository;
    }

    protected override async Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        ActiveUserRequirement requirement)
    {
        var idClaim = context.User.FindFirst(ClaimTypes.NameIdentifier)
                      ?? context.User.FindFirst("sub");

        if (idClaim == null || !int.TryParse(idClaim.Value, out var userId))
            return;

        var user = await _userRepository.GetById(userId);

        if (user?.IsActive == true)
            context.Succeed(requirement);
    }
}
