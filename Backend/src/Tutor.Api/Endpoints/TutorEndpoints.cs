using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using System.Collections.Generic;
using System.Security.Claims;
using Tutor.Application.Features.Tutors.CreateTutor;
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
                [Authorize]
                async (IMediator mediator, [FromBody] CreateTutorProfileDto createTutorProfileDto, HttpContext httpContext) =>
                {
                    // Extract UserId from JWT claims
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
            [Authorize] async (IMediator mediator,int id) =>
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
    }
}