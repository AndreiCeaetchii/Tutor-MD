using Ardalis.Result;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using System;
using System.Security.Claims;
using Tutor.Application.Features.Photos.Add_Photo;
using Tutor.Application.Features.Photos.Delete_Photo;
using Tutor.Application.Features.Photos.DTOs;
using Tutor.Application.Features.Users;
using Tutor.Application.Features.Users.Dtos;
using Tutor.Application.Features.Users.LoginOAuthUser;
using Tutor.Application.Features.Users.LoginUser;
using Tutor.Application.Features.Users.RegisterOAuthUser;

namespace Tutor.Api.Endpoints;

public static class UserEndpoints
{
    public static void MapUserEndpoints(this IEndpointRouteBuilder builder)
    {
        var group = builder.MapGroup("api/users")
            .WithTags("users");

        group.MapPost("/register", async (IMediator mediator, [FromBody] RegisterUserDto registerUserDto) =>
            {
                var command = new RegisterUserCommand(registerUserDto);
                var result = await mediator.Send(command);

                return result.IsSuccess
                    ? Results.Ok(result.Value)
                    : Results.BadRequest(result.Errors);
            })
            .Produces<UserResponseDto>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithName("RegisterUser");

        group.MapPost("/login", async (IMediator mediator, [FromBody] LoginUserDto loginUserDto) =>
            {
                var command = new LoginUserCommand(loginUserDto);
                var result = await mediator.Send(command);

                return result.IsSuccess
                    ? Results.Ok(result.Value)
                    : Results.BadRequest(result.Errors);
            }).Produces<UserResponseDto>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithName("LoginUser");

        group.MapPost("/register-auth",
                async (IMediator mediator, [FromBody] RegisterUserAuthDto registerUserAuthDto) =>
                {
                    var command = new RegisterUserWithOAuthCommand(registerUserAuthDto);
                    var result = await mediator.Send(command);

                    return result.IsSuccess
                        ? Results.Ok(result.Value)
                        : Results.BadRequest("Not success");
                }).Produces<UserResponseDto>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithName("RegisterAuthUser");

        group.MapPost("/login-auth",
                async (IMediator mediator, [FromBody] RegisterUserAuthDto registerUserAuthDto) =>
                {
                    var command = new LoginOAuthUserCommand(registerUserAuthDto);
                    var result = await mediator.Send(command);

                    return result.IsSuccess
                        ? Results.Ok(result.Value)
                        : Results.BadRequest(result.Errors);
                }).Produces<UserResponseDto>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithName("LoginAuthUser");
        
        group.MapPost("/add-photo",
                [Authorize] async (IMediator mediator, [FromForm] IFormFile file, HttpContext httpContext) =>
                {
                    var userIdClaim = httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                    if (string.IsNullOrEmpty(userIdClaim))
                        return Results.Unauthorized();

                    if (!int.TryParse(userIdClaim, out var userId))
                        return Results.BadRequest("Invalid UserId in token");
                    var command = new AddPhotoCommand(userId, file);
                    var result = await mediator.Send(command);
                    return result.IsSuccess
                        ? Results.Ok(result.Value)
                        : Results.BadRequest(result.Errors);
                })
            .WithName("AddPhoto")
            .Produces<PhotoDto>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status401Unauthorized)
            .DisableAntiforgery();
        group.MapDelete("/delete-photo",
                [Authorize] async (IMediator mediator, HttpContext httpContext) =>
                {
                    var userIdClaim = httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                    if (string.IsNullOrEmpty(userIdClaim))
                        return Results.Unauthorized();

                    if (!int.TryParse(userIdClaim, out var userId))
                        return Results.BadRequest("Invalid UserId in token");
                    var command = new DeletePhotoCommand(userId);
                    var result = await mediator.Send(command);
                    return result.IsSuccess
                        ? Results.Ok(result.Value)
                        : Results.BadRequest(result.Errors);
                })
            .WithName("DeletePhoto")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status401Unauthorized)
            .DisableAntiforgery();

        var healthGroup = builder.MapGroup("health")
            .WithTags("health");

        healthGroup.MapGet("/", () =>
            Results.Ok(new { status = "Healthy", timestamp = DateTime.UtcNow, service = "User API" }));
    }
}