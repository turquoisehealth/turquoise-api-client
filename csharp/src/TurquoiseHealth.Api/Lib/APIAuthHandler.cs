// Manual/custom libraries for TurquoiseHealth.Api C# SDK
// This directory is for hand-written code that will be bundled with the generated SDK

using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace TurquoiseHealth.Api.Lib;

/// <summary>
/// Helper class for managing API authentication tokens with automatic refresh.
/// </summary>
/// <remarks>
/// Supports multiple authentication methods:
/// <list type="number">
/// <item>Static token (never expires)</item>
/// <item>Environment variable token</item>
/// <item>OAuth client credentials flow with automatic token refresh</item>
/// </list>
/// </remarks>
/// <example>
/// <code>
/// using TurquoiseHealth.Api;
/// using TurquoiseHealth.Api.Lib;
///
/// // Static token from environment
/// var auth = APIAuthHandler.FromEnv();
/// var client = new TurquoiseHealthApiClient(token: auth.GetToken());
///
/// // OAuth with client credentials (auto-refreshes when expired)
/// var auth = APIAuthHandler.FromClientCredentials();
/// var client = new TurquoiseHealthApiClient(token: auth.GetToken());
///
/// // Custom token provider
/// var auth = new APIAuthHandler(tokenProvider: () => GetTokenFromVault());
/// var client = new TurquoiseHealthApiClient(token: auth.GetToken());
/// </code>
/// </example>
public class APIAuthHandler
{
    private readonly string? _token;
    private readonly Func<string>? _tokenProvider;
    private readonly string? _clientId;
    private readonly string? _clientSecret;
    private readonly string? _organizationId;
    private readonly string _authUrl;
    private readonly int _ttlBuffer;
    private readonly HttpClient _httpClient;

    // In-memory token cache
    private string? _cachedToken;
    private DateTime? _tokenExpiration;
    private readonly object _cacheLock = new object();

    /// <summary>
    /// Initialize the auth handler.
    /// </summary>
    /// <param name="token">Static API token</param>
    /// <param name="tokenProvider">Function that returns a token (for dynamic tokens)</param>
    /// <param name="clientId">OAuth client ID for client credentials flow</param>
    /// <param name="clientSecret">OAuth client secret</param>
    /// <param name="organizationId">Organization ID for OAuth</param>
    /// <param name="authUrl">Authentication server URL (defaults to https://api.turquoise.health)</param>
    /// <param name="ttlBuffer">Seconds before expiration to refresh token (default: 60)</param>
    public APIAuthHandler(
        string? token = null,
        Func<string>? tokenProvider = null,
        string? clientId = null,
        string? clientSecret = null,
        string? organizationId = null,
        string? authUrl = null,
        int ttlBuffer = 60)
    {
        _token = token;
        _tokenProvider = tokenProvider;
        _clientId = clientId;
        _clientSecret = clientSecret;
        _organizationId = organizationId;
        _authUrl = authUrl ?? "https://api.turquoise.health";
        _ttlBuffer = ttlBuffer;
        _httpClient = new HttpClient();
    }

    /// <summary>
    /// Create an auth handler using a static token from environment variables.
    /// </summary>
    /// <param name="envVar">Name of the environment variable containing the token</param>
    /// <returns>APIAuthHandler instance</returns>
    /// <exception cref="InvalidOperationException">If the environment variable is not set</exception>
    public static APIAuthHandler FromEnv(string envVar = "TURQUOISE_API_TOKEN")
    {
        var token = Environment.GetEnvironmentVariable(envVar);
        if (string.IsNullOrEmpty(token))
        {
            throw new InvalidOperationException($"Environment variable '{envVar}' is not set");
        }
        return new APIAuthHandler(token: token);
    }

    /// <summary>
    /// Create an auth handler using OAuth client credentials flow.
    /// </summary>
    /// <remarks>
    /// Credentials are read from parameters or environment variables:
    /// <list type="bullet">
    /// <item>TURQUOISE_CLIENT_ID</item>
    /// <item>TURQUOISE_CLIENT_SECRET</item>
    /// <item>TURQUOISE_ORGANIZATION_ID</item>
    /// <item>TURQUOISE_AUTH_URL (optional, defaults to https://api.turquoise.health)</item>
    /// </list>
    /// </remarks>
    /// <param name="clientId">OAuth client ID (or from TURQUOISE_CLIENT_ID)</param>
    /// <param name="clientSecret">OAuth client secret (or from TURQUOISE_CLIENT_SECRET)</param>
    /// <param name="organizationId">Organization ID (or from TURQUOISE_ORGANIZATION_ID)</param>
    /// <param name="authUrl">Auth server URL (or from TURQUOISE_AUTH_URL)</param>
    /// <param name="ttlBuffer">Seconds before expiration to refresh token</param>
    /// <returns>APIAuthHandler instance</returns>
    /// <exception cref="InvalidOperationException">If required credentials are not provided</exception>
    public static APIAuthHandler FromClientCredentials(
        string? clientId = null,
        string? clientSecret = null,
        string? organizationId = null,
        string? authUrl = null,
        int ttlBuffer = 60)
    {
        clientId ??= Environment.GetEnvironmentVariable("TURQUOISE_CLIENT_ID");
        clientSecret ??= Environment.GetEnvironmentVariable("TURQUOISE_CLIENT_SECRET");
        organizationId ??= Environment.GetEnvironmentVariable("TURQUOISE_ORGANIZATION_ID");
        authUrl ??= Environment.GetEnvironmentVariable("TURQUOISE_AUTH_URL");

        if (string.IsNullOrEmpty(clientId) || string.IsNullOrEmpty(clientSecret) || string.IsNullOrEmpty(organizationId))
        {
            throw new InvalidOperationException(
                "Client credentials required. Provide clientId, clientSecret, and organizationId " +
                "or set TURQUOISE_CLIENT_ID, TURQUOISE_CLIENT_SECRET, and TURQUOISE_ORGANIZATION_ID " +
                "environment variables.");
        }

        return new APIAuthHandler(
            clientId: clientId,
            clientSecret: clientSecret,
            organizationId: organizationId,
            authUrl: authUrl,
            ttlBuffer: ttlBuffer);
    }

    /// <summary>
    /// Get the current authentication token.
    /// </summary>
    /// <remarks>
    /// Resolution order:
    /// <list type="number">
    /// <item>Static token if provided</item>
    /// <item>Custom token provider if provided</item>
    /// <item>Cached OAuth token if valid</item>
    /// <item>New OAuth token via client credentials</item>
    /// </list>
    /// </remarks>
    /// <returns>The API token string</returns>
    /// <exception cref="InvalidOperationException">If no token source is configured</exception>
    public string GetToken()
    {
        // Static token takes precedence
        if (!string.IsNullOrEmpty(_token))
        {
            return _token!;
        }

        // Custom provider
        if (_tokenProvider != null)
        {
            return _tokenProvider();
        }

        // OAuth client credentials with caching
        if (!string.IsNullOrEmpty(_clientId) && !string.IsNullOrEmpty(_clientSecret) && !string.IsNullOrEmpty(_organizationId))
        {
            return GetOAuthToken();
        }

        throw new InvalidOperationException("No token source configured");
    }

    /// <summary>
    /// Get OAuth token using client credentials, with caching.
    /// </summary>
    /// <returns>Access token</returns>
    /// <exception cref="InvalidOperationException">If token request fails</exception>
    private string GetOAuthToken()
    {
        lock (_cacheLock)
        {
            // Check cache first
            if (_cachedToken != null && _tokenExpiration.HasValue)
            {
                if (DateTime.UtcNow < _tokenExpiration.Value.AddSeconds(-_ttlBuffer))
                {
                    return _cachedToken;
                }
            }

            // Request new token
            var body = new
            {
                client_id = _clientId,
                client_secret = _clientSecret,
                organization_id = _organizationId,
                audience = "https://api.turquoise.health",
                grant_type = "client_credentials"
            };

            var requestTime = DateTime.UtcNow;

            try
            {
                var content = new StringContent(
                    JsonSerializer.Serialize(body),
                    Encoding.UTF8,
                    "application/json");

                var response = _httpClient.PostAsync($"{_authUrl}/oauth/token", content).GetAwaiter().GetResult();

                if (!response.IsSuccessStatusCode)
                {
                    var responseText = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                    throw new InvalidOperationException(
                        $"Failed to retrieve access token: {response.StatusCode} {response.ReasonPhrase}. " +
                        $"Response: {responseText}");
                }

                var responseJson = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                var data = JsonSerializer.Deserialize<OAuthTokenResponse>(responseJson);

                if (data?.AccessToken == null)
                {
                    throw new InvalidOperationException("No access_token in OAuth response");
                }

                // Cache the token
                var expiresIn = data.ExpiresIn ?? 3600;
                _cachedToken = data.AccessToken;
                _tokenExpiration = requestTime.AddSeconds(expiresIn);

                return data.AccessToken;
            }
            catch (Exception ex) when (ex is not InvalidOperationException)
            {
                throw new InvalidOperationException($"Failed to retrieve access token: {ex.Message}", ex);
            }
        }
    }

    private class OAuthTokenResponse
    {
        [JsonPropertyName("access_token")]
        public string? AccessToken { get; set; }

        [JsonPropertyName("expires_in")]
        public int? ExpiresIn { get; set; }
    }
}
