using System.Text.Json;
using System.Text.Json.Serialization;
using TurquoiseHealth.Api.Core;

namespace TurquoiseHealth.Api;

/// <summary>
/// Structured location intent: exactly one of the three modes.
/// </summary>
[Serializable]
public record ConsumerSiteLocation : IJsonOnDeserialized
{
    [JsonExtensionData]
    private readonly IDictionary<string, JsonElement> _extensionData =
        new Dictionary<string, JsonElement>();

    [JsonPropertyName("near")]
    public ConsumerSiteLocationNear? Near { get; set; }

    [JsonPropertyName("within")]
    public ConsumerSiteLocationWithin? Within { get; set; }

    [JsonPropertyName("zip")]
    public string? Zip { get; set; }

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
