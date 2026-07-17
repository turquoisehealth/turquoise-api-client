// Manual/custom libraries for TurquoiseHealth.Api C# SDK
// This directory is for hand-written code that will be bundled with the generated SDK

using System;

namespace TurquoiseHealth.Api.Lib;

/// <summary>
/// Helper class for managing API authentication tokens.
/// </summary>
/// <example>
/// <code>
/// using TurquoiseHealth.Api;
/// using TurquoiseHealth.Api.Lib;
///
/// // Get token from environment
/// var auth = APIAuthHandler.FromEnv();
/// var client = new TurquoiseHealthApiClient(token: auth.GetToken());
///
/// // Or use a custom token provider
/// var auth = new APIAuthHandler(tokenProvider: () => GetTokenFromVault());
/// var client = new TurquoiseHealthApiClient(token: auth.GetToken());
/// </code>
/// </example>
public class APIAuthHandler
{
    private readonly string? _token;
    private readonly Func<string>? _tokenProvider;

    /// <summary>
    /// Initialize the auth handler.
    /// </summary>
    /// <param name="token">Static API token</param>
    /// <param name="tokenProvider">Function that returns a token (for dynamic tokens)</param>
    public APIAuthHandler(string? token = null, Func<string>? tokenProvider = null)
    {
        _token = token;
        _tokenProvider = tokenProvider;
    }

    /// <summary>
    /// Create an auth handler using a token from environment variables.
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
    /// Get the current authentication token.
    /// </summary>
    /// <returns>The API token string</returns>
    /// <exception cref="InvalidOperationException">If no token is available</exception>
    public string GetToken()
    {
        if (_tokenProvider != null)
        {
            return _tokenProvider();
        }
        if (!string.IsNullOrEmpty(_token))
        {
            return _token;
        }
        throw new InvalidOperationException("No token or token provider configured");
    }
}
