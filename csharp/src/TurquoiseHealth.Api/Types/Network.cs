using System.Text.Json;
using System.Text.Json.Serialization;
using TurquoiseHealth.Api.Core;

namespace TurquoiseHealth.Api;

[Serializable]
public record Network : IJsonOnDeserialized
{
    [JsonExtensionData]
    private readonly IDictionary<string, JsonElement> _extensionData =
        new Dictionary<string, JsonElement>();

    [JsonPropertyName("object")]
    public NetworkObject? Object { get; set; }

    /// <summary>
    /// Network identifier. Identifiers may change when the underlying data is refreshed. Look up the entity again through the API rather than storing its identifier long-term.
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
    public required EntityRef Payer { get; set; }

    /// <summary>
    /// Query-relative metadata (search only).
    /// </summary>
    [JsonPropertyName("context")]
    public SearchContext? Context { get; set; }

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
