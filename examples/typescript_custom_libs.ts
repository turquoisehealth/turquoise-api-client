/**
 * Example: Using custom libraries with the Turquoise Health TypeScript SDK
 *
 * This demonstrates how to use manually-coded libraries alongside the generated SDK.
 *
 * Recommended Approach:
 * 1. Set OAuth credentials as environment variables
 * 2. Create APIAuthHandler once (e.g., at module level or as singleton)
 * 3. Reuse the same handler across your application for automatic token refresh
 */

import { TurquoiseHealthApiClient, lib } from "@turquoisehealth/api";

/**
 * RECOMMENDED: OAuth client credentials with automatic token refresh
 *
 * This is the recommended authentication method. The auth handler caches tokens
 * in memory and automatically refreshes them before expiration.
 *
 * Important: Create the auth handler once and reuse it across your application
 * to avoid unnecessary token requests.
 */
async function exampleOAuthClientCredentials() {
    // Set OAuth credentials in environment
    process.env.TURQUOISE_CLIENT_ID = "your-client-id";
    process.env.TURQUOISE_CLIENT_SECRET = "your-client-secret";
    process.env.TURQUOISE_ORGANIZATION_ID = "your-org-id";

    try {
        // Create auth handler once - reuse this instance!
        const auth = lib.APIAuthHandler.fromClientCredentials();

        // Use asSupplier() to enable automatic token refresh
        const client = new TurquoiseHealthApiClient({
            token: auth.asSupplier()
        });

        // Example: Make API calls
        try {
            const ssps = await client.consumerPricing.ssps.list({ query: "MRI Brain" });
            console.log(`✓ Found ${ssps.items.length} packages`);

            if (ssps.items.length > 0) {
                const prices = await client.consumerPricing.prices.list({
                    sspId: ssps.items[0].id,
                    zipCode: "90210"
                });
                console.log(`✓ Found ${prices.items.length} prices`);
            }
        } catch (e) {
            console.log(`  (API call skipped: ${e})`);
        }

        console.log("✓ Client initialized with OAuth auto-refresh");
        return { client, auth };
    } catch (error) {
        console.log(`  (Skipped: ${error})`);
        return null;
    }
}

/**
 * Alternative: Using a dynamic token provider for custom token sources
 */
function exampleDynamicToken() {
    function getTokenFromVault(): string {
        // In production, this might fetch from a secrets manager
        return process.env.TURQUOISE_API_TOKEN || "default-token";
    }

    // Create auth handler with dynamic token provider
    const auth = new lib.APIAuthHandler({
        tokenProvider: getTokenFromVault
    });

    // Use with the client
    const client = new TurquoiseHealthApiClient({
        token: auth.asSupplier()
    });

    console.log("✓ Client initialized with dynamic token provider");
    return client;
}

/**
 * Alternative: Using async token provider
 */
async function exampleAsyncToken() {
    async function getTokenFromVaultAsync(): Promise<string> {
        // Simulate async token retrieval
        return new Promise((resolve) => {
            setTimeout(() => {
                resolve(process.env.TURQUOISE_API_TOKEN || "default-token");
            }, 100);
        });
    }

    const auth = new lib.APIAuthHandler({
        tokenProvider: getTokenFromVaultAsync
    });

    // Get the token
    const token = await auth.getToken();
    const client = new TurquoiseHealthApiClient({ token });

    console.log("✓ Client initialized with async token provider");
    return client;
}

/**
 * Alternative: Using a static token from environment variable (no auto-refresh)
 */
function exampleBasicAuthHandler() {
    // Set your token in environment variable
    process.env.TURQUOISE_API_TOKEN = "your-api-token-here";

    // Use the auth handler
    const auth = lib.APIAuthHandler.fromEnv();
    const client = new TurquoiseHealthApiClient({
        token: auth.asSupplier()
    });

    console.log("✓ Client initialized with static env token");
    return client;
}

// Run examples
async function main() {
    console.log("Turquoise Health SDK - Custom Libraries Examples\n");
    console.log("Note: Set real credentials in environment variables to run API calls\n");

    console.log("=".repeat(60));
    console.log("RECOMMENDED: OAuth Client Credentials (Auto-Refresh)");
    console.log("=".repeat(60));
    const result = await exampleOAuthClientCredentials();
    if (result) {
        console.log("  → Reuse the 'auth' instance across your application!");
    }

    console.log("\n" + "=".repeat(60));
    console.log("Alternative: Dynamic Token Provider");
    console.log("=".repeat(60));
    exampleDynamicToken();

    console.log("\n" + "=".repeat(60));
    console.log("Alternative: Async Token Provider");
    console.log("=".repeat(60));
    await exampleAsyncToken();

    console.log("\n" + "=".repeat(60));
    console.log("Alternative: Static Token from Environment");
    console.log("=".repeat(60));
    try {
        exampleBasicAuthHandler();
    } catch (error) {
        console.log(`  (Skipped: ${error})`);
    }

    consoleOAuthClientCredentials,
    exampleDynamicToken,
    exampleAsyncToken,
    exampleBasicAuthHandler
if (require.main === module) {
    main().catch(console.error);
}

export {
    exampleBasicAuthHandler,
    exampleDynamicToken,
    exampleAsyncToken,
    exampleOAuthClientCredentials,
