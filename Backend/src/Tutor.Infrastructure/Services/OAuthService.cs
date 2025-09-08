using Microsoft.Extensions.Configuration;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Tutor.Application.Features.Users.Dtos;
using Tutor.Application.Interfaces;
using Tutor.Domain.Entities;
using Tutor.Infrastructure.Models;

namespace Tutor.Infrastructure.Services;

public class OAuthService : IOAuthService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IConfiguration _configuration;

    public OAuthService(IHttpClientFactory httpClientFactory, IConfiguration configuration)
    {
        _httpClientFactory = httpClientFactory;
        _configuration = configuration;
    }

    public async Task<OAuthUserInfo> ValidateGoogleTokenAsync(string accessToken)
    {
        var client = _httpClientFactory.CreateClient();
        client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);

        var response = await client.GetAsync("https://www.googleapis.com/oauth2/v3/userinfo");
        try
        {
            response.EnsureSuccessStatusCode();
        }
        catch (HttpRequestException ex)
        {
            var errorMessage = $"OAuth validation failed: Google token validation returned status code {(int)response.StatusCode} ({response.ReasonPhrase}).";
            throw new InvalidOperationException(errorMessage, ex);
        }

        var content = await response.Content.ReadFromJsonAsync<GoogleUserInfoResponse>();
        
        return new OAuthUserInfo
        {
            ProviderId = content.Sub,
            Email = content.Email,
            Provider = "google"
        };
    }
    

    public string GenerateOAuthState()
    {
        return Guid.NewGuid().ToString("N");
    }

    public bool ValidateOAuthState(string state, string storedState)
    {
        return state == storedState;
    }
}