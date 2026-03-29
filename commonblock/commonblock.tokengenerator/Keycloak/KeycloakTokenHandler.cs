using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Configuration;

namespace commonblock.tokengenerator.Keycloak;

public class KeycloakTokenHandler : IKeycloakToken
{
    
    private readonly IConfiguration _configuration;
    private readonly HttpClient _httpClient;

    public KeycloakTokenHandler(
        IConfiguration configuration,
        HttpClient httpClient)
    {
        _configuration = configuration;
        _httpClient = httpClient;
    }
    public async Task<string> GetAccessTokenAsync()
    {
        // If no user context (e.g., background service), request new token
        return await GetClientCredentialsTokenAsync();
    }

    private async Task<string> GetClientCredentialsTokenAsync()
    {
        var tokenEndpoint = $"{_configuration["Keycloak:Authority"]}/protocol/openid-connect/token";

        var clientId = _configuration["Keycloak:ClientId"] ?? throw new InvalidOperationException("Keycloak:ClientId is not configured.");
        var clientSecret = _configuration["Keycloak:ClientSecret"] ?? throw new InvalidOperationException("Keycloak:ClientSecret is not configured.");

        var request = new HttpRequestMessage(HttpMethod.Post, tokenEndpoint);
        request.Content = new FormUrlEncodedContent(new Dictionary<string, string>
        {
            ["grant_type"] = "client_credentials",
            ["client_id"] = clientId,
            ["client_secret"] = clientSecret,
        });

        var response = await _httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();
        var tokenResponse = JsonSerializer.Deserialize<KeycloakTokenResponse>(content);

        if (tokenResponse == null || string.IsNullOrEmpty(tokenResponse.AccessToken))
        {
            throw new InvalidOperationException("Failed to retrieve access token from Keycloak response.");
        }

        return tokenResponse.AccessToken;
    }

}

public class KeycloakTokenResponse
{
    [JsonPropertyName("access_token")]
    public string AccessToken { get; set; } = default!;

    [JsonPropertyName("expires_in")]
    public int ExpiresIn { get; set; } = default!;

    [JsonPropertyName("refresh_token")]
    public string RefreshToken { get; set; } = default!;

    [JsonPropertyName("token_type")]
    public string TokenType { get; set; } = default!;
}
