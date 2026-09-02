using System.Text.Json;
using System.Text.Json.Serialization;
using TurquoiseHealth.Api.Core;

namespace TurquoiseHealth.Api;

/// <summary>
/// Bounded statistic bundle. No envelope per the ticket.
/// </summary>
[Serializable]
public record ConsumerSiteRateComparison : IJsonOnDeserialized
{
    [JsonExtensionData]
    private readonly IDictionary<string, JsonElement> _extensionData =
        new Dictionary<string, JsonElement>();

    /// <summary>
    /// Number of rate rows the statistics were computed over.
    /// </summary>
    [JsonPropertyName("count")]
    public required int Count { get; set; }

    [JsonPropertyName("currency")]
    public string? Currency { get; set; }

    [JsonPropertyName("min_amount")]
    public string? MinAmount { get; set; }

    [JsonPropertyName("min_amount_cents")]
    public int? MinAmountCents { get; set; }

    [JsonPropertyName("max_amount")]
    public string? MaxAmount { get; set; }

    [JsonPropertyName("max_amount_cents")]
    public int? MaxAmountCents { get; set; }

    [JsonPropertyName("avg_amount")]
    public string? AvgAmount { get; set; }

    [JsonPropertyName("avg_amount_cents")]
    public int? AvgAmountCents { get; set; }

    [JsonPropertyName("median_amount")]
    public string? MedianAmount { get; set; }

    [JsonPropertyName("median_amount_cents")]
    public int? MedianAmountCents { get; set; }

    [JsonPropertyName("q1_amount")]
    public string? Q1Amount { get; set; }

    [JsonPropertyName("q1_amount_cents")]
    public int? Q1AmountCents { get; set; }

    [JsonPropertyName("q3_amount")]
    public string? Q3Amount { get; set; }

    [JsonPropertyName("q3_amount_cents")]
    public int? Q3AmountCents { get; set; }

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
