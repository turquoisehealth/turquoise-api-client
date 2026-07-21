using System.Text.Json.Serialization;
using TurquoiseHealth.Api.Core;

namespace TurquoiseHealth.Api;

[Serializable]
public record V3PricesQueryRequest
{
    [JsonPropertyName("package_id")]
    public required string PackageId { get; set; }

    [JsonPropertyName("provider_id")]
    public string? ProviderId { get; set; }

    [JsonPropertyName("pricing")]
    public required V3PricesQueryRequestPricing Pricing { get; set; }

    [JsonPropertyName("location")]
    public V3Location? Location { get; set; }

    /// <summary>
    /// `total` (default) or `distance` (requires a near/zip location).
    /// </summary>
    [JsonPropertyName("sort")]
    public V3PriceSort? Sort { get; set; }

    /// <summary>
    /// Sort order. Defaults to ascending (lowest total / nearest first).
    /// </summary>
    [JsonPropertyName("sort_direction")]
    public V3PricesQueryRequestSortDirection? SortDirection { get; set; }

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
