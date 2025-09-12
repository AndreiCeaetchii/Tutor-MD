using Ardalis.Result;
using DotNetEnv;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;
using Tutor.Api.Common;
using Tutor.Api.Configurations;
using Tutor.Api.Endpoints;
using Tutor.Application.Interfaces;
using Tutor.Application.Services;
using Tutor.Domain.Interfaces;
using Tutor.Infrastructure;
using Tutor.Infrastructure.Repositories;
using Tutor.Infrastructure.Seeder;
using Tutor.Infrastructure.Services;

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
    await SubjectSeeder.SeedAsync(context);
    await RoleSeeder.SeedAsync(context);
}


// Configure the HTTP request pipeline.
app.UseResponseCompression();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.MapUserEndpoints();
app.MapTutorEndpoints();
app.MapStudentEndpoints();

app.UseRouting();
// app.UseAntiforgery(); 
app.UseSwaggerSetup();
app.UseHsts();

app.UseResponseCompression();
app.UseHttpsRedirection();
app.UseCors("AllowFrontend");
app.UseAuthentication();
app.UseAuthorization();


await app.RunAsync();