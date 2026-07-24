/**
 * Example: Using the APIAuthHandler for Authentication
 *
 * This demonstrates various authentication patterns using the APIAuthHandler class,
 * including static tokens, dynamic token providers, async token retrieval, and
 * OAuth client credentials with automatic token refresh.
 *
 * Note: For development, compile the TypeScript SDK first:
 *   cd ../typescript && npm install && npm run build
 * Or install the published package:
 *   npm install @turquoisehealth/api
 */

import { TurquoiseHealthApiClient, lib } from "@turquoisehealth/api";

/**
 * Example: Using APIAuthHandler with a token from environment variable
 */
function exampleBasicAuthHandler() {
    // Set your token in environment variable
    process.env.TURQUOISE_API_TOKEN = "your-api-token-here";

    // Use the auth handler
    const auth = lib.APIAuthHandler.fromEnv();
    const client = new TurquoiseHealthApiClient({
        token: auth.asSupplier()
    });

    console.log("✓ Client initialized with APIAuthHandler");
    return client;
}

/**
 * Example: Using a dynamic token provider
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
 * Example: Using async token provider
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
 * Example: OAuth client credentials with automatic token refresh
 */
async function exampleOAuthClientCredentials() {
    // Set OAuth credentials in environment
    process.env.TURQUOISE_CLIENT_ID = "your-client-id";
    process.env.TURQUOISE_CLIENT_SECRET = "your-client-secret";
    process.env.TURQUOISE_ORGANIZATION_ID = "your-org-id";

    try {
        // Create auth handler with OAuth credentials
        // Token will be automatically refreshed when it expires
        const auth = lib.APIAuthHandler.fromClientCredentials();
        const client = new TurquoiseHealthApiClient({
            token: auth.asSupplier() // Returns a function that auto-refreshes
        });

        console.log("✓ Client initialized with OAuth auto-refresh");
        return client;
    } catch (error) {
        console.log(`  (Skipped: ${error})`);
        return null;
    }
}

/**
 * Example: Static token
 */
function exampleStaticToken() {
    const auth = new lib.APIAuthHandler({
        token: "your-static-token"
    });

    const client = new TurquoiseHealthApiClient({
        token: auth.asSupplier()
    });

    console.log("✓ Client initialized with static token");
    return client;
}

// Run examples
async function main() {
    console.log("Turquoise Health SDK - APIAuthHandler Examples\n");

    console.log("Example 1: Environment Token");
    try {
        exampleBasicAuthHandler();
    } catch (error) {
        console.log(`  (Skipped: ${error})`);
    }

    console.log("\nExample 2: Dynamic Token Provider");
    exampleDynamicToken();

    console.log("\nExample 3: Async Token Provider");
    await exampleAsyncToken();

    console.log("\nExample 4: OAuth Client Credentials (Auto-Refresh)");
    await exampleOAuthClientCredentials();

    console.log("\nExample 5: Static Token");
    exampleStaticToken();

    console.log("\n✨ All authentication examples completed!");
}

// Run if executed directly
if (require.main === module) {
    main().catch(console.error);
}

export {
    exampleBasicAuthHandler,
    exampleDynamicToken,
    exampleAsyncToken,
    exampleOAuthClientCredentials,
