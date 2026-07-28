using TurquoiseHealth.Api.Core;

namespace TurquoiseHealth.Api;

public partial class TurquoiseHealthApiClient
{
    private readonly RawClient _client;

    public TurquoiseHealthApiClient(string? token = null, ClientOptions? clientOptions = null)
    {
        var defaultHeaders = new Headers(
            new Dictionary<string, string>()
            {
                { "Authorization", $"Bearer {token ?? ""}" },
                { "X-Fern-Language", "C#" },
                { "X-Fern-SDK-Name", "TurquoiseHealth.Api" },
                { "X-Fern-SDK-Version", Version.Current },
            }
        );
        clientOptions ??= new ClientOptions();
        foreach (var header in defaultHeaders)
        {
            if (!clientOptions.Headers.ContainsKey(header.Key))
            {
                clientOptions.Headers[header.Key] = header.Value;
            }
        }
        _client = new RawClient(clientOptions);
        ConsumerPricing = new ConsumerPricingClient(_client);
    }

    public ConsumerPricingClient ConsumerPricing { get; }
}
