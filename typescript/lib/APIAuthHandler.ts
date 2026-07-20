/**
 * APIAuthHandler - Custom authentication utilities for Turquoise Health API
 *
 * This is an example of a manually-coded library that extends the generated SDK.
 */

/**
 * Helper class for managing API authentication tokens.
 *
 * @example
 * ```typescript
 * import { TurquoiseHealthApiClient } from "@turquoise-health/api";
 * import { APIAuthHandler } from "@turquoise-health/api/lib";
 *
 * // Get token from environment
 * const auth = APIAuthHandler.fromEnv();
 * const client = new TurquoiseHealthApiClient({ token: auth.getToken() });
 *
 * // Or use a custom token provider
 * const auth = new APIAuthHandler({ tokenProvider: () => getTokenFromVault() });
 * const client = new TurquoiseHealthApiClient({ token: auth.asSupplier() });
 * ```
 */
export class APIAuthHandler {
    private readonly token?: string;
    private readonly tokenProvider?: () => string | Promise<string>;

    constructor(options: {
        token?: string;
        tokenProvider?: () => string | Promise<string>;
    }) {
        this.token = options.token;
        this.tokenProvider = options.tokenProvider;
    }

    /**
     * Create an auth handler using a token from environment variables.
     *
     * @param envVar - Name of the environment variable containing the token
     * @returns APIAuthHandler instance
     * @throws Error if the environment variable is not set
     */
    public static fromEnv(envVar: string = "TURQUOISE_API_TOKEN"): APIAuthHandler {
        const token = process.env[envVar];
        if (!token) {
            throw new Error(`Environment variable '${envVar}' is not set`);
        }
        return new APIAuthHandler({ token });
    }

    /**
     * Get the current authentication token.
     *
     * @returns The API token string
     * @throws Error if no token is available
     */
    public async getToken(): Promise<string> {
        if (this.tokenProvider) {
            return await this.tokenProvider();
        }
        if (this.token) {
            return this.token;
        }
        throw new Error("No token or token provider configured");
    }

    /**
     * Return a supplier function that can be passed to the TurquoiseHealthApiClient.
     *
     * @returns Supplier function that returns the token
     */
    public asSupplier(): () => string | Promise<string> {
        return () => this.getToken();
    }
}
