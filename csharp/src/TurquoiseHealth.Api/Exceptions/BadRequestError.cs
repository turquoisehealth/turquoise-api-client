namespace TurquoiseHealth.Api;

/// <summary>
/// This exception type will be thrown for any non-2XX API responses.
/// </summary>
[Serializable]
public class BadRequestError(object body)
    : TurquoiseHealthApiApiException("BadRequestError", 400, body);
