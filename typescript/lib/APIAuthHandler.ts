/**
 * APIAuthHandler - Custom authentication utilities for Turquoise Health API
 *
 * Manages API authentication tokens with automatic refresh and caching for expiring tokens.
 */

interface OAuthTokenResponse {
    access_token: string;
    expires_in?: number;
}

interface APIAuthHandlerOptions {
    token?: string;
    tokenProvider?: () => string | Promise<string>;
    clientId?: string;
    clientSecret?: string;
    organizationId?: string;
    authUrl?: string;
    ttlBuffer?: number;
}

/**
 * Helper class for managing API authentication tokens with automatic refresh.
 *
 * Supports multiple authentication methods:
 * 1. Static token (never expires)
 * 2. Environment variable token
 * 3. OAuth client credentials flow with automatic token refresh
 *
 * @example
 * ```typescript
 * import { TurquoiseHealthApiClient, lib } from "@turquoisehealth/api";
 *
 * // Static token from environment
 * const auth = lib.APIAuthHandler.fromEnv();
 * const client = new TurquoiseHealthApiClient({ token: auth.asSupplier() });
 *
 * // OAuth with client credentials (auto-refreshes when expired)
 * const auth = lib.APIAuthHandler.fromClientCredentials();
 * const client = new TurquoiseHealthApiClient({ token: auth.asSupplier() });
 *
 * // Custom token provider
 * const auth = new lib.APIAuthHandler({
 *     tokenProvider: async () => await getTokenFromVault()
 * });
 * const client = new TurquoiseHealthApiClient({ token: auth.asSupplier() });
 * ```
 */
export class APIAuthHandler {
    private readonly token?: string;
    private readonly tokenProvider?: () => string | Promise<string>;
    private readonly clientId?: string;
    private readonly clientSecret?: string;
    private readonly organizationId?: string;
    private readonly authUrl: string;
    private readonly ttlBuffer: number;

    // In-memory token cache
    private cachedToken?: string;
    private tokenExpiration?: number;

    constructor(options: APIAuthHandlerOptions = {}) {
        this.token = options.token;
        this.tokenProvider = options.tokenProvider;
        this.clientId = options.clientId;
        this.clientSecret = options.clientSecret;
        this.organizationId = options.organizationId;
        this.authUrl = options.authUrl || "https://api.turquoise.health";
        this.ttlBuffer = options.ttlBuffer || 60;
    }

    /**
     * Create an auth handler using a static token from environment variables.
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
     * Create an auth handler using OAuth client credentials flow.
     *
     * Credentials are read from parameters or environment variables:
     * - TURQUOISE_CLIENT_ID
     * - TURQUOISE_CLIENT_SECRET
     * - TURQUOISE_ORGANIZATION_ID
     * - TURQUOISE_AUTH_URL (optional, defaults to https://api.turquoise.health)
     *
     * @param options - Configuration options
     * @returns APIAuthHandler instance
     * @throws Error if required credentials are not provided
     */
    public static fromClientCredentials(options: {
        clientId?: string;
        clientSecret?: string;
        organizationId?: string;
        authUrl?: string;
        ttlBuffer?: number;
    } = {}): APIAuthHandler {
        const clientId = options.clientId || process.env.TURQUOISE_CLIENT_ID;
        const clientSecret = options.clientSecret || process.env.TURQUOISE_CLIENT_SECRET;
        const organizationId = options.organizationId || process.env.TURQUOISE_ORGANIZATION_ID;
        const authUrl = options.authUrl || process.env.TURQUOISE_AUTH_URL;

        if (!clientId || !clientSecret || !organizationId) {
            throw new Error(
                "Client credentials required. Provide clientId, clientSecret, and organizationId " +
                "or set TURQUOISE_CLIENT_ID, TURQUOISE_CLIENT_SECRET, and TURQUOISE_ORGANIZATION_ID " +
                "environment variables."
            );
        }

        return new APIAuthHandler({
            clientId,
            clientSecret,
            organizationId,
            authUrl,
            ttlBuffer: options.ttlBuffer,
        });
    }

    /**
     * Get the current authentication token.
     *
     * Resolution order:
     * 1. Static token if provided
     * 2. Custom token provider if provided
     * 3. Cached OAuth token if valid
     * 4. New OAuth token via client credentials
     *
     * @returns The API token string
     * @throws Error if no token source is configured
     */
    public async getToken(): Promise<string> {
        // Static token takes precedence
        if (this.token) {
            return this.token;
        }

        // Custom provider
        if (this.tokenProvider) {
            return await this.tokenProvider();
        }

        // OAuth client credentials with caching
        if (this.clientId && this.clientSecret && this.organizationId) {
            return await this.getOAuthToken();
        }

        throw new Error("No token source configured");
    }

    /**
     * Get OAuth token using client credentials, with caching.
     *
     * @returns Access token
     * @throws Error if token request fails
     */
    private async getOAuthToken(): Promise<string> {
        // Check cache first
        if (this.cachedToken && this.tokenExpiration) {
            const now = Date.now() / 1000;
            if (now < this.tokenExpiration - this.ttlBuffer) {
                return this.cachedToken;
            }
        }

        // Request new token
        const body = {
            client_id: this.clientId!,
            client_secret: this.clientSecret!,
            organization_id: this.organizationId!,
            audience: "https://api.turquoise.health",
            grant_type: "client_credentials",
        };

        const requestTime = Date.now() / 1000;

        try {
            const response = await fetch(`${this.authUrl}/oauth/token`, {
                method: "POST",
                headers: {
                    "Content-Type": "application/json",
                    Accept: "application/json",
                },
                body: JSON.stringify(body),
            });

            if (!response.ok) {
                const responseText = await response.text();
                throw new Error(
                    `Failed to retrieve access token: ${response.status} ${response.statusText}. ` +
                    `Response: ${responseText}`
                );
            }

            const data = (await response.json()) as OAuthTokenResponse;

            if (!data.access_token) {
                throw new Error("No access_token in OAuth response");
            }

            // Cache the token
            const expiresIn = data.expires_in || 3600;
            this.cachedToken = data.access_token;
            this.tokenExpiration = requestTime + expiresIn;

            return data.access_token;
        } catch (error) {
            if (error instanceof Error) {
                throw error;
            }
            throw new Error(`Failed to retrieve access token: ${error}`);
        }
    }

    /**
     * Return a supplier function that can be passed to the TurquoiseHealthApiClient.
     *
     * The returned function will automatically handle token refresh when using
     * OAuth client credentials.
     *
     * @returns Supplier function that returns the current token
     */
    public asSupplier(): () => Promise<string> {
        return () => this.getToken();
    }
}
