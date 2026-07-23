using System.Text.Json.Serialization;
using TurquoiseHealth.Api.Core;

namespace TurquoiseHealth.Api;

[Serializable]
public record RateCompareRequest
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
    /// Filter to a single insurance network. Omit (or pass null) to get cash prices. Int64String — JSON string-wrapped 64-bit integer, because JSON cannot safely represent values above 2^53. Upstream-derived from the dataset and may change as the dataset is rebuilt. Do not bake into URLs, bookmarks, or persistent storage.
    /// </summary>
    [JsonPropertyName("network_id")]
    public string? NetworkId { get; set; }

    /// <summary>
    /// Optional location scope. Exactly zero or one of `near`, `within`, or `zip_anchor` modes.
    /// </summary>
    [JsonPropertyName("location")]
    public RateCompareLocation? Location { get; set; }

    /// <inheritdoc />
    public override string ToString()
    {
        return JsonUtils.Serialize(this);
    }
}
