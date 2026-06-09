using System.Text.Json.Serialization;
using TurquoiseHealth.Api.Core;

namespace TurquoiseHealth.Api;

[Serializable]
public record PackagePricesRequest
{
    /// <summary>
    /// Standard service package identifier
    /// </summary>
    [JsonIgnore]
    public required string SspId { get; set; }

    /// <summary>
    /// Page number
    /// </summary>
    [JsonIgnore]
    public int? Page { get; set; }

    /// <summary>
    /// Page size
    /// </summary>
    [JsonIgnore]
    public int? PageSize { get; set; }

    /// <summary>
    /// Location to search for providers
    /// </summary>
    [JsonPropertyName("location")]
    public required LocationInput Location { get; set; }

    /// <summary>
    /// Insurance network identifier
    /// </summary>
    [JsonPropertyName("network_id")]
    public string? NetworkId { get; set; }

    [JsonPropertyName("price_filter")]
    public PriceFilter? PriceFilter { get; set; }

    [JsonPropertyName("npis")]
    public IEnumerable<string>? Npis { get; set; }

    [JsonPropertyName("sort_by")]
    public CareNavSortType? SortBy { get; set; }

    /// <inheritdoc />
    public override string ToString()
    {
        return JsonUtils.Serialize(this);
    }
}
