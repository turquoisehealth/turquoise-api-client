/**
 * Example: Using custom libraries with the Turquoise Health C# SDK
 *
 * This demonstrates how to use manually-coded libraries alongside the generated SDK.
 *
 * Recommended Approach:
 * 1. Set OAuth credentials as environment variables
 * 2. Create APIAuthHandler once (e.g., as static field or singleton)
 * 3. Reuse the same handler across your application for automatic token refresh
 */

using System;
using System.Linq;
using TurquoiseHealth.Api;
using TurquoiseHealth.Api.Lib;

namespace TurquoiseHealth.Examples
{
    public class CustomLibrariesExamples
    {
        /// <summary>
        /// RECOMMENDED: OAuth client credentials with automatic token refresh.
        ///
        /// This is the recommended authentication method. The auth handler caches tokens
        /// in memory and automatically refreshes them before expiration.
        ///
        /// Important: Create the auth handler once and reuse it across your application
        /// to avoid unnecessary token requests.
        /// </summary>
        public static (TurquoiseHealthApiClient client, APIAuthHandler auth) ExampleOAuthClientCredentials()
        {
            // Set OAuth credentials in environment
            Environment.SetEnvironmentVariable("TURQUOISE_CLIENT_ID", "your-client-id");
            Environment.SetEnvironmentVariable("TURQUOISE_CLIENT_SECRET", "your-client-secret");
            Environment.SetEnvironmentVariable("TURQUOISE_ORGANIZATION_ID", "your-org-id");

            try
            {
                // Create auth handler once - reuse this instance!
                var auth = APIAuthHandler.FromClientCredentials();
                var client = new TurquoiseHealthApiClient(token: auth.GetToken());

                // Example: Make API calls
                try
                {
                    var ssps = client.ConsumerPricing.Ssps.ListAsync(new SspsListRequest { Query = "MRI Brain" }).GetAwaiter().GetResult();
                    Console.WriteLine($"✓ Found {ssps.Items.Count()} packages");

                    if (ssps.Items.Any())
                    {
                        var prices = client.ConsumerPricing.Prices.ListAsync(new PricesListRequest
                        {
                            SspId = ssps.Items.First().Id,
                            ZipCode = "90210"
                        }).GetAwaiter().GetResult();
                        Console.WriteLine($"✓ Found {prices.Items.Count()} prices");
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine($"  (API call skipped: {e.Message})");
                }

                Console.WriteLine("✓ Client initialized with OAuth auto-refresh");
                return (client, auth);
            }
            catch (InvalidOperationException e)
            {
                Console.WriteLine($"  (Skipped: {e.Message})");
                return (null, null);
            }
        }

        /// <summary>
        /// Alternative: Using a dynamic token provider for custom token sources.
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
        /// Alternative: Using a static token from environment variable (no auto-refresh).
        /// </summary>
        public static TurquoiseHealthApiClient ExampleBasicAuthHandler()
        {
            // Set your token in environment variable
            Environment.SetEnvironmentVariable("TURQUOISE_API_TOKEN", "your-api-token-here");

            // Use the auth handler
            var auth = APIAuthHandler.FromEnv();
            var client = new TurquoiseHealthApiClient(token: auth.GetToken());

            Console.WriteLine("✓ Client initialized with static env token");
            return client;
        }

        /// <summary>
        /// Alternative: Custom environment variable name.
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
            Console.WriteLine("Turquoise Health SDK - Custom Libraries Examples\n");
            Console.WriteLine("Note: Set real credentials in environment variables to run API calls\n");

            Console.WriteLine(new string('=', 60));
            Console.WriteLine("RECOMMENDED: OAuth Client Credentials (Auto-Refresh)");
            Console.WriteLine(new string('=', 60));
            var (client, auth) = ExampleOAuthClientCredentials();
            if (auth != null)
            {
                Console.WriteLine("  → Reuse the 'auth' instance across your application!");
            }

            Console.WriteLine("\n" + new string('=', 60));
            Console.WriteLine("Alternative: Dynamic Token Provider");
            Console.WriteLine(new string('=', 60));
            ExampleDynamicToken();

            Console.WriteLine("\n" + new string('=', 60));
            Console.WriteLine("Alternative: Static Token from Environment");
            Console.WriteLine(new string('=', 60));
            try
            {
                ExampleBasicAuthHandler();
            }
            catch (InvalidOperationException e)
            {
                Console.WriteLine($"  (Skipped: {e.Message})");
            }

            Console.WriteLine("\n" + new string('=', 60));
            Console.WriteLine("Alternative: Custom Environment Variable");
            Console.WriteLine(new string('=', 60));
            ExampleCustomEnvVar();

            Console.WriteLine("\n✨ All examples completed!");
        }
    }
}
