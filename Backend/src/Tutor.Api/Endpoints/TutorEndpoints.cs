using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using System.Collections.Generic;
using System.Security.Claims;
using Tutor.Api.Filters.Atributes;
using Tutor.Application.Features.Tutors.AddTutorSubject;
using Tutor.Application.Features.Tutors.Approve_Tutor;
using Tutor.Application.Features.Tutors.CreateAvailability;
using Tutor.Application.Features.Tutors.CreateTutor;
using Tutor.Application.Features.Tutors.DeclineTutor;
using Tutor.Application.Features.Tutors.DeleteAvilability;
using Tutor.Application.Features.Tutors.DeleteTutorSubject;
using Tutor.Application.Features.Tutors.Dto;
using Tutor.Application.Features.Tutors.GetAllTutors;
using Tutor.Application.Features.Tutors.GetTutorAvailability;
using Tutor.Application.Features.Tutors.GetTutorById;
using Tutor.Application.Features.Tutors.UpdateAvailability;
using Tutor.Application.Features.Tutors.UpdateTutorProfile;
using Tutor.Application.Features.Tutors.UpdateTutorSubject;


namespace Tutor.Api.Endpoints;

public static class TutorEndpoints
{
    public static void MapTutorEndpoints(this IEndpointRouteBuilder builder)
    {
        var group = builder.MapGroup("api/tutors")
            .WithTags("tutors");

        group.MapPost("/create-tutor",
                 async (IMediator mediator, [FromBody] CreateTutorProfileDto createTutorProfileDto,
                    HttpContext httpContext) =>
                {
                    var userIdClaim = httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                    if (string.IsNullOrEmpty(userIdClaim))
                        return Results.Unauthorized();

                    if (!int.TryParse(userIdClaim, out var userId))
                        return Results.BadRequest("Invalid UserId in token");

                    var command = new CreateTutorCommand(userId, createTutorProfileDto);

                    var result = await mediator.Send(command);

                    return result.IsSuccess
                        ? Results.Ok(result.Value)
                        : Results.BadRequest(result.Errors);
                })
            .WithName("CreateTutorProfile")
            .Produces<TutorProfileDto>(StatusCodes.Status200OK)
            .RequireAuthorization("ActiveUserOnly")
            .RequireAuthorization("TutorPolicy") 
            .Produces(StatusCodes.Status401Unauthorized);
        
        group.MapPut("/update-tutor",
                async (IMediator mediator, [FromBody] UpdateTutorProfileDto updateTutorProfileDto,
                    HttpContext httpContext) =>
                {
                    var userIdClaim = httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                    if (string.IsNullOrEmpty(userIdClaim))
                        return Results.Unauthorized();

                    if (!int.TryParse(userIdClaim, out var userId))
                        return Results.BadRequest("Invalid UserId in token");

                    var command = new UpdateTutorProfileCommand(updateTutorProfileDto, userId);

                    var result = await mediator.Send(command);

                    return result.IsSuccess
                        ? Results.Ok(result.Value)
                        : Results.BadRequest(result.Errors);
                })
            .WithName("UpdateTutorProfile")
            .Produces<TutorProfileDto>(StatusCodes.Status200OK)
            .RequireAuthorization("ActiveUserOnly")
            .RequireAuthorization("TutorPolicy") 
            .Produces(StatusCodes.Status401Unauthorized);

        group.MapGet("/get-tutor/{id}",
                async (IMediator mediator, int id) =>
                {
                    var command = new GetTutorByIdQuery(id);

                    var result = await mediator.Send(command);
                    return result.IsSuccess
                        ? Results.Ok(result.Value)
                        : Results.BadRequest(result.Errors);
                }).WithName("GetTutorProfile")
            .Produces<TutorProfileDto>(StatusCodes.Status200OK)
            .RequireAuthorization("TutorOrStudentPolicy")
            .Produces(StatusCodes.Status401Unauthorized);

        group.MapGet("/get-tutors",
               
                async (IMediator mediator,
                    [FromQuery] string? city = null,
                    [FromQuery] string? country = null,
                    [FromQuery] int[]? subjectId = null,
                    [FromQuery] int[]? ratings = null,
                    [FromQuery] decimal? minPrice = null,
                    [FromQuery] decimal? maxPrice = null,
                    [FromQuery] string? sortBy = null,
                    [FromQuery] bool sortDescending = false) =>
                {
                     var command = new GetAllTutorsQuery(
                        city, 
                        country, 
                        subjectId, 
                        ratings,
                        minPrice, 
                        maxPrice, 
                        sortBy, 
                        sortDescending
                    );;

                    var result = await mediator.Send(command);
                    return result.IsSuccess
                        ? Results.Ok(result.Value)
                        : Results.BadRequest(result.Errors);
                }).WithName("GetAllTutorProfile")
            .Produces<List<TutorProfileDto>>(StatusCodes.Status200OK)
            .RequireAuthorization("ActiveUserOnly")
            .RequireAuthorization("AdminOrStudentPolicy") 
            .Produces(StatusCodes.Status401Unauthorized);

        group.MapPut("/approve-tutor/{id}",
                [Authorize] async (IMediator mediator, int id) =>
                {
                    var command = new ApproveTutorCommand(id);
                    var result = await mediator.Send(command);
                    return result.IsSuccess
                        ? Results.Ok(result.Value)
                        : Results.BadRequest(result.Errors);
                }).WithName("ApproveTutor")
            .Produces<TutorProfileDto>(StatusCodes.Status200OK)
            .RequireAuthorization("AdminPolicy") 
            .RequireAuthorization("ActiveUserOnly")
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized);

        group.MapPut("/decline-tutor/{id}",
                [Authorize] async (IMediator mediator, int id) =>
                {
                    var command = new DeclineTutorCommand(id);
                    var result = await mediator.Send(command);
                    return result.IsSuccess
                        ? Results.Ok(result.Value)
                        : Results.BadRequest(result.Errors);
                }).WithName("DeclineTutor")
            .Produces<TutorProfileDto>(StatusCodes.Status200OK)
            .RequireAuthorization("AdminPolicy") 
            .RequireAuthorization("ActiveUserOnly")
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized);

        group.MapPost("/add-subject",
                async (IMediator mediator, HttpContext httpContext, [FromBody] TutorSubjectRequestDto tutorSubjectRequestDto) =>
                {
                    var userIdClaim = httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                    if (string.IsNullOrEmpty(userIdClaim))
                        return Results.Unauthorized();

                    if (!int.TryParse(userIdClaim, out var userId))
                        return Results.BadRequest("Invalid UserId in token");
                    var command = new AddTutorSubjectCommand(tutorSubjectRequestDto, userId);
                    var result = await mediator.Send(command);
                    return result.IsSuccess
                        ? Results.Ok(result.Value)
                        : Results.BadRequest(result.Errors);
                }).WithName("AddSubject")
            .Produces<TutorProfileDto>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .RequireAuthorization("TutorPolicy") 
            .RequireAuthorization("ActiveUserOnly")
            .Produces(StatusCodes.Status401Unauthorized);
        
        group.MapDelete("/delete-subject/{subjectId}",
                async (IMediator mediator, HttpContext httpContext,  int subjectId) =>
                {
                    var userIdClaim = httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                    if (string.IsNullOrEmpty(userIdClaim))
                        return Results.Unauthorized();

                    if (!int.TryParse(userIdClaim, out var userId))
                        return Results.BadRequest("Invalid UserId in token");
                    var command = new DeleteTutorSubjectCommand(userId, subjectId);
                    var result = await mediator.Send(command);
                    return result.IsSuccess
                        ? Results.Ok(result.Value)
                        : Results.BadRequest(result.Errors);
                }).WithName("DeleteSubject")
            .Produces<TutorProfileDto>(StatusCodes.Status200OK)
            .RequireAuthorization("TutorPolicy") 
            .RequireAuthorization("ActiveUserOnly")
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized);
        
        group.MapPut("/update-subject",
                async (IMediator mediator, HttpContext httpContext, [FromBody] TutorSubjectDto tutorSubjectDto) =>
                {
                    var userIdClaim = httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                    if (string.IsNullOrEmpty(userIdClaim))
                        return Results.Unauthorized();

                    if (!int.TryParse(userIdClaim, out var userId))
                        return Results.BadRequest("Invalid UserId in token");
                    var command = new UpdateTutorSubjectCommand(userId, tutorSubjectDto);
                    var result = await mediator.Send(command);
                    return result.IsSuccess
                        ? Results.Ok(result.Value)
                        : Results.BadRequest(result.Errors);
                }).WithName("UpdateSubject")
            .Produces<TutorProfileDto>(StatusCodes.Status200OK)
            .RequireAuthorization("TutorPolicy") 
            .RequireAuthorization("ActiveUserOnly")
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized);
        group.MapPost("availability/create",
            async (IMediator mediator, [FromBody] CreateAvailabilityDto createAvailabilityDto, HttpContext httpContext) =>
            {
                var userIdClaim = httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userIdClaim))
                    return Results.Unauthorized();

                if (!int.TryParse(userIdClaim, out var userId))
                    return Results.BadRequest("Invalid UserId in token");
                var command = new CreateAvailabilityCommand(userId, createAvailabilityDto);
                var result = await mediator.Send(command);
                return result.IsSuccess
                    ? Results.Ok(result.Value)
                    : Results.BadRequest(result.Errors);
            }).WithName("CreateAvailability")
            .Produces<AvailabilityDto>(StatusCodes.Status200OK)
            .RequireAuthorization("TutorPolicy")
            .RequireAuthorization("ActiveUserOnly")
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized);
        group.MapPut("availability/update",
                async (IMediator mediator, [FromBody] AvailabilityDto availabilityDto, HttpContext httpContext) =>
                {
                    var userIdClaim = httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                    if (string.IsNullOrEmpty(userIdClaim))
                        return Results.Unauthorized();

                    if (!int.TryParse(userIdClaim, out var userId))
                        return Results.BadRequest("Invalid UserId in token");
                    var command = new UpdateAvailabilityCommand(userId, availabilityDto);
                    var result = await mediator.Send(command);
                    return result.IsSuccess
                        ? Results.Ok(result.Value)
                        : Results.BadRequest(result.Errors);
                }).WithName("UpdateAvailability")
            .Produces<AvailabilityDto>(StatusCodes.Status200OK)
            .RequireAuthorization("TutorPolicy")
            .RequireAuthorization("ActiveUserOnly")
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized);
        group.MapDelete("availability/delete/{id}",
                async (IMediator mediator, HttpContext httpContext, int id) =>
                {
                    var userIdClaim = httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                    if (string.IsNullOrEmpty(userIdClaim))
                        return Results.Unauthorized();

                    if (!int.TryParse(userIdClaim, out var userId))
                        return Results.BadRequest("Invalid UserId in token");
                    var command = new DeleteAvailabilityCommand(userId, id);
                    var result = await mediator.Send(command);
                    return result.IsSuccess
                        ? Results.Ok(result.Value)
                        : Results.BadRequest(result.Errors);
                }).WithName("DeleteAvailability")
            .Produces(StatusCodes.Status200OK)
            .RequireAuthorization("TutorPolicy")
            .RequireAuthorization("ActiveUserOnly")
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized);
        
        group.MapGet("availability/{userId}",
                async (IMediator mediator, int userId) =>
                {
                    var command = new GetTutorAvailabilityCommand(userId);
                    var result = await mediator.Send(command);
                    return result.IsSuccess
                        ? Results.Ok(result.Value)
                        : Results.BadRequest(result.Errors);
                }).WithName("GetTutorAvailability")
            .Produces<List<AvailabilityDto>>(StatusCodes.Status200OK)
            .RequireAuthorization("TutorOrStudentPolicy")
            .RequireAuthorization("ActiveUserOnly")
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized);

    }
    
   
}
