using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;
using NUnit.Framework;
using TurquoiseHealth.Api.Lib;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;

namespace TurquoiseHealth.Api.Test.Unit.Lib;

[TestFixture]
[NonParallelizable]
public class APIAuthHandlerTests
{
    private readonly Dictionary<string, string?> _environment = new();
    private WireMockServer _server = null!;

    [SetUp]
    public void SetUp()
    {
        foreach (var name in new[]
        {
            "TURQUOISE_API_TOKEN",
            "TURQUOISE_CLIENT_ID",
            "TURQUOISE_CLIENT_SECRET",
            "TURQUOISE_ORGANIZATION_ID",
            "TURQUOISE_AUTH_URL",
            "TEST_API_TOKEN"
        })
        {
            _environment[name] = Environment.GetEnvironmentVariable(name);
            Environment.SetEnvironmentVariable(name, null);
        }
        _server = WireMockServer.Start();
        RespondWith("""{"access_token":"oauth-token","expires_in":3600}""");
    }

    [TearDown]
    public void TearDown()
    {
        _server.Dispose();
        foreach (var entry in _environment)
        {
            Environment.SetEnvironmentVariable(entry.Key, entry.Value);
        }
    }

    [Test]
    public void StaticToken_TakesPrecedenceOverProviderAndOAuth()
    {
        var providerCalls = 0;
        var auth = new APIAuthHandler(
            token: "static-token",
            tokenProvider: () => { providerCalls++; return "provider-token"; },
            clientId: "test-client",
            clientSecret: "test-secret",
            organizationId: "test-org",
            authUrl: _server.Url);

        Assert.That(auth.GetToken(), Is.EqualTo("static-token"));
        Assert.That(auth.GetToken(), Is.EqualTo("static-token"));
        Assert.That(providerCalls, Is.Zero);
        Assert.That(_server.LogEntries, Is.Empty);
    }

    [Test]
    public void Provider_IsEvaluatedEveryTimeBeforeOAuth()
    {
        var calls = 0;
        var auth = new APIAuthHandler(
            tokenProvider: () => $"provider-{++calls}",
            clientId: "test-client",
            clientSecret: "test-secret",
            organizationId: "test-org",
            authUrl: _server.Url);
        Assert.That(auth.GetToken(), Is.EqualTo("provider-1"));
        Assert.That(auth.GetToken(), Is.EqualTo("provider-2"));
        Assert.That(_server.LogEntries, Is.Empty);
    }

    [Test]
    public void ProviderErrors_Propagate()
    {
        var error = new InvalidOperationException("vault unavailable");
        var auth = new APIAuthHandler(tokenProvider: () => throw error);
        Assert.That(Assert.Throws<InvalidOperationException>(() => auth.GetToken()), Is.SameAs(error));
    }

    [TestCase("TURQUOISE_API_TOKEN")]
    [TestCase("TEST_API_TOKEN")]
    public void FromEnv_ReadsStaticToken(string envVar)
    {
        Environment.SetEnvironmentVariable(envVar, "environment-token");
        var auth = envVar == "TURQUOISE_API_TOKEN"
            ? APIAuthHandler.FromEnv()
            : APIAuthHandler.FromEnv(envVar);
        Assert.That(auth.GetToken(), Is.EqualTo("environment-token"));
    }

    [Test]
    public void FromEnv_RejectsMissingToken()
    {
        Assert.That(
            Assert.Throws<InvalidOperationException>(() => APIAuthHandler.FromEnv())!.Message,
            Does.Contain("TURQUOISE_API_TOKEN").And.Contain("not set"));
    }

    [TestCase(null, "secret", "org")]
    [TestCase("client", null, "org")]
    [TestCase("client", "secret", null)]
    [TestCase("", "secret", "org")]
    [TestCase("client", "", "org")]
    [TestCase("client", "secret", "")]
    public void FromClientCredentials_RejectsIncompleteCredentials(
        string? clientId, string? clientSecret, string? organizationId)
    {
        Assert.That(
            Assert.Throws<InvalidOperationException>(() =>
                APIAuthHandler.FromClientCredentials(clientId, clientSecret, organizationId))!.Message,
            Does.Contain("Client credentials required"));
    }

    [TestCase(null, null, null)]
    [TestCase("client", null, null)]
    [TestCase("client", "secret", null)]
    public void GetToken_RejectsIncompleteSources(
        string? clientId, string? clientSecret, string? organizationId)
    {
        var auth = new APIAuthHandler(
            clientId: clientId, clientSecret: clientSecret, organizationId: organizationId);
        Assert.That(
            Assert.Throws<InvalidOperationException>(() => auth.GetToken())!.Message,
            Is.EqualTo("No token source configured"));
    }

    [TestCase(null)]
    [TestCase(" ")]
    public void FromClientCredentials_ReadsEnvironment(string? authUrl)
    {
        Environment.SetEnvironmentVariable("TURQUOISE_CLIENT_ID", "test-client");
        Environment.SetEnvironmentVariable("TURQUOISE_CLIENT_SECRET", "test-secret");
        Environment.SetEnvironmentVariable("TURQUOISE_ORGANIZATION_ID", "test-org");
        Environment.SetEnvironmentVariable("TURQUOISE_AUTH_URL", _server.Url);
        var auth = APIAuthHandler.FromClientCredentials(authUrl: authUrl);
        Assert.That(auth.GetToken(), Is.EqualTo("oauth-token"));
        AssertOAuthRequest();
    }

    [Test]
    public void ExplicitCredentials_OverrideEnvironment()
    {
        Environment.SetEnvironmentVariable("TURQUOISE_CLIENT_ID", "ignored-client");
        Environment.SetEnvironmentVariable("TURQUOISE_CLIENT_SECRET", "ignored-secret");
        Environment.SetEnvironmentVariable("TURQUOISE_ORGANIZATION_ID", "ignored-org");
        Environment.SetEnvironmentVariable("TURQUOISE_AUTH_URL", "http://ignored.invalid");
        Assert.That(CreateAuth().GetToken(), Is.EqualTo("oauth-token"));
        AssertOAuthRequest();
    }

    [Test]
    public void OAuthToken_IsCached()
    {
        var auth = CreateAuth();
        Assert.That(auth.GetToken(), Is.EqualTo("oauth-token"));
        Assert.That(auth.GetToken(), Is.EqualTo("oauth-token"));
        Assert.That(_server.LogEntries.Count(), Is.EqualTo(1));
    }

    [Test]
    public void MissingExpiry_DefaultsToOneHour()
    {
        RespondWith("""{"access_token":"token"}""");
        var auth = CreateAuth();
        Assert.That(auth.GetToken(), Is.EqualTo("token"));
        Assert.That(auth.GetToken(), Is.EqualTo("token"));
        Assert.That(_server.LogEntries.Count(), Is.EqualTo(1));
    }

    [Test]
    public void TokenAtRefreshBuffer_IsRefetched()
    {
        RespondWith("""{"access_token":"token","expires_in":60}""");
        var auth = CreateAuth();
        Assert.That(auth.GetToken(), Is.EqualTo("token"));
        RespondWith("""{"access_token":"refreshed","expires_in":3600}""");
        Assert.That(auth.GetToken(), Is.EqualTo("refreshed"));
        Assert.That(_server.LogEntries.Count(), Is.EqualTo(2));
    }

    [Test]
    public void ZeroBuffer_DoesNotRefreshEarly()
    {
        RespondWith("""{"access_token":"token","expires_in":60}""");
        var auth = CreateAuth(ttlBuffer: 0);
        Assert.That(auth.GetToken(), Is.EqualTo("token"));
        Assert.That(auth.GetToken(), Is.EqualTo("token"));
        Assert.That(_server.LogEntries.Count(), Is.EqualTo(1));
    }

    [Test]
    public void ZeroExpiry_IsNotCached()
    {
        RespondWith("""{"access_token":"token","expires_in":0}""");
        var auth = CreateAuth();
        auth.GetToken();
        auth.GetToken();
        Assert.That(_server.LogEntries.Count(), Is.EqualTo(2));
    }

    [Test]
    public void ConcurrentRequests_ShareTheCachedToken()
    {
        var auth = CreateAuth();
        Parallel.For(0, 8, _ => Assert.That(auth.GetToken(), Is.EqualTo("oauth-token")));
        Assert.That(_server.LogEntries.Count(), Is.EqualTo(1));
    }

    [Test]
    public void HttpFailure_IsReportedAndNotCached()
    {
        RespondWith("""{"error":"invalid_client"}""", 401);
        var auth = CreateAuth();
        var error = Assert.Throws<InvalidOperationException>(() => auth.GetToken());
        Assert.That(error!.Message, Does.Contain("Unauthorized").And.Contain("invalid_client"));
        RespondWith("""{"access_token":"recovered"}""");
        Assert.That(auth.GetToken(), Is.EqualTo("recovered"));
        Assert.That(_server.LogEntries.Count(), Is.EqualTo(2));
    }

    [TestCase("{}")]
    [TestCase("""{"access_token":""}""")]
    [TestCase("""{"access_token":null}""")]
    [TestCase("null")]
    public void MissingToken_IsReported(string body)
    {
        RespondWith(body);
        Assert.That(
            Assert.Throws<InvalidOperationException>(() => CreateAuth().GetToken())!.Message,
            Does.Contain("No access_token"));
    }

    [Test]
    public void InvalidJson_IsWrappedWithOriginalCause()
    {
        RespondWith("not-json");
        var error = Assert.Throws<InvalidOperationException>(() => CreateAuth().GetToken());
        Assert.That(error!.Message, Does.Contain("Failed to retrieve access token"));
        Assert.That(error.InnerException, Is.InstanceOf<JsonException>());
    }

    [Test]
    public void InvalidAuthUrl_IsReported()
    {
        var auth = new APIAuthHandler(
            clientId: "client", clientSecret: "secret", organizationId: "org", authUrl: "not-a-url");
        var error = Assert.Throws<InvalidOperationException>(() => auth.GetToken());
        Assert.That(error!.Message, Does.Contain("invalid request URI").IgnoreCase);
    }

    [Test]
    public async Task CreateRefreshTokenHandler_UsesCurrentTokenForEachRequest()
    {
        var tokenNumber = 0;
        var auth = new APIAuthHandler(tokenProvider: () => $"token-{++tokenNumber}");
        var recordingHandler = new RecordingHandler();
        using var httpClient = new HttpClient(auth.CreateRefreshTokenHandler(recordingHandler));

        using var firstResponse = await httpClient.GetAsync("https://example.com/first");
        using var secondResponse = await httpClient.GetAsync("https://example.com/second");

        Assert.Multiple(() =>
        {
            Assert.That(firstResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(secondResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(recordingHandler.AuthorizationHeaders, Is.EqualTo(new[]
            {
                new AuthenticationHeaderValue("Bearer", "token-1"),
                new AuthenticationHeaderValue("Bearer", "token-2")
            }));
        });
    }

    [Test]
    public async Task RefreshHandler_ReplacesExistingAuthorizationAndPreservesRequest()
    {
        var auth = new APIAuthHandler(token: "current-token");
        var recordingHandler = new RecordingHandler();
        using var httpClient = new HttpClient(auth.CreateRefreshTokenHandler(recordingHandler));
        using var request = new HttpRequestMessage(HttpMethod.Post, "https://example.com/query")
        {
            Content = new StringContent("request-body")
        };
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", "stale-token");
        request.Headers.Add("X-Custom", "preserved");
        using var response = await httpClient.SendAsync(request);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        Assert.That(recordingHandler.AuthorizationHeaders.Single()!.Parameter, Is.EqualTo("current-token"));
        Assert.That(request.Method, Is.EqualTo(HttpMethod.Post));
        Assert.That(await request.Content.ReadAsStringAsync(), Is.EqualTo("request-body"));
        Assert.That(request.Headers.GetValues("X-Custom").Single(), Is.EqualTo("preserved"));
    }

    [Test]
    public void RefreshHandler_PropagatesTokenFailureWithoutSending()
    {
        var auth = new APIAuthHandler(tokenProvider: () => throw new InvalidOperationException("vault unavailable"));
        var recordingHandler = new RecordingHandler();
        using var httpClient = new HttpClient(auth.CreateRefreshTokenHandler(recordingHandler));
        Assert.That(
            Assert.ThrowsAsync<InvalidOperationException>(async () =>
                await httpClient.GetAsync("https://example.com"))!.Message,
            Is.EqualTo("vault unavailable"));
        Assert.That(recordingHandler.AuthorizationHeaders, Is.Empty);
    }

    [Test]
    public async Task DefaultRefreshHandler_SendsToLocalServer()
    {
        _server.Given(Request.Create().WithPath("/resource").UsingGet())
            .RespondWith(Response.Create().WithStatusCode(202));
        var auth = new APIAuthHandler(token: "static-token");
        using var client = new HttpClient(auth.CreateRefreshTokenHandler());
        using var response = await client.GetAsync($"{_server.Url}/resource");
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Accepted));
        Assert.That(
            _server.LogEntries.Single().RequestMessage.Headers!["Authorization"].ToString(),
            Does.Contain("Bearer static-token"));
    }

    private APIAuthHandler CreateAuth(int ttlBuffer = 60) =>
        APIAuthHandler.FromClientCredentials(
            clientId: "test-client",
            clientSecret: "test-secret",
            organizationId: "test-org",
            authUrl: _server.Url,
            ttlBuffer: ttlBuffer);

    private void RespondWith(string body, int statusCode = 200) =>
        _server.Given(Request.Create().WithPath("/oauth/token").UsingPost())
            .RespondWith(Response.Create().WithStatusCode(statusCode)
                .WithHeader("Content-Type", "application/json").WithBody(body));

    private void AssertOAuthRequest()
    {
        var request = _server.LogEntries.Single().RequestMessage;
        Assert.That(request.Path, Is.EqualTo("/oauth/token"));
        Assert.That(request.Method, Is.EqualTo("POST"));
        Assert.That(request.Headers!["Content-Type"].ToString(), Does.Contain("application/json"));
        using var json = JsonDocument.Parse(request.Body!);
        var body = json.RootElement;
        Assert.That(body.GetProperty("client_id").GetString(), Is.EqualTo("test-client"));
        Assert.That(body.GetProperty("client_secret").GetString(), Is.EqualTo("test-secret"));
        Assert.That(body.GetProperty("organization_id").GetString(), Is.EqualTo("test-org"));
        Assert.That(body.GetProperty("grant_type").GetString(), Is.EqualTo("client_credentials"));
        Assert.That(body.GetProperty("audience").GetString(), Is.EqualTo("https://api.turquoise.health"));
    }

    private sealed class RecordingHandler : HttpMessageHandler
    {
        public List<AuthenticationHeaderValue?> AuthorizationHeaders { get; } = [];

        protected override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            AuthorizationHeaders.Add(request.Headers.Authorization);
            return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK));
        }
    }
}
