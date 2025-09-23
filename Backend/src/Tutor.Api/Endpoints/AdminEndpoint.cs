using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System.Collections.Generic;
using Tutor.Application.Features.Tutors.Dto;
using Tutor.Application.Features.Tutors.GetAllTutorsForAdmin;

namespace Tutor.Api.Endpoints;

public static class AdminEndpoints
{
    public static void MapAdminEndpoints(this IEndpointRouteBuilder builder)
    {
        var group = builder.MapGroup("api/admin")
            .WithTags("admin");
    
        group.MapGet("/tutors", 
                async (IMediator mediator) =>
                {
                   
                    var command = new GetAllTutorsAdminCommand();

                    var result = await mediator.Send(command);

                    return result.IsSuccess
                        ? Results.Ok(result.Value)
                        : Results.BadRequest(result.Errors);
                })
            .WithName("GetTutorsForAdminProfile")
            .RequireAuthorization("AdminPolicy")
            .RequireAuthorization("ActiveUserOnly")
            .Produces<List<TutorProfileDto>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status401Unauthorized);
    }   
}
