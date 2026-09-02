using System.Text.Json;
using System.Text.Json.Serialization;
using TurquoiseHealth.Api.Core;

namespace TurquoiseHealth.Api;

/// <summary>
/// PriceComparison
/// </summary>
[Serializable]
public record ConsumerSitePriceComparison : IJsonOnDeserialized
{
    [JsonExtensionData]
    private readonly IDictionary<string, JsonElement> _extensionData =
        new Dictionary<string, JsonElement>();

    [JsonPropertyName("object")]
    public string? Object { get; set; }

    /// <summary>
    /// Number of prices the statistics were computed over.
    /// </summary>
    [JsonPropertyName("count")]
    public required int Count { get; set; }

    [JsonPropertyName("stats")]
    public ConsumerSiteComparisonStats? Stats { get; set; }

    [JsonPropertyName("disclosures")]
    public IEnumerable<string>? Disclosures { get; set; }

    [JsonPropertyName("meta")]
    public ConsumerSiteResponseMeta? Meta { get; set; }

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
