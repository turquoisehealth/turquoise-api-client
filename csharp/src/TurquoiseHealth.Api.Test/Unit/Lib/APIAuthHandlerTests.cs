using System.Net;
using System.Net.Http.Headers;
using NUnit.Framework;
using TurquoiseHealth.Api.Lib;

namespace TurquoiseHealth.Api.Test.Unit.Lib;

[TestFixture]
public class APIAuthHandlerTests
{
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
