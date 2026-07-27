using System.Text.Json;
using System.Text.Json.Serialization;
using TurquoiseHealth.Api.Core;

namespace TurquoiseHealth.Api;

[Serializable]
public record V3Network : IJsonOnDeserialized
{
    [JsonExtensionData]
    private readonly IDictionary<string, JsonElement> _extensionData =
        new Dictionary<string, JsonElement>();

    [JsonPropertyName("object")]
    public V3NetworkObject? Object { get; set; }

    /// <summary>
    /// Network identifier. Upstream-derived from the dataset and may change as the dataset is rebuilt. Re-resolve via search rather than persisting long-term.
    /// </summary>
    [JsonPropertyName("id")]
    public required string Id { get; set; }

    /// <summary>
    /// Display name of the network.
    /// </summary>
    [JsonPropertyName("name")]
    public required string Name { get; set; }

    /// <summary>
    /// The network's payer as an {id, name} stub; the full entity lives at /v3/payers/{id}.
    /// </summary>
    [JsonPropertyName("payer")]
    public required V3EntityRef Payer { get; set; }

    /// <summary>
    /// Query-relative metadata (search only).
    /// </summary>
    [JsonPropertyName("context")]
    public V3SearchContext? Context { get; set; }

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
