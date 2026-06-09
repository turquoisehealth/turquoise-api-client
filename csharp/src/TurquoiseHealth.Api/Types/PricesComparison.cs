using System.Text.Json;
using System.Text.Json.Serialization;
using TurquoiseHealth.Api.Core;

namespace TurquoiseHealth.Api;

[Serializable]
public record PricesComparison : IJsonOnDeserialized
{
    [JsonExtensionData]
    private readonly IDictionary<string, JsonElement> _extensionData =
        new Dictionary<string, JsonElement>();

    /// <summary>
    /// Lowest package price in the area
    /// </summary>
    [JsonPropertyName("min_price")]
    public required double MinPrice { get; set; }

    /// <summary>
    /// Highest package price in the area
    /// </summary>
    [JsonPropertyName("max_price")]
    public required double MaxPrice { get; set; }

    /// <summary>
    /// Average package price in the area
    /// </summary>
    [JsonPropertyName("avg_price")]
    public required double AvgPrice { get; set; }

    /// <summary>
    /// 25th percentile package price
    /// </summary>
    [JsonPropertyName("q1_price")]
    public required double Q1Price { get; set; }

    /// <summary>
    /// Median (50th percentile) package price
    /// </summary>
    [JsonPropertyName("median_price")]
    public required double MedianPrice { get; set; }

    /// <summary>
    /// 75th percentile package price
    /// </summary>
    [JsonPropertyName("q3_price")]
    public required double Q3Price { get; set; }

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
