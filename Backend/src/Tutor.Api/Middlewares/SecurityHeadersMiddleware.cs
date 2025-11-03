using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Tutor.Api.Middlewares;

/// <summary>
/// Middleware to add security headers including Content Security Policy (CSP)
/// </summary>
public class SecurityHeadersMiddleware
{
    private readonly RequestDelegate _next;

    public SecurityHeadersMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // Content Security Policy (CSP) - Secure policy without unsafe-inline
        // This policy allows external services used by the application while maintaining security
        // Key improvement: REMOVED unsafe-inline from style-src and script-src
        context.Response.Headers["Content-Security-Policy"] =
            "default-src 'self'; " +
            "script-src 'self' https://accounts.google.com https://apis.google.com; " +
            "style-src 'self' https://fonts.googleapis.com; " +
            "img-src 'self' data: https: blob:; " +
            "font-src 'self' https://fonts.gstatic.com; " +
            "connect-src 'self' https://accounts.google.com https://www.googleapis.com https://oauth2.googleapis.com https://res.cloudinary.com https://api.cloudinary.com; " +
            "frame-src 'self' https://accounts.google.com; " +
            "frame-ancestors 'none'; " +
            "base-uri 'self'; " +
            "form-action 'self';";

        // X-Content-Type-Options: Prevents MIME type sniffing
        context.Response.Headers["X-Content-Type-Options"] = "nosniff";

        // X-Frame-Options: Prevents clickjacking attacks
        context.Response.Headers["X-Frame-Options"] = "DENY";

        // Referrer-Policy: Controls referrer information
        context.Response.Headers["Referrer-Policy"] = "strict-origin-when-cross-origin";

        // Permissions-Policy: Controls browser features
        context.Response.Headers["Permissions-Policy"] =
            "accelerometer=(), camera=(), geolocation=(), gyroscope=(), magnetometer=(), microphone=(), payment=(), usb=()";

        await _next(context);
    }
}
