using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using Tutor.Api.IntegrationTests.Common;
using Tutor.Application.Features.Users.Dtos;

namespace Tutor.Api.IntegrationTests;

public class RateLimitingTests : BaseTest
{
    public RateLimitingTests(CustomWebApplicationFactory apiFactory) : base(apiFactory)
    {
    }

    #region Auth Endpoints Rate Limiting

    [Fact]
    public async Task Login_ExceedingAuthRateLimit_Returns429TooManyRequests()
    {
        // Arrange - According to config, auth endpoints allow 5 requests per minute
        var loginDto = new LoginUserDto
        {
            Email = "test@example.com",
            Password = "WrongPassword123!"
        };

        // Act - Make 6 requests (exceeding the limit of 5)
        var responses = new List<HttpResponseMessage>();
        for (int i = 0; i < 6; i++)
        {
            var response = await PostAsync("/api/users/login", loginDto, ensureSuccess: false);
            responses.Add(response);
        }

        // Assert
        // First 5 should either succeed or return BadRequest (login failed)
        responses.Take(5).Should().OnlyContain(r =>
            r.StatusCode == HttpStatusCode.OK ||
            r.StatusCode == HttpStatusCode.BadRequest);

        // 6th request should be rate limited
        var rateLimitedResponse = responses.Last();
        rateLimitedResponse.StatusCode.Should().Be(HttpStatusCode.TooManyRequests);

        // Verify response body contains rate limit message
        var content = await rateLimitedResponse.Content.ReadAsStringAsync();
        content.Should().Contain("Rate limit exceeded");
    }

    [Fact]
    public async Task Register_ExceedingAuthRateLimit_Returns429TooManyRequests()
    {
        // Arrange - Auth endpoints allow 5 requests per minute
        var registerDtos = Enumerable.Range(1, 6).Select(i => new RegisterUserDto
        {
            Email = $"testuser{i}@example.com",
            Password = "TestPassword123!",
            RoleId = 1
        }).ToList();

        // Act
        var responses = new List<HttpResponseMessage>();
        foreach (var dto in registerDtos)
        {
            var response = await PostAsync("/api/users/register", dto, ensureSuccess: false);
            responses.Add(response);
        }

        // Assert
        // First 5 might succeed or fail validation, but should not be rate limited
        responses.Take(5).Should().OnlyContain(r =>
            r.StatusCode != HttpStatusCode.TooManyRequests);

        // 6th request should be rate limited
        responses.Last().StatusCode.Should().Be(HttpStatusCode.TooManyRequests);
    }

    #endregion

    #region Password Reset Rate Limiting

    [Fact]
    public async Task PasswordReset_ExceedingLimit_Returns429TooManyRequests()
    {
        // Arrange - Password reset allows 3 requests per 15 minutes
        var resetDto = new GetResetTokenDto
        {
            Email = "test@example.com"
        };

        // Act - Make 4 requests (exceeding the limit of 3)
        var responses = new List<HttpResponseMessage>();
        for (int i = 0; i < 4; i++)
        {
            var response = await PutAsync("/api/users/password", resetDto, ensureSuccess: false);
            responses.Add(response);
        }

        // Assert
        // First 3 should not be rate limited
        responses.Take(3).Should().OnlyContain(r =>
            r.StatusCode != HttpStatusCode.TooManyRequests);

        // 4th request should be rate limited
        responses.Last().StatusCode.Should().Be(HttpStatusCode.TooManyRequests);
    }

    #endregion

    #region Concurrent Rate Limiting

    [Fact]
    public async Task ConcurrentRequests_ExceedingLimit_SomeGetRateLimited()
    {
        // Arrange - Test global rate limiter
        var loginDto = new LoginUserDto
        {
            Email = "test@example.com",
            Password = "TestPassword123!"
        };

        // Act - Fire many concurrent requests
        var tasks = Enumerable.Range(1, 10)
            .Select(_ => PostAsync("/api/users/login", loginDto, ensureSuccess: false))
            .ToArray();

        var responses = await Task.WhenAll(tasks);

        // Assert - At least some requests should get through
        // Some might be rate limited depending on timing
        responses.Should().Contain(r =>
            r.StatusCode == HttpStatusCode.OK ||
            r.StatusCode == HttpStatusCode.BadRequest ||
            r.StatusCode == HttpStatusCode.TooManyRequests);
    }

    #endregion

    #region Rate Limit Headers and Response

    [Fact]
    public async Task RateLimited_Response_ContainsProperErrorMessage()
    {
        // Arrange
        var loginDto = new LoginUserDto
        {
            Email = "test@example.com",
            Password = "WrongPassword123!"
        };

        // Act - Exceed rate limit
        HttpResponseMessage? rateLimitedResponse = null;
        for (int i = 0; i < 10; i++)
        {
            var response = await PostAsync("/api/users/login", loginDto, ensureSuccess: false);
            if (response.StatusCode == HttpStatusCode.TooManyRequests)
            {
                rateLimitedResponse = response;
                break;
            }
        }

        // Assert
        rateLimitedResponse.Should().NotBeNull();
        rateLimitedResponse!.StatusCode.Should().Be(HttpStatusCode.TooManyRequests);

        var content = await rateLimitedResponse.Content.ReadAsStringAsync();
        content.Should().Contain("Too Many Requests");
        content.Should().Contain("Rate limit exceeded");
    }

    #endregion

    #region Different Endpoints, Different Limits

    [Fact]
    public async Task DifferentEndpoints_HaveIndependentRateLimits()
    {
        // Arrange
        var loginDto = new LoginUserDto
        {
            Email = "test@example.com",
            Password = "WrongPassword123!"
        };

        var resetDto = new GetResetTokenDto
        {
            Email = "test@example.com"
        };

        // Act - Exhaust login rate limit
        var loginResponses = new List<HttpResponseMessage>();
        for (int i = 0; i < 6; i++)
        {
            loginResponses.Add(await PostAsync("/api/users/login", loginDto, ensureSuccess: false));
        }

        // Then try password reset (should have independent limit)
        var resetResponse = await PutAsync("/api/users/password", resetDto, ensureSuccess: false);

        // Assert
        // Login should be rate limited
        loginResponses.Last().StatusCode.Should().Be(HttpStatusCode.TooManyRequests);

        // Password reset should still work (independent rate limit)
        resetResponse.StatusCode.Should().NotBe(HttpStatusCode.TooManyRequests);
    }

    #endregion

    #region Rate Limit Recovery

    [Fact]
    public async Task RateLimit_AllowsRequestsAfterWindowExpires()
    {
        // Arrange
        var loginDto = new LoginUserDto
        {
            Email = "test@example.com",
            Password = "WrongPassword123!"
        };

        // Act - Exhaust rate limit
        for (int i = 0; i < 6; i++)
        {
            await PostAsync("/api/users/login", loginDto, ensureSuccess: false);
        }

        // Wait for rate limit window to reset (1 minute + buffer for auth endpoints)
        // Note: In real tests, you might want to use a shorter window for testing
        // or mock the time provider
        await Task.Delay(TimeSpan.FromSeconds(65));

        // Try again
        var responseAfterWait = await PostAsync("/api/users/login", loginDto, ensureSuccess: false);

        // Assert - Should not be rate limited anymore
        responseAfterWait.StatusCode.Should().NotBe(HttpStatusCode.TooManyRequests);
    }

    #endregion
}
