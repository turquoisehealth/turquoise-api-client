using System.Text.Json;
using System.Text.Json.Serialization;
using OneOf;
using TurquoiseHealth.Api.Core;

namespace TurquoiseHealth.Api;

/// <summary>
/// Structured location for POST bodies. Exactly zero or one mode permitted.  Validated by the service layer (location.py shares LocationQuery for GET query params; this is the body equivalent).
/// </summary>
[Serializable]
public record ConsumerSiteRateCompareLocation : IJsonOnDeserialized
{
    [JsonExtensionData]
    private readonly IDictionary<string, JsonElement> _extensionData =
        new Dictionary<string, JsonElement>();

    [JsonPropertyName("near_lat")]
    public OneOf<double, int>? NearLat { get; set; }

    [JsonPropertyName("near_lng")]
    public OneOf<double, int>? NearLng { get; set; }

    [JsonPropertyName("near_radius_m")]
    public int? NearRadiusM { get; set; }

    [JsonPropertyName("within_state")]
    public string? WithinState { get; set; }

    [JsonPropertyName("within_cbsa_name")]
    public string? WithinCbsaName { get; set; }

    [JsonPropertyName("within_zip_codes")]
    public IEnumerable<string>? WithinZipCodes { get; set; }

    [JsonPropertyName("zip_anchor")]
    public string? ZipAnchor { get; set; }

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
