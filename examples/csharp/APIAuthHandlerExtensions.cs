using System.Net.Http.Headers;
using TurquoiseHealth.Api.Lib;

// Remove this compatibility patch when the example is upgraded to SDK 3.2.114.
internal static class APIAuthHandlerExtensions
{
    public static HttpMessageHandler CreateRefreshTokenHandler(
        this APIAuthHandler auth,
        HttpMessageHandler? innerHandler = null)
    {
        return new RefreshTokenHandler(auth, innerHandler ?? new HttpClientHandler());
    }

    private sealed class RefreshTokenHandler(APIAuthHandler auth, HttpMessageHandler innerHandler)
        : DelegatingHandler(innerHandler)
    {
        protected override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", auth.GetToken());
            return base.SendAsync(request, cancellationToken);
        }
    }
}
