using System.Text.Json.Serialization;
using TurquoiseHealth.Api.Core;

namespace TurquoiseHealth.Api;

[Serializable]
public record GetProvidersRequest
{
    /// <summary>
    /// Match provider name (case-insensitive)
    /// </summary>
    [JsonIgnore]
    public string? ProviderName { get; set; }

    /// <summary>
    /// Exact provider NPI
    /// </summary>
    [JsonIgnore]
    public string? Npi { get; set; }

    /// <summary>
    /// Exact provider city
    /// </summary>
    [JsonIgnore]
    public string? City { get; set; }

    /// <summary>
    /// Two-letter state abbreviation
    /// </summary>
    [JsonIgnore]
    public string? State { get; set; }

    /// <summary>
    /// Exact ZIP code
    /// </summary>
    [JsonIgnore]
    public string? ZipCode { get; set; }

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
