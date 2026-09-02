using System.Text.Json;
using System.Text.Json.Serialization;
using TurquoiseHealth.Api.Core;

namespace TurquoiseHealth.Api;

/// <summary>
/// ComparisonStats
/// </summary>
[Serializable]
public record ConsumerSiteComparisonStats : IJsonOnDeserialized
{
    [JsonExtensionData]
    private readonly IDictionary<string, JsonElement> _extensionData =
        new Dictionary<string, JsonElement>();

    [JsonPropertyName("min")]
    public required ConsumerSiteMoney Min { get; set; }

    [JsonPropertyName("max")]
    public required ConsumerSiteMoney Max { get; set; }

    [JsonPropertyName("avg")]
    public required ConsumerSiteMoney Avg { get; set; }

    [JsonPropertyName("median")]
    public required ConsumerSiteMoney Median { get; set; }

    [JsonPropertyName("q1")]
    public required ConsumerSiteMoney Q1 { get; set; }

    [JsonPropertyName("q3")]
    public required ConsumerSiteMoney Q3 { get; set; }

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
