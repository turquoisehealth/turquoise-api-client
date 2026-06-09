using System.Text.Json;
using System.Text.Json.Serialization;
using TurquoiseHealth.Api.Core;

namespace TurquoiseHealth.Api;

[Serializable]
public record EnvelopeRate : IJsonOnDeserialized
{
    [JsonExtensionData]
    private readonly IDictionary<string, JsonElement> _extensionData =
        new Dictionary<string, JsonElement>();

    [JsonPropertyName("items")]
    public IEnumerable<Rate> Items { get; set; } = new List<Rate>();

    [JsonPropertyName("page")]
    public required PageMeta Page { get; set; }

    /// <summary>
    /// Set when `items` is empty. `no_data` means no matching records exist; `permission_denied` means matching records exist but are excluded by your data permissions.
    /// </summary>
    [JsonPropertyName("no_data_reason")]
    public NoDataReason? NoDataReason { get; set; }

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
