using System.Text.Json.Serialization;
using TurquoiseHealth.Api.Core;

namespace TurquoiseHealth.Api;

[Serializable]
public record V3PricesCompareRequest
{
    [JsonPropertyName("package_id")]
    public required string PackageId { get; set; }

    [JsonPropertyName("provider_id")]
    public string? ProviderId { get; set; }

    [JsonPropertyName("pricing")]
    public required V3PricesCompareRequestPricing Pricing { get; set; }

    [JsonPropertyName("location")]
    public V3Location? Location { get; set; }

    /// <inheritdoc />
    public override string ToString()
    {
        return JsonUtils.Serialize(this);
    }
}
