using System.Net.Http.Headers;

namespace TurquoiseHealth.Api.Lib;

internal sealed class RefreshTokenHandler(APIAuthHandler auth, HttpMessageHandler innerHandler)
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
