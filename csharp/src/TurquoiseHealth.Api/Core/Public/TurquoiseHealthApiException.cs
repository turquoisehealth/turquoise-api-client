namespace TurquoiseHealth.Api;

/// <summary>
/// Base exception class for all exceptions thrown by the SDK.
/// </summary>
public class TurquoiseHealthApiException(string message, Exception? innerException = null)
    : Exception(message, innerException);
