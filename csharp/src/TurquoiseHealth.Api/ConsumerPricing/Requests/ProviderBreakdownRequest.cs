using System.Text.Json.Serialization;
using TurquoiseHealth.Api.Core;

namespace TurquoiseHealth.Api;

[Serializable]
public record ProviderBreakdownRequest
{
    /// <summary>
    /// Provider identifier. Upstream-derived from the dataset and may change as the dataset is rebuilt. Do not bake into URLs, bookmarks, or persistent storage.
    /// </summary>
    [JsonPropertyName("provider_id")]
    public required string ProviderId { get; set; }

    /// <summary>
    /// SSP identifier. Upstream-derived from the dataset and may change as the dataset is rebuilt. Do not bake into URLs, bookmarks, or persistent storage.
    /// </summary>
    [JsonPropertyName("ssp_id")]
    public required string SspId { get; set; }

    /// <summary>
    /// Optional network filter. Omit for cash-price breakdown. Int64String — JSON string-wrapped 64-bit integer, because JSON cannot safely represent values above 2^53. Upstream-derived from the dataset and may change as the dataset is rebuilt. Do not bake into URLs, bookmarks, or persistent storage.
    /// </summary>
    [JsonPropertyName("network_id")]
    public string? NetworkId { get; set; }

    /// <inheritdoc />
    public override string ToString()
    {
        return JsonUtils.Serialize(this);
    }
}
