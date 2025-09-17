using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using System.Security.Claims;
using Tutor.Application.Features.Booking.CreateBooking;
using Tutor.Application.Features.Booking.Dto;
using Tutor.Application.Features.Booking.UpdateBookingStatus;
using Tutor.Application.Features.Students.CreateStudent;
using Tutor.Application.Features.Students.DTOs;
using Tutor.Application.Features.Students.GetStudent;
using Tutor.Application.Features.Students.UpdateStudent;
using Tutor.Application.Features.Tutors.Dto;
using Tutor.Domain.Entities;

namespace Tutor.Api.Endpoints;

public static class StudentEndpoints
{
    public static void MapStudentEndpoints(this IEndpointRouteBuilder builder)
    {
        var group = builder.MapGroup("api/students")
            .WithTags("students");
        group.MapPost("/create-student",
            [Authorize] async (IMediator mediator, [FromBody] CreateStudentDto createStudentDto, HttpContext httpContext) =>
            {
                var userIdClaim = httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userIdClaim))
                    return Results.Unauthorized();

                if (!int.TryParse(userIdClaim, out var userId))
                    return Results.BadRequest("Invalid UserId in token");
                var command = new CreateStudentCommand(userId, createStudentDto);

                var result = await mediator.Send(command);

                return result.IsSuccess
                    ? Results.Ok(result.Value)
                    : Results.BadRequest(result.Errors);
            })
            .WithName("CreateStudentProfile")
            .RequireAuthorization("StudentPolicy") 
            .Produces<TutorProfileDto>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status401Unauthorized);
        
        group.MapGet("/student-profile",
                [Authorize] async (IMediator mediator,  HttpContext httpContext) =>
                {
                    var userIdClaim = httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                    if (string.IsNullOrEmpty(userIdClaim))
                        return Results.Unauthorized();

                    if (!int.TryParse(userIdClaim, out var userId))
                        return Results.BadRequest("Invalid UserId in token");
                    var command = new GetStudentCommand(userId);

                    var result = await mediator.Send(command);

                    return result.IsSuccess
                        ? Results.Ok(result.Value)
                        : Results.BadRequest(result.Errors);
                })
            .WithName("GetStudentProfile")
            .Produces<TutorProfileDto>(StatusCodes.Status200OK)
            .RequireAuthorization("StudentPolicy") 
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized);
        group.MapPut("/update-profile",
                 async (IMediator mediator, [FromBody] UpdateStudentProfileDto updateStudentProfileDto,HttpContext httpContext) =>
                {
                    var userIdClaim = httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                    if (string.IsNullOrEmpty(userIdClaim))
                        return Results.Unauthorized();

                    if (!int.TryParse(userIdClaim, out var userId))
                        return Results.BadRequest("Invalid UserId in token");
                    var command = new UpdateStudentCommand(userId, updateStudentProfileDto);

                    var result = await mediator.Send(command);

                    return result.IsSuccess
                        ? Results.Ok(result.Value)
                        : Results.BadRequest(result.Errors);
                })
            .WithName("UpdateStudentProfile")
            .Produces<TutorProfileDto>(StatusCodes.Status200OK)
            .RequireAuthorization("StudentPolicy") 
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized);
        
        group.MapPost("/booking/create",
            async (IMediator mediator, [FromBody] CreateBookingDto createBookingDto, HttpContext httpContext) =>
            {
                var userIdClaim = httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userIdClaim))
                    return Results.Unauthorized();

                if (!int.TryParse(userIdClaim, out var userId))
                    return Results.BadRequest("Invalid UserId in token");
                var command = new CreateBookingCommand(userId, createBookingDto);

                var result = await mediator.Send(command);

                return result.IsSuccess
                    ? Results.Ok(result.Value)
                    : Results.BadRequest(result.Errors);
            }).WithName("CreateBooking")
            .Produces<BookingDto>(StatusCodes.Status200OK)
            .RequireAuthorization("StudentPolicy") 
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized);
        
        group.MapPut("/booking/update/{bookingId}",
                async (IMediator mediator, [FromBody] BookingStatus status, int bookingId ,HttpContext httpContext) =>
                {
                    var userIdClaim = httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                    if (string.IsNullOrEmpty(userIdClaim))
                        return Results.Unauthorized();

                    if (!int.TryParse(userIdClaim, out var userId))
                        return Results.BadRequest("Invalid UserId in token");
                    var command = new UpdateBookingStatusCommand(userId, bookingId,status);

                    var result = await mediator.Send(command);

                    return result.IsSuccess
                        ? Results.Ok(result.Value)
                        : Results.BadRequest(result.Errors);
                }).WithName("UpdateBookingStatus")
            .Produces<BookingDto>(StatusCodes.Status200OK)
            .RequireAuthorization("TutorOrStudentPolicy") 
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized);
        
           
    }
}