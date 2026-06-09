namespace TurquoiseHealth.Api.Core;

public interface IIsRetryableContent
{
    public bool IsRetryable { get; }
}
