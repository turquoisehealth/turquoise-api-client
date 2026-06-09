using System.Text.Json;
using System.Text.Json.Serialization;
using TurquoiseHealth.Api.Core;

namespace TurquoiseHealth.Api;

[Serializable]
public record PriceDetail : IJsonOnDeserialized
{
    [JsonExtensionData]
    private readonly IDictionary<string, JsonElement> _extensionData =
        new Dictionary<string, JsonElement>();

    [JsonPropertyName("max_price")]
    public required double MaxPrice { get; set; }

    [JsonPropertyName("max_price_billing_code")]
    public required string MaxPriceBillingCode { get; set; }

    /// <summary>
    /// Signed 64-bit integer serialized as a string to preserve precision in JSON.
    /// </summary>
    [JsonPropertyName("network_id")]
    public required string NetworkId { get; set; }

    [JsonPropertyName("provider_id")]
    public required int ProviderId { get; set; }

    [JsonPropertyName("ssp_id")]
    public required string SspId { get; set; }

    [JsonPropertyName("line_items")]
    public IEnumerable<PackageLineItem> LineItems { get; set; } = new List<PackageLineItem>();

    [JsonIgnore]
    public ReadOnlyAdditionalProperties AdditionalProperties { get; private set; } = new();

    void IJsonOnDeserialized.OnDeserialized() =>
        AdditionalProperties.CopyFromExtensionData(_extensionData);

    /// <inheritdoc />
    public override string ToString()
    {
        return JsonUtils.Serialize(this);
    }
}
