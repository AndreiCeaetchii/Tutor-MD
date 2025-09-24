using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using System.Security.Claims;
using Tutor.Application.Features.Booking.AddToCalendar;
using Tutor.Application.Features.Booking.CreateBooking;
using Tutor.Application.Features.Booking.Dto;
using Tutor.Application.Features.Booking.GetBooking;
using Tutor.Application.Features.Booking.GetBookingsByUser;
using Tutor.Application.Features.Booking.UpdateBookingStatus;
using Tutor.Application.Features.Reviews.CreateReview;
using Tutor.Application.Features.Reviews.DeleteReview;
using Tutor.Application.Features.Reviews.Dto;
using Tutor.Application.Features.Reviews.GetReviews;
using Tutor.Application.Features.Reviews.UpdateReview;
using Tutor.Application.Features.Students.CreateStudent;
using Tutor.Application.Features.Students.DTOs;
using Tutor.Application.Features.Students.GetAllStudents;
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
            async (IMediator mediator, [FromBody] CreateStudentDto createStudentDto, HttpContext httpContext) =>
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
            .RequireAuthorization("ActiveUserOnly")
            .Produces<StudentDto>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status401Unauthorized);
        
        group.MapGet("/student-profile",
                async (IMediator mediator,  HttpContext httpContext) =>
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
            .Produces<StudentDto>(StatusCodes.Status200OK)
            .RequireAuthorization("StudentPolicy") 
            .RequireAuthorization("ActiveUserOnly")
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized);
        group.MapGet("/all-students",
                async (IMediator mediator) =>
                {
                    var command = new GetAllStudentsCommand();

                    var result = await mediator.Send(command);

                    return result.IsSuccess
                        ? Results.Ok(result.Value)
                        : Results.BadRequest(result.Errors);
                })
            .WithName("AllStudents")
            .Produces<StudentDto>(StatusCodes.Status200OK)
            .RequireAuthorization("AdminPolicy") 
            .RequireAuthorization("ActiveUserOnly")
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
            .Produces<StudentDto>(StatusCodes.Status200OK)
            .RequireAuthorization("StudentPolicy") 
            .RequireAuthorization("ActiveUserOnly")
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
            .RequireAuthorization("ActiveUserOnly")
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
            .RequireAuthorization("ActiveUserOnly")
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized);
        
        group.MapGet("/booking/{bookingId}",
                async (IMediator mediator,int bookingId ,HttpContext httpContext) =>
                {
                    var userIdClaim = httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                    if (string.IsNullOrEmpty(userIdClaim))
                        return Results.Unauthorized();

                    if (!int.TryParse(userIdClaim, out var userId))
                        return Results.BadRequest("Invalid UserId in token");
                    var command = new GetBookingCommand(userId, bookingId);

                    var result = await mediator.Send(command);

                    return result.IsSuccess
                        ? Results.Ok(result.Value)
                        : Results.BadRequest(result.Errors);
                }).WithName("GetBooking")
            .Produces<BookingDto>(StatusCodes.Status200OK)
            .RequireAuthorization("TutorOrStudentPolicy") 
            .RequireAuthorization("ActiveUserOnly")
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized);
        
        group.MapGet("/bookings",
                async (IMediator mediator,HttpContext httpContext) =>
                {
                    var userIdClaim = httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                    if (string.IsNullOrEmpty(userIdClaim))
                        return Results.Unauthorized();

                    if (!int.TryParse(userIdClaim, out var userId))
                        return Results.BadRequest("Invalid UserId in token");
                    var command = new GetBookingsByUserCommand(userId);

                    var result = await mediator.Send(command);

                    return result.IsSuccess
                        ? Results.Ok(result.Value)
                        : Results.BadRequest(result.Errors);
                }).WithName("GetBookingsByTutor")
            .Produces<BookingDto>(StatusCodes.Status200OK)
            .RequireAuthorization("TutorOrStudentPolicy") 
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized);
        group.MapPut("/booking/add-calendar/{bookingId}",
                async (IMediator mediator,HttpContext httpContext,int bookingId ,[FromBody]string accessToken) =>
                {
                    var userIdClaim = httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                    if (string.IsNullOrEmpty(userIdClaim))
                        return Results.Unauthorized();

                    if (!int.TryParse(userIdClaim, out var userId))
                        return Results.BadRequest("Invalid UserId in token");
                    var command = new AddToCalendarCommand(userId, bookingId, accessToken);

                    var result = await mediator.Send(command);

                    return result.IsSuccess
                        ? Results.Ok(result.Value)
                        : Results.BadRequest(result.Errors);
                }).WithName("AddToGoogleCalendar")
            .Produces(StatusCodes.Status200OK)
            .RequireAuthorization("TutorOrStudentPolicy") 
            .RequireAuthorization("ActiveUserOnly")
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized);
        
        group.MapPost("/reviews/create",
                async (IMediator mediator, [FromBody] CreateReviewDto createReviewDto ,HttpContext httpContext) =>
                {
                    var userIdClaim = httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                    if (string.IsNullOrEmpty(userIdClaim))
                        return Results.Unauthorized();

                    if (!int.TryParse(userIdClaim, out var userId))
                        return Results.BadRequest("Invalid UserId in token");
                    var command = new CreateReviewCommand(userId, createReviewDto);

                    var result = await mediator.Send(command);

                    return result.IsSuccess
                        ? Results.Ok(result.Value)
                        : Results.BadRequest(result.Errors);
                }).WithName("CreateReview")
            .Produces<ReviewDto>(StatusCodes.Status200OK)
            .RequireAuthorization("StudentPolicy") 
            .RequireAuthorization("ActiveUserOnly")
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized);
        
        group.MapPut("/reviews/update/{reviewId}",
                async (IMediator mediator, [FromBody] UpdateReviewDto updateReviewDto ,HttpContext httpContext, int reviewId ) =>
                {
                    var userIdClaim = httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                    if (string.IsNullOrEmpty(userIdClaim))
                        return Results.Unauthorized();

                    if (!int.TryParse(userIdClaim, out var userId))
                        return Results.BadRequest("Invalid UserId in token");
                    var command = new UpdateReviewCommand(userId, updateReviewDto, reviewId);

                    var result = await mediator.Send(command);

                    return result.IsSuccess
                        ? Results.Ok(result.Value)
                        : Results.BadRequest(result.Errors);
                }).WithName("UpdateReview")
            .Produces<ReviewDto>(StatusCodes.Status200OK)
            .RequireAuthorization("StudentPolicy") 
            .RequireAuthorization("ActiveUserOnly")
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized);
        
        group.MapDelete("/reviews/delete/{reviewId}",
                async (IMediator mediator,HttpContext httpContext, int reviewId ) =>
                {
                    var userIdClaim = httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                    if (string.IsNullOrEmpty(userIdClaim))
                        return Results.Unauthorized();

                    if (!int.TryParse(userIdClaim, out var userId))
                        return Results.BadRequest("Invalid UserId in token");
                    var command = new DeleteReviewCommand(reviewId, userId);

                    var result = await mediator.Send(command);

                    return result.IsSuccess
                        ? Results.Ok(result.Value)
                        : Results.BadRequest(result.Errors);
                }).WithName("DeleteReview")
            .Produces(StatusCodes.Status200OK)
            .RequireAuthorization("AdminOrStudentPolicy") 
            .RequireAuthorization("ActiveUserOnly")
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized);
        
        group.MapGet("/reviews/{tutorId}",
                async (IMediator mediator,HttpContext httpContext, int tutorId ) =>
                {
                    var userIdClaim = httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                    if (string.IsNullOrEmpty(userIdClaim))
                        return Results.Unauthorized();

                    if (!int.TryParse(userIdClaim, out var userId))
                        return Results.BadRequest("Invalid UserId in token");
                    var command = new GetReviewsCommand(tutorId, userId);

                    var result = await mediator.Send(command);

                    return result.IsSuccess
                        ? Results.Ok(result.Value)
                        : Results.BadRequest(result.Errors);
                }).WithName("GetReviews")
            .Produces(StatusCodes.Status200OK)
            .RequireAuthorization("AdminOrTutorOrStudentPolicy") 
            .RequireAuthorization("ActiveUserOnly")
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized);
        
           
    }
}