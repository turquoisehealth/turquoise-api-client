using System.Text.Json.Serialization;
using TurquoiseHealth.Api.Core;

namespace TurquoiseHealth.Api;

[Serializable]
public record GetInsuranceNetworksRequest
{
    /// <summary>
    /// Limit networks to a specific SSP
    /// </summary>
    [JsonIgnore]
    public string? SspId { get; set; }

    /// <summary>
    /// Filter by payer name
    /// </summary>
    [JsonIgnore]
    public string? PayerName { get; set; }

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

    /// <inheritdoc />
    public override string ToString()
    {
        return JsonUtils.Serialize(this);
    }
}
