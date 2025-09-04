using Microsoft.Extensions.DependencyInjection;
using Tutor.Application.Common.Behaviors;
using Tutor.Application.Features.Users.RegisterUser;

namespace Tutor.Api.Configurations;

public static class MediatRSetup
{
    public static IServiceCollection AddMediatRSetup(this IServiceCollection services)
    {
        services.AddMediatR((config) =>
        {
            config.RegisterServicesFromAssemblyContaining(typeof(Tutor.Application.IAssemblyMarker));
            config.AddOpenBehavior(typeof(ValidationResultPipelineBehavior<,>));
            config.RegisterServicesFromAssembly(typeof(RegisterUserCommandHandler).Assembly);
        });
        


        return services;
    }
}