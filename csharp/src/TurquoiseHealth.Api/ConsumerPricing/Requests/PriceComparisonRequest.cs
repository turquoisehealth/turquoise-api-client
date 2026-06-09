using System.Text.Json.Serialization;
using TurquoiseHealth.Api.Core;

namespace TurquoiseHealth.Api;

[Serializable]
public record PriceComparisonRequest
{
    /// <summary>
    /// Standard service package identifier
    /// </summary>
    [JsonIgnore]
    public required string SspId { get; set; }

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

    [JsonPropertyName("npis")]
    public IEnumerable<string>? Npis { get; set; }

    /// <inheritdoc />
    public override string ToString()
    {
        return JsonUtils.Serialize(this);
    }
}
