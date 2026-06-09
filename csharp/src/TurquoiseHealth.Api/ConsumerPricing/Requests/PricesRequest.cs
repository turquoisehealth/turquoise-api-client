using System.Text.Json.Serialization;
using TurquoiseHealth.Api.Core;

namespace TurquoiseHealth.Api;

[Serializable]
public record PricesRequest
{
    /// <summary>
    /// Filter to a single SSP. Upstream-derived from the dataset and may change as the dataset is rebuilt. Do not bake into URLs, bookmarks, or persistent storage.
    /// </summary>
    [JsonPropertyName("ssp_id")]
    public string? SspId { get; set; }

    /// <summary>
    /// Filter to a single provider. Upstream-derived from the dataset and may change as the dataset is rebuilt. Do not bake into URLs, bookmarks, or persistent storage.
    /// </summary>
    [JsonPropertyName("provider_id")]
    public string? ProviderId { get; set; }

    /// <summary>
    /// Filter to a single insurance network. Int64String — JSON string-wrapped 64-bit integer, because JSON cannot safely represent values above 2^53. Upstream-derived from the dataset and may change as the dataset is rebuilt. Do not bake into URLs, bookmarks, or persistent storage.
    /// </summary>
    [JsonPropertyName("network_id")]
    public string? NetworkId { get; set; }

    /// <summary>
    /// `cash` filters to rows with no network. `negotiated` requires a network match.
    /// </summary>
    [JsonPropertyName("rate_type")]
    public PricesRequestRateType? RateType { get; set; }

    /// <summary>
    /// Optional location scope. Exactly zero or one of `near`, `within`, or `zip_anchor` modes.
    /// </summary>
    [JsonPropertyName("location")]
    public RateCompareLocation? Location { get; set; }

    /// <summary>
    /// Page size, 1-250.
    /// </summary>
    [JsonPropertyName("page_size")]
    public int? PageSize { get; set; }

    /// <summary>
    /// Opaque cursor from a previous response's `page.next_cursor`.
    /// </summary>
    [JsonPropertyName("cursor")]
    public string? Cursor { get; set; }

    /// <inheritdoc />
    public override string ToString()
    {
        return JsonUtils.Serialize(this);
    }
}
