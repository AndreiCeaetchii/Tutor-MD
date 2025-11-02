using Microsoft.AspNetCore.Builder;
using System;
using System.Threading.RateLimiting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Tutor.Api.Configurations;

public static class RateLimitingSetup
{
    public static IServiceCollection AddRateLimitingSetup(this IServiceCollection services, IConfiguration configuration)
    {
        var rateLimitOptions = configuration.GetSection("RateLimiting");

        services.AddRateLimiter(options =>
        {
            // Note: GlobalLimiter is commented out to avoid conflicts with endpoint-specific limiters
            // Uncomment if you want an overall API rate limit in addition to endpoint limits
            /*
            options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(context =>
            {
                var userIdentifier = context.User.Identity?.IsAuthenticated == true
                    ? context.User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value ?? "anonymous"
                    : context.Connection.RemoteIpAddress?.ToString() ?? "anonymous";

                return RateLimitPartition.GetFixedWindowLimiter(userIdentifier, _ => new FixedWindowRateLimiterOptions
                {
                    PermitLimit = rateLimitOptions.GetValue<int>("Global:PermitLimit", 1000),
                    Window = TimeSpan.FromMinutes(rateLimitOptions.GetValue<int>("Global:WindowMinutes", 1)),
                    QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                    QueueLimit = 0
                });
            });
            */

            // Authentication endpoints (login, register, MFA) - IP-based, very strict
            options.AddFixedWindowLimiter("auth", limiterOptions =>
            {
                limiterOptions.PermitLimit = rateLimitOptions.GetValue<int>("Auth:PermitLimit", 5);
                limiterOptions.Window = TimeSpan.FromMinutes(rateLimitOptions.GetValue<int>("Auth:WindowMinutes", 1));
                limiterOptions.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
                limiterOptions.QueueLimit = 0;
            });

            // Password reset endpoints - IP-based, extremely strict
            options.AddFixedWindowLimiter("password-reset", limiterOptions =>
            {
                limiterOptions.PermitLimit = rateLimitOptions.GetValue<int>("PasswordReset:PermitLimit", 3);
                limiterOptions.Window = TimeSpan.FromMinutes(rateLimitOptions.GetValue<int>("PasswordReset:WindowMinutes", 15));
                limiterOptions.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
                limiterOptions.QueueLimit = 0;
            });

            // Sensitive operations (bookings, payments, profile changes) - User-based
            options.AddSlidingWindowLimiter("sensitive", limiterOptions =>
            {
                limiterOptions.PermitLimit = rateLimitOptions.GetValue<int>("Sensitive:PermitLimit", 10);
                limiterOptions.Window = TimeSpan.FromMinutes(rateLimitOptions.GetValue<int>("Sensitive:WindowMinutes", 1));
                limiterOptions.SegmentsPerWindow = rateLimitOptions.GetValue<int>("Sensitive:SegmentsPerWindow", 2);
                limiterOptions.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
                limiterOptions.QueueLimit = 0;
            });

            // General API endpoints - User or IP-based
            options.AddSlidingWindowLimiter("api", limiterOptions =>
            {
                limiterOptions.PermitLimit = rateLimitOptions.GetValue<int>("Api:PermitLimit", 100);
                limiterOptions.Window = TimeSpan.FromMinutes(rateLimitOptions.GetValue<int>("Api:WindowMinutes", 1));
                limiterOptions.SegmentsPerWindow = rateLimitOptions.GetValue<int>("Api:SegmentsPerWindow", 3);
                limiterOptions.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
                limiterOptions.QueueLimit = 2;
            });

            // Token bucket for endpoints that allow bursts (e.g., search, browse)
            options.AddTokenBucketLimiter("burst", limiterOptions =>
            {
                limiterOptions.TokenLimit = rateLimitOptions.GetValue<int>("Burst:TokenLimit", 50);
                limiterOptions.ReplenishmentPeriod = TimeSpan.FromSeconds(rateLimitOptions.GetValue<int>("Burst:ReplenishmentSeconds", 10));
                limiterOptions.TokensPerPeriod = rateLimitOptions.GetValue<int>("Burst:TokensPerPeriod", 10);
                limiterOptions.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
                limiterOptions.QueueLimit = 5;
            });

            // Concurrency limiter for resource-intensive operations
            options.AddConcurrencyLimiter("concurrent", limiterOptions =>
            {
                limiterOptions.PermitLimit = rateLimitOptions.GetValue<int>("Concurrent:PermitLimit", 10);
                limiterOptions.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
                limiterOptions.QueueLimit = 5;
            });

            // Customize rejection response
            options.OnRejected = async (context, cancellationToken) =>
            {
                context.HttpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;

                if (context.Lease.TryGetMetadata(MetadataName.RetryAfter, out var retryAfter))
                {
                    await context.HttpContext.Response.WriteAsJsonAsync(new
                    {
                        error = "Too Many Requests",
                        message = "Rate limit exceeded. Please try again later.",
                        retryAfter = retryAfter.ToString()
                    }, cancellationToken: cancellationToken);
                }
                else
                {
                    await context.HttpContext.Response.WriteAsJsonAsync(new
                    {
                        error = "Too Many Requests",
                        message = "Rate limit exceeded. Please try again later."
                    }, cancellationToken: cancellationToken);
                }
            };
        });

        return services;
    }
}
