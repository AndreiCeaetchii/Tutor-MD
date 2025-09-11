using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using System.Collections.Generic;
using System.Security.Claims;
using Tutor.Application.Features.Tutors.AddTutorSubject;
using Tutor.Application.Features.Tutors.Approve_Tutor;
using Tutor.Application.Features.Tutors.CreateTutor;
using Tutor.Application.Features.Tutors.DeclineTutor;
using Tutor.Application.Features.Tutors.DeleteTutorSubject;
using Tutor.Application.Features.Tutors.Dto;
using Tutor.Application.Features.Tutors.GetAllTutors;
using Tutor.Application.Features.Tutors.GetTutorById;
using Tutor.Application.Features.Users.CreateProfile;
using Tutor.Application.Features.Users.Dtos;


namespace Tutor.Api.Endpoints;

public static class TutorEndpoints
{
    public static void MapTutorEndpoints(this IEndpointRouteBuilder builder)
    {
        var group = builder.MapGroup("api/tutors")
            .WithTags("tutors");

        group.MapPost("/create-tutor",
                [Authorize] async (IMediator mediator, [FromBody] CreateTutorProfileDto createTutorProfileDto,
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
            .Produces(StatusCodes.Status401Unauthorized);

        group.MapGet("/get-tutor/{id}",
                [Authorize] async (IMediator mediator, int id) =>
                {
                    var command = new GetTutorByIdQuery(id);

                    var result = await mediator.Send(command);
                    return result.IsSuccess
                        ? Results.Ok(result.Value)
                        : Results.BadRequest(result.Errors);
                }).WithName("GetTutorProfile")
            .Produces<TutorProfileDto>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status401Unauthorized);

        group.MapGet("/get-tutors",
                [Authorize] async (IMediator mediator) =>
                {
                    var command = new GetAllTutorsQuery();

                    var result = await mediator.Send(command);
                    return result.IsSuccess
                        ? Results.Ok(result.Value)
                        : Results.BadRequest(result.Errors);
                }).WithName("GetAllTutorProfile")
            .Produces<List<TutorProfileDto>>(StatusCodes.Status200OK)
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
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized);

        group.MapPost("/add-subject",
                [Authorize]
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
            .Produces(StatusCodes.Status401Unauthorized);
        
        group.MapDelete("/delete-subject/{subjectId}",
                [Authorize]
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
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized);
    }
}