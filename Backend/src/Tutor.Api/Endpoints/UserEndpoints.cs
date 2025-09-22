using Ardalis.Result;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using System;
using System.Security.Claims;
using Tutor.Application.Features.Admin.ActivateDeactivateStatus;
using Tutor.Application.Features.Admin.CreateAdmin;
using Tutor.Application.Features.Admin.Dto.Activate;
using Tutor.Application.Features.Notifications.Dto;
using Tutor.Application.Features.Notifications.GetNotificationCount;
using Tutor.Application.Features.Notifications.GetNotifications;
using Tutor.Application.Features.Notifications.ReadNotification;
using Tutor.Application.Features.Photos.Add_Photo;
using Tutor.Application.Features.Photos.Delete_Photo;
using Tutor.Application.Features.Photos.DTOs;
using Tutor.Application.Features.Users;
using Tutor.Application.Features.Users.Dtos;
using Tutor.Application.Features.Users.LoginOAuthUser;
using Tutor.Application.Features.Users.LoginUser;
using Tutor.Application.Features.Users.RegisterOAuthUser;
using Tutor.Application.Features.Users.RegisterUser;

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
                        : Results.BadRequest(result.Errors);
                }).Produces<UserResponseDto>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithName("RegisterAuthUser");

        group.MapPost("/login-auth",
                async (IMediator mediator, [FromBody] LoginUserAuthDto loginUserAuthDto) =>
                {
                    var command = new LoginOAuthUserCommand(loginUserAuthDto);
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
            .RequireAuthorization("TutorOrStudentPolicy") 
            .RequireAuthorization("ActiveUserOnly")
            .Produces(StatusCodes.Status401Unauthorized)
            .DisableAntiforgery();
        group.MapDelete("/delete-photo",
                async (IMediator mediator, HttpContext httpContext) =>
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
            .RequireAuthorization("TutorOrStudentPolicy")
            .RequireAuthorization("ActiveUserOnly")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status401Unauthorized);
        
        group.MapGet("/notification/count",
            async (IMediator mediator, HttpContext httpContext) =>
            {
                var userIdClaim = httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userIdClaim))
                    return Results.Unauthorized();

                if (!int.TryParse(userIdClaim, out var userId))
                    return Results.BadRequest("Invalid UserId in token");
                var command = new GetNotificationCountCommand(userId);
                var result = await mediator.Send(command);
                return result.IsSuccess
                    ? Results.Ok(result.Value)
                    : Results.BadRequest(result.Errors);
            })
            .WithName("GetNotificationCount")
            .RequireAuthorization("TutorOrStudentPolicy")
            .Produces<NotificationCountDto>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status401Unauthorized);
        
        group.MapGet("/notifications",
                async (IMediator mediator, HttpContext httpContext) =>
                {
                    var userIdClaim = httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                    if (string.IsNullOrEmpty(userIdClaim))
                        return Results.Unauthorized();

                    if (!int.TryParse(userIdClaim, out var userId))
                        return Results.BadRequest("Invalid UserId in token");
                    var command = new GetNotificationsCommand(userId);
                    var result = await mediator.Send(command);
                    return result.IsSuccess
                        ? Results.Ok(result.Value)
                        : Results.BadRequest(result.Errors);
                })
            .WithName("GetNotifications")
            .RequireAuthorization("TutorOrStudentPolicy")
            .RequireAuthorization("ActiveUserOnly")
            .Produces<NotificationsDto>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status401Unauthorized);
        
        group.MapPut("/notification/read/{notificationId}",
                async (IMediator mediator, HttpContext httpContext, int  notificationId) =>
                {
                    var userIdClaim = httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                    if (string.IsNullOrEmpty(userIdClaim))
                        return Results.Unauthorized();

                    if (!int.TryParse(userIdClaim, out var userId))
                        return Results.BadRequest("Invalid UserId in token");
                    var command = new ReadNotificationCommand(userId, notificationId);
                    var result = await mediator.Send(command);
                    return result.IsSuccess
                        ? Results.Ok(result.Value)
                        : Results.BadRequest(result.Errors);
                })
            .WithName("ReadNotification")
            .RequireAuthorization("TutorOrStudentPolicy")
            .RequireAuthorization("ActiveUserOnly")
            .Produces<NotificationsDto>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status401Unauthorized);

        group.MapPut("/changestatus",
            async (IMediator mediator, [FromBody] ActivateDeactivateDto activateDeactivateDto) =>
            {
                var command = new ActivateDeactivateStatusCommand(activateDeactivateDto);
                var result = await mediator.Send(command);
                return result.IsSuccess
                    ? Results.Ok(result.Value)
                    : Results.BadRequest(result.Errors);
            })  .WithName("ChangeIsActiveStatus")
            .RequireAuthorization("AdminPolicy")
            .RequireAuthorization("ActiveUserOnly")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status401Unauthorized);
       
        group.MapPost("/admin/create",
                async (IMediator mediator, [FromBody] UserIdDto userIdDto) =>
                {
                    var command = new CreateAdminCommand(userIdDto.UserId);
                    var result = await mediator.Send(command);
                    return result.IsSuccess
                        ? Results.Ok(result.Value)
                        : Results.BadRequest(result.Errors);
                })  .WithName("CreateAdmin")
            .RequireAuthorization("AdminPolicy")
            .RequireAuthorization("ActiveUserOnly")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status401Unauthorized);

        

        var healthGroup = builder.MapGroup("health")
            .WithTags("health");

        healthGroup.MapGet("/", () =>
            Results.Ok(new { status = "Healthy", timestamp = DateTime.UtcNow, service = "User API" }));
    }
}