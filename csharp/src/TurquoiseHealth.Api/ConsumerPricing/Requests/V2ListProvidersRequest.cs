using System.Text.Json.Serialization;
using TurquoiseHealth.Api.Core;

namespace TurquoiseHealth.Api;

[Serializable]
public record V2ListProvidersRequest
{
    /// <summary>
    /// Case-insensitive substring match on provider name.
    /// </summary>
    [JsonIgnore]
    public string? Name { get; set; }

    /// <summary>
    /// Exact NPI match.
    /// </summary>
    [JsonIgnore]
    public string? Npi { get; set; }

    /// <summary>
    /// Page size.
    /// </summary>
    [JsonIgnore]
    public int? PageSize { get; set; }

    /// <summary>
    /// Opaque cursor from a previous response's page.next_cursor.
    /// </summary>
    [JsonIgnore]
    public string? Cursor { get; set; }

    /// <summary>
    /// Latitude anchor for `near` mode. Must be paired with `near.lng`.
    /// </summary>
    [JsonIgnore]
    public double? NearLat { get; set; }

    /// <summary>
    /// Longitude anchor for `near` mode. Must be paired with `near.lat`.
    /// </summary>
    [JsonIgnore]
    public double? NearLng { get; set; }

    /// <summary>
    /// Search radius in meters for `near` mode. Defaults to 25 000 when omitted.
    /// </summary>
    [JsonIgnore]
    public int? NearRadiusM { get; set; }

    /// <summary>
    /// Two-letter USPS state code for `within` mode (exact match).
    /// </summary>
    [JsonIgnore]
    public string? WithinState { get; set; }

    /// <summary>
    /// CBSA name for `within` mode (case-insensitive substring match).
    /// </summary>
    [JsonIgnore]
    public string? WithinCbsaName { get; set; }

    /// <summary>
    /// ZIP codes for `within` mode (exact match, any-of). Repeat the param for multiple values.
    /// </summary>
    [JsonIgnore]
    public IEnumerable<string>? WithinZipCodes { get; set; }

    /// <summary>
    /// Convenience: resolves a ZIP to its centroid, then runs `near` with the default radius.
    /// </summary>
    [JsonIgnore]
    public string? ZipAnchor { get; set; }

    /// <inheritdoc />
    public override string ToString()
    {
        return JsonUtils.Serialize(this);
    }
}
