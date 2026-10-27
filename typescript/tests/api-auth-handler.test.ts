import { APIAuthHandler } from "../lib";

const credentials = {
  clientId: "test-client",
  clientSecret: "test-secret",
  organizationId: "test-org",
};

describe("APIAuthHandler (offline)", () => {
  const originalEnv = process.env;
  let fetchMock: jest.SpiedFunction<typeof fetch>;
  let clock: jest.SpiedFunction<typeof Date.now>;

  beforeEach(() => {
    process.env = { ...originalEnv };
    for (const key of [
      "TURQUOISE_API_TOKEN",
      "TURQUOISE_CLIENT_ID",
      "TURQUOISE_CLIENT_SECRET",
      "TURQUOISE_ORGANIZATION_ID",
      "TURQUOISE_AUTH_URL",
      "TEST_API_TOKEN",
    ]) {
      delete process.env[key];
    }
    clock = jest.spyOn(Date, "now").mockReturnValue(1_000_000);
    fetchMock = jest.spyOn(globalThis, "fetch").mockImplementation(async () =>
      new Response(JSON.stringify({ access_token: "oauth-token", expires_in: 100 })),
    );
  });

  afterEach(() => {
    process.env = originalEnv;
    jest.restoreAllMocks();
  });

  test("static token takes precedence over provider and OAuth", async () => {
    const tokenProvider = jest.fn(() => "provider-token");
    const auth = new APIAuthHandler({ ...credentials, token: "static-token", tokenProvider });
    expect(await auth.getToken()).toBe("static-token");
    expect(await auth.asSupplier()()).toBe("static-token");
    expect(tokenProvider).not.toHaveBeenCalled();
    expect(fetchMock).not.toHaveBeenCalled();
  });

  test("supplier reevaluates a synchronous provider before OAuth", async () => {
    const tokenProvider = jest.fn().mockReturnValueOnce("first").mockReturnValueOnce("second");
    const supplier = new APIAuthHandler({ ...credentials, tokenProvider }).asSupplier();
    expect(await supplier()).toBe("first");
    expect(await supplier()).toBe("second");
    expect(tokenProvider).toHaveBeenCalledTimes(2);
    expect(fetchMock).not.toHaveBeenCalled();
  });

  test("supports asynchronous token providers", async () => {
    const auth = new APIAuthHandler({ tokenProvider: async () => "async-token" });
    expect(await auth.getToken()).toBe("async-token");
  });

  test("propagates provider errors", async () => {
    const error = new Error("vault unavailable");
    const auth = new APIAuthHandler({ tokenProvider: () => { throw error; } });
    await expect(auth.getToken()).rejects.toBe(error);
  });

  test("default constructor reports an unconfigured token source", async () => {
    await expect(new APIAuthHandler().getToken()).rejects.toThrow("No token source configured");
  });

  test.each(["TURQUOISE_API_TOKEN", "TEST_API_TOKEN"])("reads token from %s", async (envVar) => {
    process.env[envVar] = "environment-token";
    const auth = envVar === "TURQUOISE_API_TOKEN"
      ? APIAuthHandler.fromEnv()
      : APIAuthHandler.fromEnv(envVar);
    expect(await auth.getToken()).toBe("environment-token");
  });

  test.each([undefined, ""])("rejects missing environment token (%s)", (value) => {
    if (value !== undefined) process.env.TURQUOISE_API_TOKEN = value;
    expect(() => APIAuthHandler.fromEnv()).toThrow("Environment variable 'TURQUOISE_API_TOKEN' is not set");
  });

  test.each(["clientId", "clientSecret", "organizationId"] as const)(
    "rejects missing %s",
    (missing) => {
      expect(() => APIAuthHandler.fromClientCredentials({ ...credentials, [missing]: undefined }))
        .toThrow("Client credentials required");
    },
  );

  test("rejects absent credentials", () => {
    expect(() => APIAuthHandler.fromClientCredentials()).toThrow("Client credentials required");
  });

  test.each([{}, { clientId: "only-client" }, { clientId: "client", clientSecret: "secret" }])(
    "rejects an incomplete token source (%o)",
    async (options) => {
      await expect(new APIAuthHandler(options).getToken()).rejects.toThrow("No token source configured");
    },
  );

  test("reads credentials and auth URL from environment", async () => {
    process.env.TURQUOISE_CLIENT_ID = "test-client";
    process.env.TURQUOISE_CLIENT_SECRET = "test-secret";
    process.env.TURQUOISE_ORGANIZATION_ID = "test-org";
    process.env.TURQUOISE_AUTH_URL = "https://auth.example";
    expect(await APIAuthHandler.fromClientCredentials().getToken()).toBe("oauth-token");
    expect(fetchMock).toHaveBeenCalledWith("https://auth.example/oauth/token", expect.any(Object));
    const body = JSON.parse(String(fetchMock.mock.calls[0][1]?.body));
    expect(body.client_id).toBe("test-client");
    expect(body.client_secret).toBe("test-secret");
    expect(body.organization_id).toBe("test-org");
  });

  test("explicit credentials override environment and serialize the OAuth request", async () => {
    process.env.TURQUOISE_CLIENT_ID = "ignored-client";
    process.env.TURQUOISE_CLIENT_SECRET = "ignored-secret";
    process.env.TURQUOISE_ORGANIZATION_ID = "ignored-org";
    process.env.TURQUOISE_AUTH_URL = "https://ignored.example";
    const auth = APIAuthHandler.fromClientCredentials({ ...credentials, authUrl: "https://auth.example" });
    expect(await auth.getToken()).toBe("oauth-token");
    expect(fetchMock).toHaveBeenCalledWith("https://auth.example/oauth/token", {
      method: "POST",
      headers: { "Content-Type": "application/json", Accept: "application/json" },
      body: JSON.stringify({
        client_id: "test-client",
        client_secret: "test-secret",
        organization_id: "test-org",
        audience: "https://api.turquoise.health",
        grant_type: "client_credentials",
      }),
    });
  });

  test("defaults to the production auth URL", async () => {
    await APIAuthHandler.fromClientCredentials(credentials).getToken();
    expect(fetchMock.mock.calls[0][0]).toBe("https://api.turquoise.health/oauth/token");
  });

  test("caches tokens and refreshes exactly at the buffer boundary through the supplier", async () => {
    const supplier = APIAuthHandler.fromClientCredentials(credentials).asSupplier();
    expect(await supplier()).toBe("oauth-token");
    clock.mockReturnValue(1_039_000);
    expect(await supplier()).toBe("oauth-token");
    expect(fetchMock).toHaveBeenCalledTimes(1);
    clock.mockReturnValue(1_040_000);
    fetchMock.mockResolvedValueOnce(new Response('{"access_token":"refreshed"}'));
    expect(await supplier()).toBe("refreshed");
    expect(fetchMock).toHaveBeenCalledTimes(2);
  });

  test("supports a zero buffer until actual expiration", async () => {
    const auth = APIAuthHandler.fromClientCredentials({ ...credentials, ttlBuffer: 0 });
    await auth.getToken();
    clock.mockReturnValue(1_099_000);
    expect(await auth.getToken()).toBe("oauth-token");
    expect(fetchMock).toHaveBeenCalledTimes(1);
    clock.mockReturnValue(1_100_000);
    await auth.getToken();
    expect(fetchMock).toHaveBeenCalledTimes(2);
  });

  test("defaults missing expiry to one hour", async () => {
    fetchMock.mockImplementation(async () => new Response('{"access_token":"token"}'));
    const auth = APIAuthHandler.fromClientCredentials(credentials);
    await auth.getToken();
    clock.mockReturnValue(4_539_000);
    expect(await auth.getToken()).toBe("token");
    expect(fetchMock).toHaveBeenCalledTimes(1);
    clock.mockReturnValue(4_540_000);
    await auth.getToken();
    expect(fetchMock).toHaveBeenCalledTimes(2);
  });

  test("does not cache a token with zero lifetime", async () => {
    fetchMock.mockImplementation(async () => new Response('{"access_token":"token","expires_in":0}'));
    const auth = APIAuthHandler.fromClientCredentials(credentials);
    await auth.getToken();
    await auth.getToken();
    expect(fetchMock).toHaveBeenCalledTimes(2);
  });

  test("measures expiration before network delay", async () => {
    fetchMock.mockImplementation(async () => {
      clock.mockReturnValue(1_050_000);
      return new Response('{"access_token":"token","expires_in":100}');
    });
    const auth = APIAuthHandler.fromClientCredentials(credentials);
    await auth.getToken();
    await auth.getToken();
    expect(fetchMock).toHaveBeenCalledTimes(2);
  });

  test("reports HTTP errors and allows recovery without caching the failure", async () => {
    fetchMock.mockResolvedValueOnce(new Response('{"error":"invalid_client"}', {
      status: 401, statusText: "Unauthorized",
    }));
    const auth = APIAuthHandler.fromClientCredentials(credentials);
    await expect(auth.getToken()).rejects.toThrow("401 Unauthorized. Response: {\"error\":\"invalid_client\"}");
    expect(await auth.getToken()).toBe("oauth-token");
    expect(fetchMock).toHaveBeenCalledTimes(2);
  });

  test.each(["{}", '{"access_token":""}', '{"access_token":null}'])(
    "rejects missing access token (%s)",
    async (body) => {
      fetchMock.mockResolvedValueOnce(new Response(body));
      await expect(APIAuthHandler.fromClientCredentials(credentials).getToken())
        .rejects.toThrow("No access_token in OAuth response");
    },
  );

  test("propagates invalid JSON", async () => {
    fetchMock.mockResolvedValueOnce(new Response("not-json"));
    await expect(APIAuthHandler.fromClientCredentials(credentials).getToken()).rejects.toThrow(/JSON|Unexpected/);
  });

  test("propagates network errors", async () => {
    const error = new Error("connection refused");
    fetchMock.mockRejectedValueOnce(error);
    await expect(APIAuthHandler.fromClientCredentials(credentials).getToken()).rejects.toBe(error);
  });

  test("wraps non-Error failures", async () => {
    fetchMock.mockRejectedValueOnce("connection refused");
    await expect(APIAuthHandler.fromClientCredentials(credentials).getToken())
      .rejects.toThrow("Failed to retrieve access token: connection refused");
  });
});
