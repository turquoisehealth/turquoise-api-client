namespace TurquoiseHealth.Api;

/// <summary>
/// This exception type will be thrown for any non-2XX API responses.
/// </summary>
[Serializable]
public class NotFoundError(V3ErrorResponse body)
    : TurquoiseHealthApiClientApiException("NotFoundError", 404, body)
{
    /// <summary>
    /// The body of the response that triggered the exception.
    /// </summary>
    public new V3ErrorResponse Body => body;
}
