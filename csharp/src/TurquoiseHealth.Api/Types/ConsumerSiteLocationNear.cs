using System.Text.Json;
using System.Text.Json.Serialization;
using OneOf;
using TurquoiseHealth.Api.Core;

namespace TurquoiseHealth.Api;

/// <summary>
/// LocationNear
/// </summary>
[Serializable]
public record ConsumerSiteLocationNear : IJsonOnDeserialized
{
    [JsonExtensionData]
    private readonly IDictionary<string, JsonElement> _extensionData =
        new Dictionary<string, JsonElement>();

    /// <summary>
    /// Latitude of the search anchor.
    /// </summary>
    [JsonPropertyName("lat")]
    public required OneOf<double, int> Lat { get; set; }

    /// <summary>
    /// Longitude of the search anchor.
    /// </summary>
    [JsonPropertyName("lng")]
    public required OneOf<double, int> Lng { get; set; }

    /// <summary>
    /// Search radius in meters.
    /// </summary>
    [JsonPropertyName("radius_m")]
    public int? RadiusM { get; set; }

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
