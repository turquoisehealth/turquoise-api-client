using System.Text.Json;
using System.Text.Json.Serialization;
using TurquoiseHealth.Api.Core;

namespace TurquoiseHealth.Api;

[Serializable]
public record V3PageMeta : IJsonOnDeserialized
{
    [JsonExtensionData]
    private readonly IDictionary<string, JsonElement> _extensionData =
        new Dictionary<string, JsonElement>();

    /// <summary>
    /// Number of items in this page.
    /// </summary>
    [JsonPropertyName("size")]
    public required int Size { get; set; }

    /// <summary>
    /// Count of the full filtered set, not just this page.
    /// </summary>
    [JsonPropertyName("total")]
    public required int Total { get; set; }

    /// <summary>
    /// Opaque cursor for the next page; null when exhausted.
    /// </summary>
    [JsonPropertyName("next_cursor")]
    public string? NextCursor { get; set; }

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
