using System.Text.Json.Serialization;

namespace Tutor.Infrastructure.Models;

public class GoogleUserInfoResponse
{
    [JsonPropertyName("sub")]
    public string Sub { get; set; } = string.Empty; // Unique identifier for the user

    [JsonPropertyName("email")] public string Email { get; set; } = string.Empty;
}