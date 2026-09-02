using System.Text.Json.Serialization;
using TurquoiseHealth.Api.Core;

namespace TurquoiseHealth.Api;

[Serializable]
public record V3ListProvidersRequest
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
    /// Provider type as reported in the dataset (exact match).
    /// </summary>
    [JsonIgnore]
    public string? Type { get; set; }

    /// <summary>
    /// Providers with at least one price for this package.
    /// </summary>
    [JsonIgnore]
    public string? PackageId { get; set; }

    /// <summary>
    /// Providers with at least one price under this network (combined with package_id/payer_id, the same price row must match).
    /// </summary>
    [JsonIgnore]
    public string? NetworkId { get; set; }

    /// <summary>
    /// Providers priced under any of this payer's networks (combined with other relationship filters, the same price row must match).
    /// </summary>
    [JsonIgnore]
    public string? PayerId { get; set; }

    /// <summary>
    /// Semantic search over provider names. Composes with the other filters and any location mode; returns a single relevance-ordered page. 503 search_unavailable until the provider embedding index is populated.
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
