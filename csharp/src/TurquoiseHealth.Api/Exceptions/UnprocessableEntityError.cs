namespace TurquoiseHealth.Api;

/// <summary>
/// This exception type will be thrown for any non-2XX API responses.
/// </summary>
[Serializable]
public class UnprocessableEntityError(object body)
    : TurquoiseHealthApiApiException("UnprocessableEntityError", 422, body);
