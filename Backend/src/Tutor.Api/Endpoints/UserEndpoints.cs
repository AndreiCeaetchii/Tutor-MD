using MediatR;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Security.Claims;
using Tutor.Api.Filters;
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
using Tutor.Application.Features.Users.DisableMFA;
using Tutor.Application.Features.Users.Dtos;
using Tutor.Application.Features.Users.EnableMfa;
using Tutor.Application.Features.Users.GetResetPassword;
using Tutor.Application.Features.Users.LoginOAuthUser;
using Tutor.Application.Features.Users.LoginUser;
using Tutor.Application.Features.Users.RefreshToken;
using Tutor.Application.Features.Users.RegisterOAuthUser;
using Tutor.Application.Features.Users.RegisterUser;
using Tutor.Application.Features.Users.ResetPassword;
using Tutor.Application.Features.Users.VerifyMFA;

namespace Tutor.Api.Endpoints;

public static class UserEndpoints
{
    public static void MapUserEndpoints(this IEndpointRouteBuilder builder)
    {
        var group = builder.MapGroup("api/users")
            .WithTags("users");

        group.MapPost("/register",
                async (IMediator mediator, [FromBody] RegisterUserDto registerUserDto, HttpContext context) =>
                {
                    var command = new RegisterUserCommand(registerUserDto);
                    var result = await mediator.Send(command);

                    if (result.IsSuccess)
                    {
                        var refreshToken = result.Value.RefreshToken;
                        result.Value.RefreshToken = null;
                        var cookieOptions = new CookieOptions
                        {
                            HttpOnly = true,
                            Secure = true, // Only send over HTTPS
                            SameSite = SameSiteMode.Strict,
                            Expires = result.Value.RefreshTokenExpiryTime, // Use the expiry time from response
                            Path = "/", // Available across the entire site
                            IsEssential = true // Important for GDPR compliance
                        };

                        context.Response.Cookies.Append("refreshToken", refreshToken, cookieOptions);

                        return Results.Ok(result.Value);
                    }
                    else
                    {
                        return Results.BadRequest(result.Errors);
                    }
                })
            .AddEndpointFilter<ValidationFilter>()
            .Produces<UserResponseDto>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithName("RegisterUser");

        group.MapPost("/login", async (IMediator mediator, [FromBody] LoginUserDto loginUserDto, HttpContext context) =>
            {
                var command = new LoginUserCommand(loginUserDto);
                var result = await mediator.Send(command);
                if (result.IsSuccess)
                {
                    var refreshToken = result.Value.RefreshToken;
                    result.Value.RefreshToken = null;
                    var cookieOptions = new CookieOptions
                    {
                        HttpOnly = true,
                        Secure = true, // Only send over HTTPS
                        SameSite = SameSiteMode.Strict,
                        Expires = result.Value.RefreshTokenExpiryTime, // Use the expiry time from response
                        Path = "/", // Available across the entire site
                        IsEssential = true // Important for GDPR compliance
                    };

                    context.Response.Cookies.Append("refreshToken", refreshToken, cookieOptions);

                    return Results.Ok(result.Value);
                }
                else
                {
                    return Results.BadRequest(result.Errors);
                }
            }).Produces<UserResponseDto>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithName("LoginUser");
        group.MapPut("/refresh", async (IMediator mediator, HttpContext context) =>
            {
                var refreshToken = context.Request.Cookies["refreshToken"];
                var authHeader = context.Request.Headers["Authorization"].ToString();
                if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("bearer "))
                {
                    return Results.BadRequest("Valid Authorization header with Bearer token is required");
                }

                var accessToken = authHeader.Substring("Bearer ".Length).Trim();
                if (string.IsNullOrEmpty(accessToken) || string.IsNullOrEmpty(refreshToken))
                {
                    return Results.BadRequest("Access token and refresh token are required");
                }

                var command = new RefreshTokenCommand(accessToken, refreshToken);
                var result = await mediator.Send(command);

                if (result.IsSuccess)
                {
                    var newRefreshToken = result.Value.RefreshToken;
                    var newRefreshTokenExpiry = result.Value.RefreshTokenExpiryTime;
                    result.Value.RefreshToken = null;
                    var cookieOptions = new CookieOptions
                    {
                        HttpOnly = true,
                        Secure = true,
                        SameSite = SameSiteMode.Strict,
                        Expires = newRefreshTokenExpiry,
                        Path = "/",
                        IsEssential = true
                    };
                    context.Response.Cookies.Append("refreshToken", newRefreshToken, cookieOptions);

                    return Results.Ok(result.Value);
                }
                else
                {
                    context.Response.Cookies.Delete("refreshToken");
                    return Results.BadRequest(result.Errors);
                }
            }).Produces<TokenResponseDto>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithName("RefreshUser");


        group.MapPost("/register-auth",
                async (IMediator mediator, [FromBody] RegisterUserAuthDto registerUserAuthDto, HttpContext context) =>
                {
                    var command = new RegisterUserWithOAuthCommand(registerUserAuthDto);
                    var result = await mediator.Send(command);

                    if (result.IsSuccess)
                    {
                        var refreshToken = result.Value.RefreshToken;
                        result.Value.RefreshToken = null;
                        var cookieOptions = new CookieOptions
                        {
                            HttpOnly = true,
                            Secure = true, // Only send over HTTPS
                            SameSite = SameSiteMode.Strict,
                            Expires = result.Value.RefreshTokenExpiryTime, // Use the expiry time from response
                            Path = "/", // Available across the entire site
                            IsEssential = true // Important for GDPR compliance
                        };

                        context.Response.Cookies.Append("refreshToken", refreshToken, cookieOptions);

                        return Results.Ok(result.Value);
                    }
                    else
                    {
                        return Results.BadRequest(result.Errors);
                    }
                }).Produces<UserResponseDto>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithName("RegisterAuthUser");

        group.MapPost("/login-auth",
                async (IMediator mediator, [FromBody] LoginUserAuthDto loginUserAuthDto, HttpContext context) =>
                {
                    var command = new LoginOAuthUserCommand(loginUserAuthDto);
                    var result = await mediator.Send(command);
                    if (result.IsSuccess)
                    {
                        var refreshToken = result.Value.RefreshToken;
                        result.Value.RefreshToken = null;
                        var cookieOptions = new CookieOptions
                        {
                            HttpOnly = true,
                            Secure = true,
                            SameSite = SameSiteMode.Strict,
                            Expires = result.Value.RefreshTokenExpiryTime,
                            Path = "/",
                            IsEssential = true
                        };

                        context.Response.Cookies.Append("refreshToken", refreshToken, cookieOptions);

                        return Results.Ok(result.Value);
                    }
                    else
                    {
                        return Results.BadRequest(result.Errors);
                    }
                }).Produces<UserResponseDto>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithName("LoginAuthUser");
        group.MapPut("/password",
                async (IMediator mediator, [FromBody] GetResetTokenDto resetTokenDto) =>
                {
                    var command = new GetResetPasswordCommand(resetTokenDto);
                    var result = await mediator.Send(command);
                    return result.IsSuccess
                        ? Results.Ok(result.Value)
                        : Results.BadRequest(result.Errors);
                }).WithName("RequestResetPassword")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status401Unauthorized);
        group.MapPut("/password-reset",
                async (IMediator mediator, [FromBody] ResetPasswordDto resetPasswordDto) =>
                {
                    var command = new ResetPasswordCommand(resetPasswordDto);
                    var result = await mediator.Send(command);
                    return result.IsSuccess
                        ? Results.Ok(result.Value)
                        : Results.BadRequest(result.Errors);
                }).WithName("ResetPassword")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status401Unauthorized);

        group.MapPut("/enable-mfa",
                async (IMediator mediator, HttpContext httpContext) =>
                {
                    var userIdClaim = httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                    if (string.IsNullOrEmpty(userIdClaim))
                        return Results.Unauthorized();

                    if (!int.TryParse(userIdClaim, out var userId))
                        return Results.BadRequest("Invalid UserId in token");
                    var command = new EnableMfaCommand(userId);
                    var result = await mediator.Send(command);

                    return result.IsSuccess
                        ? Results.Ok(result.Value)
                        : Results.BadRequest(result.Errors);
                }).Produces<EnableMFAResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .RequireAuthorization("TutorOrStudentPolicy")
            .RequireAuthorization("ActiveUserOnly")
            .WithName("EnableMfa");
        group.MapPut("/verify-enable-mfa",
                async (IMediator mediator, HttpContext httpContext,
                    [FromBody] VerifyMFACodeRequest verifyMFACodeRequest) =>
                {
                    var userIdClaim = httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                    if (string.IsNullOrEmpty(userIdClaim))
                        return Results.Unauthorized();

                    if (!int.TryParse(userIdClaim, out var userId))
                        return Results.BadRequest("Invalid UserId in token");
                    var command = new VerifyMfaCommand(userId, verifyMFACodeRequest);
                    var result = await mediator.Send(command);

                    return result.IsSuccess
                        ? Results.Ok(result.Value)
                        : Results.BadRequest(result.Errors);
                }).Produces<bool>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .RequireAuthorization("TutorOrStudentPolicy")
            .RequireAuthorization("ActiveUserOnly")
            .WithName("VerifyMfa");

        group.MapPut("/disable-mfa",
                async (IMediator mediator, HttpContext httpContext) =>
                {
                    var userIdClaim = httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                    if (string.IsNullOrEmpty(userIdClaim))
                        return Results.Unauthorized();

                    if (!int.TryParse(userIdClaim, out var userId))
                        return Results.BadRequest("Invalid UserId in token");
                    var command = new DisableMFACommand(userId);
                    var result = await mediator.Send(command);

                    return result.IsSuccess
                        ? Results.Ok(result.Value)
                        : Results.BadRequest(result.Errors);
                }).Produces<bool>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .RequireAuthorization("TutorOrStudentPolicy")
            .RequireAuthorization("ActiveUserOnly")
            .WithName("DisableMfa");


        group.MapPost("/add-photo",
                [Authorize] async (IMediator mediator, [FromForm] IFormFile file, HttpContext httpContext) =>
                {
                    // Then your business logic
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
            .Produces(StatusCodes.Status403Forbidden)
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
                async (IMediator mediator, HttpContext httpContext, int notificationId) =>
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
                }).WithName("ChangeIsActiveStatus")
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
                }).WithName("CreateAdmin")
            .RequireAuthorization("AdminPolicy")
            .RequireAuthorization("ActiveUserOnly")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status401Unauthorized);


        group.MapGet("/csrf-token", (IAntiforgery antiforgery, HttpContext context) =>
            {
                var tokens = antiforgery.GetAndStoreTokens(context);
                return Results.Ok(new { csrfToken = tokens.RequestToken });
            }).WithName("GetCsrfToken")
            .RequireAuthorization("AdminOrTutorOrStudentPolicy");


        var healthGroup = builder.MapGroup("health")
            .WithTags("health");

        healthGroup.MapGet("/", () =>
            Results.Ok(new { status = "Healthy", timestamp = DateTime.UtcNow, service = "User API" }));
    }
}