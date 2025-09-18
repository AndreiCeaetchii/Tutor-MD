using Hangfire;
using Hangfire.MemoryStorage;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;
using Tutor.Application.Common;
using Tutor.Application.Interfaces;
using Tutor.Application.Mappers;
using Tutor.Application.Mappers.TutorMapper;
using Tutor.Application.Services;
using Tutor.Application.Services.Background;
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
        services.AddScoped<IStudentService, StudentService>();
        services.AddScoped<IAvailabilityService, AvailabilityService>();
        services.AddScoped<IBookingService, BookingService>();
        services.AddScoped<IBookingNotificationService ,BookingNotificationService>();
        services.AddScoped<INotificationService, NotificationService>();

        services.Configure<CloudinarySettings>(configuration.GetSection("CloudinarySettings"));
        services.AddTransient<JobSchedulerService>();



        services.AddAutoMapper(typeof(TutorMappingProfile));
        services.AddAutoMapper(typeof(StudentMappingProfile));
        services.AddAutoMapper(typeof(AvailabilityMappingProfile));
        services.AddAutoMapper(typeof(BookingMappingProfile));
        services.AddAutoMapper(typeof(NotificationMappingProfile));


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
            });
        var googleClientId = configuration["Google:ClientId"];
        var googleClientSecret = configuration["Google:ClientSecret"];

        if (!string.IsNullOrEmpty(googleClientId) && !string.IsNullOrEmpty(googleClientSecret))
        {
            services.AddAuthentication()
                .AddGoogle(options =>
                {
                    options.ClientId = googleClientId;
                    options.ClientSecret = googleClientSecret;
                    options.UserInformationEndpoint = configuration["Google:UserInfoEndpoint"];
                });
        }
        else
        {
            Console.WriteLine("Google authentication not configured. ClientId or ClientSecret missing.");
        }

        services.AddCors(options =>
        {
            options.AddPolicy("AllowFrontend",
                policy =>
                {
                    policy.WithOrigins("http://localhost:5173", "https://localhost:5173")
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials();
                });
        });

        return services;
    }
}