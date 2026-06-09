using System.Text.Json;
using System.Text.Json.Serialization;
using TurquoiseHealth.Api.Core;

namespace TurquoiseHealth.Api;

[Serializable]
public record SspPricePage : IJsonOnDeserialized
{
    [JsonExtensionData]
    private readonly IDictionary<string, JsonElement> _extensionData =
        new Dictionary<string, JsonElement>();

    [JsonPropertyName("results")]
    public IEnumerable<SspPrice> Results { get; set; } = new List<SspPrice>();

    [JsonPropertyName("page_size")]
    public int? PageSize { get; set; }

    [JsonPropertyName("count")]
    public required int Count { get; set; }

    /// <summary>
    /// Set when results are empty. `no_data` if no matching records exist; `permission_denied` if matching records exist but are excluded by your data permissions.
    /// </summary>
    [JsonPropertyName("no_data_reason")]
    public NoDataReason? NoDataReason { get; set; }

    /// <summary>
    /// Minimum price of total results, not just ones on the current page
    /// </summary>
    [JsonPropertyName("min_price")]
    public double? MinPrice { get; set; }

    /// <summary>
    /// Maximum price of total results, not just ones on the current page
    /// </summary>
    [JsonPropertyName("max_price")]
    public double? MaxPrice { get; set; }

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
