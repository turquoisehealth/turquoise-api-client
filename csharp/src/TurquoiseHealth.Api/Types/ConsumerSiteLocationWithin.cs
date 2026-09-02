using System.Text.Json;
using System.Text.Json.Serialization;
using TurquoiseHealth.Api.Core;

namespace TurquoiseHealth.Api;

/// <summary>
/// Region filter. Exactly one of state / cbsa / zip_codes.
/// </summary>
[Serializable]
public record ConsumerSiteLocationWithin : IJsonOnDeserialized
{
    [JsonExtensionData]
    private readonly IDictionary<string, JsonElement> _extensionData =
        new Dictionary<string, JsonElement>();

    [JsonPropertyName("state")]
    public string? State { get; set; }

    [JsonPropertyName("cbsa")]
    public string? Cbsa { get; set; }

    [JsonPropertyName("zip_codes")]
    public IEnumerable<string>? ZipCodes { get; set; }

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
