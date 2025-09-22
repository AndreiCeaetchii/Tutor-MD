using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;
using Tutor.Application.Common;
using Tutor.Application.Interfaces;
using Tutor.Application.Mappers.TutorMapper;
using Tutor.Application.Services;
using Tutor.Domain.Interfaces;
using Tutor.Infrastructure;
using Tutor.Infrastructure.Helpers;
using Tutor.Infrastructure.Repositories;
using Tutor.Infrastructure.Services;

namespace Tutor.Api.Configurations;

public static class ApplicationSetup
{
    public static IServiceCollection AddApplicationSetup(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IContext, ApplicationDbContext>();

        services.AddHttpClient();

        services.AddScoped(typeof(IGenericRepository<,>), typeof(GenericRepository<,>));
        services.AddScoped(typeof(IGenericRepository2<>), typeof(GenericRepository2<>));
        services.AddScoped<IPasswordHasher, PasswordHasher>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IOAuthService, OAuthService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<ITutorService, TutorService>();
        services.AddScoped<ISubjectService, SubjectService>();
        services.AddScoped<IUserRoleService, UserRoleService>();
        services.AddScoped<ITutorSubjectService, TutorSubjectService>();
        services.AddScoped<IPhotoService, PhotoService>();
        
        services.Configure<CloudinarySettings>(configuration.GetSection("CloudinarySettings"));



        services.AddAutoMapper(typeof(TutorMappingProfile));

        services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = configuration["Jwt:Issuer"],
                    ValidAudience = configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(configuration["Jwt:Secret"]))
                };
            })
            .AddGoogle(options =>
            {
                options.ClientId = Environment.GetEnvironmentVariable("GOOGLE_CLIENT_ID")
                                   ?? configuration["Google:ClientId"];
                options.ClientSecret = Environment.GetEnvironmentVariable("GOOGLE_CLIENT_SECRET")
                                       ?? configuration["Google:ClientSecret"];
                options.UserInformationEndpoint = configuration["Google:UserInfoEndpoint"];
            });

        var allowedOriginsEnv = Environment.GetEnvironmentVariable("ALLOWED_ORIGINS");
        var corsPolicyNameEnv = Environment.GetEnvironmentVariable("CORS_POLICY_NAME");

        var corsPolicyName = string.IsNullOrWhiteSpace(corsPolicyNameEnv)
            ? "AllowFrontend"
            : corsPolicyNameEnv;

        var allowedOrigins = string.IsNullOrWhiteSpace(allowedOriginsEnv)
            ? Array.Empty<string>()
            : allowedOriginsEnv
                .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

        services.AddCors(options =>
        {
            options.AddPolicy(corsPolicyName, policy =>
            {
                if (allowedOrigins.Length > 0)
                {
                    policy.WithOrigins(allowedOrigins)
                          .AllowAnyHeader()
                          .AllowAnyMethod()
                          .AllowCredentials();
                }
                else
                {
                    policy.AllowAnyOrigin()
                          .AllowAnyHeader()
                          .AllowAnyMethod();
                }
            });
        });

        return services;
    }
}