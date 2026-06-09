using System.Text.Json;
using System.Text.Json.Serialization;
using TurquoiseHealth.Api.Core;

namespace TurquoiseHealth.Api;

/// <summary>
/// Insurance network row.
/// </summary>
[Serializable]
public record Network : IJsonOnDeserialized
{
    [JsonExtensionData]
    private readonly IDictionary<string, JsonElement> _extensionData =
        new Dictionary<string, JsonElement>();

    /// <summary>
    /// Network identifier. Int64String — JSON string-wrapped 64-bit integer, because JSON cannot safely represent values above 2^53. Upstream-derived from the dataset and may change as the dataset is rebuilt. Do not bake into URLs, bookmarks, or persistent storage.
    /// </summary>
    [JsonPropertyName("id")]
    public required string Id { get; set; }

    /// <summary>
    /// Insurance network name.
    /// </summary>
    [JsonPropertyName("name")]
    public required string Name { get; set; }

    /// <summary>
    /// Insurance payer identifier. Upstream-derived from the dataset and may change as the dataset is rebuilt. Do not bake into URLs, bookmarks, or persistent storage.
    /// </summary>
    [JsonPropertyName("payer_id")]
    public required string PayerId { get; set; }

    /// <summary>
    /// Insurance payer name.
    /// </summary>
    [JsonPropertyName("payer_name")]
    public required string PayerName { get; set; }

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
