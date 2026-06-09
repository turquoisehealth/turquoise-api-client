using System.Text.Json.Serialization;
using TurquoiseHealth.Api.Core;

namespace TurquoiseHealth.Api;

[Serializable]
public record GetSspsRequest
{
    /// <summary>
    /// Filter SSPs by partial name match
    /// </summary>
    [JsonIgnore]
    public string? SspName { get; set; }

    /// <summary>
    /// Filter SSPs by partial patient description match
    /// </summary>
    [JsonIgnore]
    public string? SspDescription { get; set; }

    /// <summary>
    /// Search SSPs by name or patient description
    /// </summary>
    [JsonIgnore]
    public string? Search { get; set; }

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
