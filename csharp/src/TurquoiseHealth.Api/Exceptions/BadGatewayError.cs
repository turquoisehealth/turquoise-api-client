namespace TurquoiseHealth.Api;

/// <summary>
/// This exception type will be thrown for any non-2XX API responses.
/// </summary>
[Serializable]
public class BadGatewayError(ConsumerSitePersonalizedEstimateErrorResponse body)
    : TurquoiseHealthApiClientApiException("BadGatewayError", 502, body)
{
    /// <summary>
    /// The body of the response that triggered the exception.
    /// </summary>
    public new ConsumerSitePersonalizedEstimateErrorResponse Body => body;
}
