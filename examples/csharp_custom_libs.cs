/**
 * Example: Using custom libraries with the Turquoise Health C# SDK
 *
 * This demonstrates how to use manually-coded libraries alongside the generated SDK.
 */

using System;
using TurquoiseHealth.Api;
using TurquoiseHealth.Api.Lib;

namespace TurquoiseHealth.Examples
{
    public class CustomLibrariesExamples
    {
        /// <summary>
        /// Example: Using APIAuthHandler for basic authentication
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
            Console.WriteLine("Turquoise Health SDK - Custom Libraries Examples\n");

            Console.WriteLine("Example 1: Basic Auth Handler");
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

            Console.WriteLine("\nExample 4: Custom Environment Variable");
            ExampleCustomEnvVar();

            Console.WriteLine("\n✨ All examples completed!");
        }
    }
}
