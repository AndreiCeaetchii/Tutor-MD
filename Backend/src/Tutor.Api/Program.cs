using DotNetEnv;
using Hangfire;
using Hangfire.MemoryStorage;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Tutor.Api.Configurations;
using Tutor.Api.Endpoints;
using System;
using System.Threading;
using Tutor.Api.Common;
using Tutor.Api.Filters.Guards;
using Tutor.Application.Services.Background;
using Tutor.Infrastructure;
using Tutor.Infrastructure.Seeder;

var builder = WebApplication.CreateBuilder(args);

Env.Load();
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

// Controllers
builder.AddValidationSetup();

builder.Services.AddHttpClient();

// builder.Services.AddAntiforgery();
// Swagger
builder.Services.AddSwaggerSetup();

// Persistence
builder.Services.AddPersistenceSetup(builder.Configuration);
//Hangfire
builder.Services.AddHangfire(config =>
    config.UseMemoryStorage());
builder.Services.AddHangfireServer();

// Application layer setup
builder.Services.AddApplicationSetup(builder.Configuration);


builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("StudentPolicy", policy =>
        policy.RequireAuthenticatedUser().RequireRole("Student"));

    options.AddPolicy("TutorPolicy", policy =>
        policy.RequireAuthenticatedUser().RequireRole("Tutor"));

    options.AddPolicy("AdminPolicy", policy =>
        policy.RequireAuthenticatedUser().RequireRole("Admin"));
    options.AddPolicy("TutorOrStudentPolicy", policy =>
        policy.RequireRole("Tutor", "Student"));
    options.AddPolicy("AdminOrStudentPolicy", policy =>
        policy.RequireRole("Admin", "Student"));
    options.AddPolicy("AdminOrTutorOrStudentPolicy", policy =>
        policy.RequireRole("Tutor", "Student","Admin"));
    
    options.AddPolicy("ActiveUserOnly", policy =>
        policy.Requirements.Add(new ActiveUserRequirement()));
});
// Add identity stuff
builder.Services
    .AddIdentityApiEndpoints<ApplicationUser>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

// Request response compression
builder.Services.AddCompressionSetup();

// HttpContextAcessor
builder.Services.AddHttpContextAccessor();

// Mediator
builder.Services.AddMediatRSetup();

// Exception handler
builder.Services.AddExceptionHandler<ExceptionHandler>();

//Anti forgery
builder.Services.AddAntiforgery(options =>
{
    options.HeaderName = "X-CSRF-TOKEN"; 
});


builder.Logging.ClearProviders();

// Add serilog
if (builder.Environment.EnvironmentName != "Testing")
{
    builder.Host.UseLoggingSetup(builder.Configuration);

    // Add opentelemetry
    builder.AddOpenTemeletrySetup();
}


var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    await context.Database.MigrateAsync();
    await SubjectSeeder.SeedAsync(context);
    await RoleSeeder.SeedAsync(context);
    var scheduler = scope.ServiceProvider.GetRequiredService<JobSchedulerService>();
    await scheduler.StartAsync(CancellationToken.None);
}

// Configure the HTTP request pipeline.
app.UseResponseCompression();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseHsts();
app.UseResponseCompression();
app.UseHttpsRedirection();
app.UseCors("AllowFrontend");

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();
app.Use(async (context, next) =>
{
    var antiforgery = context.RequestServices.GetRequiredService<IAntiforgery>();

    if (HttpMethods.IsPost(context.Request.Method) ||
        HttpMethods.IsPut(context.Request.Method) ||
        HttpMethods.IsDelete(context.Request.Method))
    {
        try
        {
            await antiforgery.ValidateRequestAsync(context);
        }
        catch (AntiforgeryValidationException)
        {
            var problem = Results.Problem(
                title: "Forbidden",
                detail: "Invalid or missing CSRF token.",
                statusCode: StatusCodes.Status403Forbidden,
                instance: context.Request.Path
            );

            await problem.ExecuteAsync(context);
            return; // stop pipeline
        }
    }

    await next();
});
app.UseSwaggerSetup();
app.UseHangfireDashboard();
app.MapUserEndpoints();
app.MapTutorEndpoints();
app.MapStudentEndpoints();
app.MapAdminEndpoints();



await app.RunAsync();