using System.Text.Json.Serialization;
using TurquoiseHealth.Api.Core;

namespace TurquoiseHealth.Api;

[Serializable]
public record V2ListNetworksRequest
{
    /// <summary>
    /// Case-insensitive substring match on network or payer name. Ignored when 'search' parameter is provided.
    /// </summary>
    [JsonIgnore]
    public string? Name { get; set; }

    /// <summary>
    /// Filter to networks under this payer. Ignored when 'search' parameter is provided.
    /// </summary>
    [JsonIgnore]
    public string? PayerId { get; set; }

    /// <summary>
    /// Semantic search across network and payer names using AI embeddings. Returns results with similarity scores. When provided, other filter parameters are ignored and pagination is limited to the first page of top results.
    /// </summary>
    [JsonIgnore]
    public string? Search { get; set; }

    /// <summary>
    /// Minimum similarity score threshold (0-1) for semantic search results. Only applies when 'search' param is provided.
    /// </summary>
    [JsonIgnore]
    public double? MinScore { get; set; }

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

    /// <inheritdoc />
    public override string ToString()
    {
        return JsonUtils.Serialize(this);
    }
}
