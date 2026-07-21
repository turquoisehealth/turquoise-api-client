using System.Text.Json;
using System.Text.Json.Serialization;
using TurquoiseHealth.Api.Core;

namespace TurquoiseHealth.Api;

[Serializable]
public record V3Provider : IJsonOnDeserialized
{
    [JsonExtensionData]
    private readonly IDictionary<string, JsonElement> _extensionData =
        new Dictionary<string, JsonElement>();

    [JsonPropertyName("object")]
    public V3ProviderObject? Object { get; set; }

    /// <summary>
    /// Provider identifier. Upstream-derived from the dataset and may change as the dataset is rebuilt. Re-resolve via search rather than persisting long-term.
    /// </summary>
    [JsonPropertyName("id")]
    public required string Id { get; set; }

    /// <summary>
    /// Provider facility or organization name.
    /// </summary>
    [JsonPropertyName("name")]
    public required string Name { get; set; }

    /// <summary>
    /// Provider type as reported in the dataset.
    /// </summary>
    [JsonPropertyName("type")]
    public string? Type { get; set; }

    /// <summary>
    /// National Provider Identifier.
    /// </summary>
    [JsonPropertyName("npi")]
    public string? Npi { get; set; }

    [JsonPropertyName("address")]
    public required V3ProviderAddress Address { get; set; }

    /// <summary>
    /// Query-relative metadata (near/zip mode only).
    /// </summary>
    [JsonPropertyName("context")]
    public V3MatchContext? Context { get; set; }

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
