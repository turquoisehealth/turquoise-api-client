/**
 * Example: Using custom libraries with the Turquoise Health TypeScript SDK
 *
 * This demonstrates how to use manually-coded libraries alongside the generated SDK.
 */

import { TurquoiseHealthApiClient, lib } from "@turquoise-health/api";

/**
 * Example: Using APIAuthHandler for basic authentication
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
    console.log("Turquoise Health SDK - Custom Libraries Examples\n");

    console.log("Example 1: Basic Auth Handler");
    try {
        exampleBasicAuthHandler();
    } catch (error) {
        console.log(`  (Skipped: ${error})`);
    }

    console.log("\nExample 2: Dynamic Token Provider");
    exampleDynamicToken();

    console.log("\nExample 3: Async Token Provider");
    await exampleAsyncToken();

    console.log("\nExample 4: Static Token");
    exampleStaticToken();

    console.log("\n✨ All examples completed!");
}

// Run if executed directly
if (require.main === module) {
    main().catch(console.error);
}

export {
    exampleBasicAuthHandler,
    exampleDynamicToken,
    exampleAsyncToken,
    exampleStaticToken
};
