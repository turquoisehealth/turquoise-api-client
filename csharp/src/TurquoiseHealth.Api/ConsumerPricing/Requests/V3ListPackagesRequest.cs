using System.Text.Json.Serialization;
using TurquoiseHealth.Api.Core;

namespace TurquoiseHealth.Api;

[Serializable]
public record V3ListPackagesRequest
{
    /// <summary>
    /// Case-insensitive substring match on package name.
    /// </summary>
    [JsonIgnore]
    public string? Name { get; set; }

    /// <summary>
    /// Billing code lookup; matches against anchor_codes[].code (package base codes only). Returns every package anchored by the code — exactly one in the current catalog, but uniqueness is not contractual (an anchor's full upstream identity includes revenue code and billing class, which this API collapses).
    /// </summary>
    [JsonIgnore]
    public string? AnchorCode { get; set; }

    /// <summary>
    /// Packages priced at this provider.
    /// </summary>
    [JsonIgnore]
    public string? ProviderId { get; set; }

    /// <summary>
    /// Packages with at least one price under this network (combined with provider_id/payer_id, the same price row must match).
    /// </summary>
    [JsonIgnore]
    public string? NetworkId { get; set; }

    /// <summary>
    /// Packages priced under any of this payer's networks (combined with other relationship filters, the same price row must match).
    /// </summary>
    [JsonIgnore]
    public string? PayerId { get; set; }

    /// <summary>
    /// Semantic search over package name and description. Composes with the other filters; returns a single relevance-ordered page (no cursor). 503 search_unavailable when the embedding index is not populated.
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

    /// <inheritdoc />
    public override string ToString()
    {
        return JsonUtils.Serialize(this);
    }
}
