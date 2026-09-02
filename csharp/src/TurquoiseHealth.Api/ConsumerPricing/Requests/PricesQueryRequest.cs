using System.Text.Json.Serialization;
using TurquoiseHealth.Api.Core;

namespace TurquoiseHealth.Api;

[Serializable]
public record PricesQueryRequest
{
    [JsonPropertyName("package_id")]
    public required string PackageId { get; set; }

    [JsonPropertyName("provider_id")]
    public string? ProviderId { get; set; }

    [JsonPropertyName("pricing")]
    public required PricesQueryRequestPricing Pricing { get; set; }

    [JsonPropertyName("location")]
    public Location? Location { get; set; }

    /// <summary>
    /// `total` (default) or `distance` (requires a near/zip location).
    /// </summary>
    [JsonPropertyName("sort")]
    public PriceSort? Sort { get; set; }

    /// <summary>
    /// Sort order. Defaults to ascending (lowest total / nearest first).
    /// </summary>
    [JsonPropertyName("sort_direction")]
    public PricesQueryRequestSortDirection? SortDirection { get; set; }

    [JsonPropertyName("page_size")]
    public int? PageSize { get; set; }

    [JsonPropertyName("cursor")]
    public string? Cursor { get; set; }

    /// <inheritdoc />
    public override string ToString()
    {
        return JsonUtils.Serialize(this);
    }
}
