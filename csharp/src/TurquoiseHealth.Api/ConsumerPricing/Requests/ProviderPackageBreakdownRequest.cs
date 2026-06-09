using System.Text.Json.Serialization;
using TurquoiseHealth.Api.Core;

namespace TurquoiseHealth.Api;

[Serializable]
public record ProviderPackageBreakdownRequest
{
    /// <summary>
    /// Standard service package identifier
    /// </summary>
    [JsonIgnore]
    public required string SspId { get; set; }

    /// <summary>
    /// Provider identifier
    /// </summary>
    [JsonIgnore]
    public required string ProviderId { get; set; }

    /// <summary>
    /// Insurance network identifier
    /// </summary>
    [JsonPropertyName("network_id")]
    public string? NetworkId { get; set; }

    /// <inheritdoc />
    public override string ToString()
    {
        return JsonUtils.Serialize(this);
    }
}
