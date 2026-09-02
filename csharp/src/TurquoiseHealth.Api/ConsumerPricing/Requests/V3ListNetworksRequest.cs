using System.Text.Json.Serialization;
using TurquoiseHealth.Api.Core;

namespace TurquoiseHealth.Api;

[Serializable]
public record V3ListNetworksRequest
{
    /// <summary>
    /// Case-insensitive substring match on network or payer name.
    /// </summary>
    [JsonIgnore]
    public string? Name { get; set; }

    /// <summary>
    /// Exact payer id match.
    /// </summary>
    [JsonIgnore]
    public string? PayerId { get; set; }

    /// <summary>
    /// Networks with at least one price at this provider.
    /// </summary>
    [JsonIgnore]
    public string? ProviderId { get; set; }

    /// <summary>
    /// Networks with at least one price for this package.
    /// </summary>
    [JsonIgnore]
    public string? PackageId { get; set; }

    /// <summary>
    /// Semantic search over network and payer names. Composes with the other filters and location; returns a single relevance-ordered page (no cursor).
    /// </summary>
    [JsonIgnore]
    public string? Search { get; set; }

    /// <summary>
    /// Minimum similarity score (0-1).
    /// </summary>
    [JsonIgnore]
    public double? MinScore { get; set; }

    [JsonIgnore]
    public int? PageSize { get; set; }

    /// <summary>
    /// Opaque cursor from a previous page.next_cursor.
    /// </summary>
    [JsonIgnore]
    public string? Cursor { get; set; }

    [JsonIgnore]
    public double? LocationNearLat { get; set; }

    [JsonIgnore]
    public double? LocationNearLng { get; set; }

    [JsonIgnore]
    public int? LocationNearRadiusM { get; set; }

    [JsonIgnore]
    public string? LocationWithinState { get; set; }

    [JsonIgnore]
    public string? LocationWithinCbsa { get; set; }

    /// <summary>
    /// Comma-separated ZIP codes (exact match, any-of).
    /// </summary>
    [JsonIgnore]
    public string? LocationWithinZipCodes { get; set; }

    /// <summary>
    /// Resolves the ZIP to its centroid, then runs `near` with the default radius.
    /// </summary>
    [JsonIgnore]
    public string? LocationZip { get; set; }

    /// <inheritdoc />
    public override string ToString()
    {
        return JsonUtils.Serialize(this);
    }
}
