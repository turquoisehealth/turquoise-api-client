namespace TurquoiseHealth.Api;

/// <summary>
/// This exception type will be thrown for any non-2XX API responses.
/// </summary>
[Serializable]
public class TooManyRequestsError(object body)
    : TurquoisehealthApiApiException("TooManyRequestsError", 429, body);
