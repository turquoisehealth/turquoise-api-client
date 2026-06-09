using System.Text.Json;
using System.Text.Json.Serialization;
using TurquoiseHealth.Api.Core;

namespace TurquoiseHealth.Api;

[Serializable]
public record RateBreakdown : IJsonOnDeserialized
{
    [JsonExtensionData]
    private readonly IDictionary<string, JsonElement> _extensionData =
        new Dictionary<string, JsonElement>();

    [JsonPropertyName("rate_type")]
    public required RateBreakdownRateType RateType { get; set; }

    [JsonPropertyName("provider_id")]
    public required string ProviderId { get; set; }

    [JsonPropertyName("ssp_id")]
    public required string SspId { get; set; }

    /// <summary>
    /// Signed 64-bit integer serialized as a string to preserve precision in JSON.
    /// </summary>
    [JsonPropertyName("network_id")]
    public string? NetworkId { get; set; }

    /// <summary>
    /// SSP-total amount from /prices.
    /// </summary>
    [JsonPropertyName("total_amount")]
    public required string TotalAmount { get; set; }

    [JsonPropertyName("total_amount_cents")]
    public required int TotalAmountCents { get; set; }

    [JsonPropertyName("currency")]
    public string? Currency { get; set; }

    [JsonPropertyName("line_items")]
    public IEnumerable<RateBreakdownLineItem> LineItems { get; set; } =
        new List<RateBreakdownLineItem>();

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
