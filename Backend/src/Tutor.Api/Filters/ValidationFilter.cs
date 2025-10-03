using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Tutor.Api.Filters;

public class ValidationFilter : IEndpointFilter
{
    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        // Find DTO parameters with validation attributes
        foreach (var argument in context.Arguments)
        {
            if (argument != null && HasValidationAttributes(argument.GetType()))
            {
                var validationResults = new List<ValidationResult>();
                var isValid = Validator.TryValidateObject(argument, 
                    new ValidationContext(argument), validationResults, true);

                if (!isValid)
                {
                    var errors = validationResults.Select(v => v.ErrorMessage).ToList();
                    return Results.BadRequest(new { errors });
                }
            }
        }

        return await next(context);
    }

    private static bool HasValidationAttributes(Type type)
    {
        return type.GetProperties()
            .Any(prop => prop.GetCustomAttributes<ValidationAttribute>().Any());
    }
}