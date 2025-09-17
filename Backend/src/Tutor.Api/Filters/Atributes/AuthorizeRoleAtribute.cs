using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Linq;
using System.Security.Claims;

namespace Tutor.Api.Filters.Atributes;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
public class AuthorizeRoleAttribute : Attribute, IAuthorizationFilter
{
    private readonly string[] _roles;

    public AuthorizeRoleAttribute(params string[] roles)
    {
        _roles = roles;
    }

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var user = context.HttpContext.User;
        Console.WriteLine($"User authenticated: {user?.Identity?.IsAuthenticated}");
        Console.WriteLine($"User name: {user?.Identity?.Name}");
        if (user?.Identity == null || !user.Identity.IsAuthenticated)
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        var userRoles = user.Claims
            .Where(c => c.Type == ClaimTypes.Role)
            .Select(c => c.Value)
            .ToList();
        Console.WriteLine(userRoles);
        var hasRole = _roles.Any(required => userRoles.Contains(required, StringComparer.OrdinalIgnoreCase));
        if (!hasRole)
        {
            context.Result = new ForbidResult();
        }
    }
}