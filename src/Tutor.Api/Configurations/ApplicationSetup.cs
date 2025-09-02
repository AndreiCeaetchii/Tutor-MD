using Microsoft.Extensions.DependencyInjection;
using Tutor.Application.Common;
using Tutor.Infrastructure;

namespace Tutor.Api.Configurations;

public static class ApplicationSetup
{
    public static IServiceCollection AddApplicationSetup(this IServiceCollection services)
    {
        services.AddScoped<IContext, ApplicationDbContext>();
        return services;
    }
    
    
}