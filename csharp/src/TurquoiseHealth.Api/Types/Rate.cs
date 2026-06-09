using System.Text.Json;
using System.Text.Json.Serialization;
using TurquoiseHealth.Api.Core;

namespace TurquoiseHealth.Api;

/// <summary>
/// SSP-total rate row at the (provider × ssp × [network]) grain.
/// </summary>
[Serializable]
public record Rate : IJsonOnDeserialized
{
    [JsonExtensionData]
    private readonly IDictionary<string, JsonElement> _extensionData =
        new Dictionary<string, JsonElement>();

    /// <summary>
    /// `cash` ⇔ `network_id is null`. `negotiated` otherwise.
    /// </summary>
    [JsonPropertyName("rate_type")]
    public required RateRateType RateType { get; set; }

    /// <summary>
    /// Provider identifier. Upstream-derived from the dataset and may change as the dataset is rebuilt. Do not bake into URLs, bookmarks, or persistent storage.
    /// </summary>
    [JsonPropertyName("provider_id")]
    public required string ProviderId { get; set; }

    /// <summary>
    /// SSP identifier. Upstream-derived from the dataset and may change as the dataset is rebuilt. Do not bake into URLs, bookmarks, or persistent storage.
    /// </summary>
    [JsonPropertyName("ssp_id")]
    public required string SspId { get; set; }

    /// <summary>
    /// Network identifier; null for cash rates. Int64String — JSON string-wrapped 64-bit integer, because JSON cannot safely represent values above 2^53. Upstream-derived from the dataset and may change as the dataset is rebuilt. Do not bake into URLs, bookmarks, or persistent storage.
    /// </summary>
    [JsonPropertyName("network_id")]
    public string? NetworkId { get; set; }

    /// <summary>
    /// String-decimal money value, e.g. "1250.00".
    /// </summary>
    [JsonPropertyName("amount")]
    public required string Amount { get; set; }

    /// <summary>
    /// Integer minor units, e.g. 125000.
    /// </summary>
    [JsonPropertyName("amount_cents")]
    public required int AmountCents { get; set; }

    /// <summary>
    /// ISO currency code. Always USD today.
    /// </summary>
    [JsonPropertyName("currency")]
    public string? Currency { get; set; }

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
