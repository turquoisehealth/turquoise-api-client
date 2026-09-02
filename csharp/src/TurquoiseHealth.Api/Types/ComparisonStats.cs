using System.Text.Json;
using System.Text.Json.Serialization;
using TurquoiseHealth.Api.Core;

namespace TurquoiseHealth.Api;

[Serializable]
public record ComparisonStats : IJsonOnDeserialized
{
    [JsonExtensionData]
    private readonly IDictionary<string, JsonElement> _extensionData =
        new Dictionary<string, JsonElement>();

    [JsonPropertyName("min")]
    public required Money Min { get; set; }

    [JsonPropertyName("max")]
    public required Money Max { get; set; }

    [JsonPropertyName("avg")]
    public required Money Avg { get; set; }

    [JsonPropertyName("median")]
    public required Money Median { get; set; }

    [JsonPropertyName("q1")]
    public required Money Q1 { get; set; }

    [JsonPropertyName("q3")]
    public required Money Q3 { get; set; }

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
