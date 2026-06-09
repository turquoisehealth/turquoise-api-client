using System.Text.Json;
using System.Text.Json.Serialization;
using TurquoiseHealth.Api.Core;

namespace TurquoiseHealth.Api;

/// <summary>
/// Structured location for POST bodies. Exactly zero or one mode permitted.
///
/// Validated by the service layer (location.py shares LocationQuery for GET
/// query params; this is the body equivalent).
/// </summary>
[Serializable]
public record RateCompareLocation : IJsonOnDeserialized
{
    [JsonExtensionData]
    private readonly IDictionary<string, JsonElement> _extensionData =
        new Dictionary<string, JsonElement>();

    /// <summary>
    /// Latitude for `near` mode. Must be paired with `near_lng`.
    /// </summary>
    [JsonPropertyName("near_lat")]
    public double? NearLat { get; set; }

    /// <summary>
    /// Longitude for `near` mode. Must be paired with `near_lat`.
    /// </summary>
    [JsonPropertyName("near_lng")]
    public double? NearLng { get; set; }

    /// <summary>
    /// Search radius in meters for `near` mode. Defaults to 25 000 when omitted.
    /// </summary>
    [JsonPropertyName("near_radius_m")]
    public int? NearRadiusM { get; set; }

    /// <summary>
    /// Two-letter USPS state code for `within` mode (exact match).
    /// </summary>
    [JsonPropertyName("within_state")]
    public string? WithinState { get; set; }

    /// <summary>
    /// CBSA name for `within` mode (case-insensitive substring match).
    /// </summary>
    [JsonPropertyName("within_cbsa_name")]
    public string? WithinCbsaName { get; set; }

    /// <summary>
    /// ZIP codes for `within` mode (exact match, any-of).
    /// </summary>
    [JsonPropertyName("within_zip_codes")]
    public IEnumerable<string>? WithinZipCodes { get; set; }

    /// <summary>
    /// Convenience: resolves a ZIP to its centroid, then runs `near` with the default radius.
    /// </summary>
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
