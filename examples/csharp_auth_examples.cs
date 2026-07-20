/**
 * Example: Using the APIAuthHandler for Authentication
 *
 * This demonstrates various authentication patterns using the APIAuthHandler class,
 * including static tokens, dynamic token providers, OAuth client credentials with
 * automatic token refresh, and custom environment variables.
 *
 * Note: For development, build the C# SDK first:
 *   cd ../csharp && dotnet build
 * Or reference the published NuGet package.
 */

using System;
using TurquoiseHealth.Api;
using TurquoiseHealth.Api.Lib;

namespace TurquoiseHealth.Examples
{
    public class AuthHandlerExamples
    {
        /// <summary>
        /// Example: Using APIAuthHandler with a token from environment variable
        /// </summary>
        public static TurquoiseHealthApiClient ExampleBasicAuthHandler()
        {
            // Set your token in environment variable
            Environment.SetEnvironmentVariable("TURQUOISE_API_TOKEN", "your-api-token-here");

            // Use the auth handler
            var auth = APIAuthHandler.FromEnv();
            var client = new TurquoiseHealthApiClient(token: auth.GetToken());

            Console.WriteLine("✓ Client initialized with APIAuthHandler");
            return client;
        }

        /// <summary>
        /// Example: Using a dynamic token provider
        /// </summary>
        public static TurquoiseHealthApiClient ExampleDynamicToken()
        {
            string GetTokenFromVault()
            {
                // In production, this might fetch from a secrets manager
                return Environment.GetEnvironmentVariable("TURQUOISE_API_TOKEN") ?? "default-token";
            }

            // Create auth handler with dynamic token provider
            var auth = new APIAuthHandler(tokenProvider: GetTokenFromVault);
            var client = new TurquoiseHealthApiClient(token: auth.GetToken());

            Console.WriteLine("✓ Client initialized with dynamic token provider");
            return client;
        }

        /// <summary>
        /// Example: Static token
        /// </summary>
        public static TurquoiseHealthApiClient ExampleStaticToken()
        {
            var auth = new APIAuthHandler(token: "your-static-token");
            var client = new TurquoiseHealthApiClient(token: auth.GetToken());

            Console.WriteLine("✓ Client initialized with static token");
            return client;
        }

        /// <summary>
        /// Example: OAuth client credentials with automatic token refresh
        /// </summary>
        public static TurquoiseHealthApiClient ExampleOAuthClientCredentials()
        {
            // Set OAuth credentials in environment
            Environment.SetEnvironmentVariable("TURQUOISE_CLIENT_ID", "your-client-id");
            Environment.SetEnvironmentVariable("TURQUOISE_CLIENT_SECRET", "your-client-secret");
            Environment.SetEnvironmentVariable("TURQUOISE_ORGANIZATION_ID", "your-org-id");

            try
            {
                // Create auth handler with OAuth credentials
                // Token will be automatically refreshed when it expires
                var auth = APIAuthHandler.FromClientCredentials();
                var client = new TurquoiseHealthApiClient(token: auth.GetToken());

                Console.WriteLine("✓ Client initialized with OAuth auto-refresh");
                return client;
            }
            catch (InvalidOperationException e)
            {
                Console.WriteLine($"  (Skipped: {e.Message})");
                return null;
            }
        }

        /// <summary>
        /// Example: Custom environment variable name
        /// </summary>
        public static TurquoiseHealthApiClient ExampleCustomEnvVar()
        {
            Environment.SetEnvironmentVariable("MY_CUSTOM_TOKEN_VAR", "my-token");

            var auth = APIAuthHandler.FromEnv(envVar: "MY_CUSTOM_TOKEN_VAR");
            var client = new TurquoiseHealthApiClient(token: auth.GetToken());

            Console.WriteLine("✓ Client initialized with custom environment variable");
            return client;
        }

        public static void Main(string[] args)
        {
            Console.WriteLine("Turquoise Health SDK - APIAuthHandler Examples\n");

            Console.WriteLine("Example 1: Environment Token");
            try
            {
                ExampleBasicAuthHandler();
            }
            catch (InvalidOperationException e)
            {
                Console.WriteLine($"  (Skipped: {e.Message})");
            }

            Console.WriteLine("\nExample 2: Dynamic Token Provider");
            ExampleDynamicToken();

            Console.WriteLine("\nExample 3: Static Token");
            ExampleStaticToken();

            Console.WriteLine("\nExample 4: OAuth Client Credentials (Auto-Refresh)");
            ExampleOAuthClientCredentials();

            Console.WriteLine("\nExample 5: Custom Environment Variable");
            ExampleCustomEnvVar();

            Console.WriteLine("\n✨ All authentication examples completed!");
        }
    }
}
