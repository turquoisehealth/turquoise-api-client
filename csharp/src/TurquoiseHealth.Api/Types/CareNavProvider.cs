using System.Text.Json;
using System.Text.Json.Serialization;
using TurquoiseHealth.Api.Core;

namespace TurquoiseHealth.Api;

/// <summary>
/// Care-navigation (v1) provider row.
///
/// Named `CareNavProvider` (not `Provider`) so it doesn't collide with
/// `modules.external.consumer_pricing_v2.dtos.providers.Provider` in the
/// shared external OpenAPI export. FastAPI keys component schemas by class
/// name; without distinct names, both modules would emit `Provider` and the
/// generated `client_external` would parse v1 responses with the v2 shape.
/// </summary>
[Serializable]
public record CareNavProvider : IJsonOnDeserialized
{
    [JsonExtensionData]
    private readonly IDictionary<string, JsonElement> _extensionData =
        new Dictionary<string, JsonElement>();

    /// <summary>
    /// Provider identifier
    /// </summary>
    [JsonPropertyName("provider_id")]
    public required string ProviderId { get; set; }

    /// <summary>
    /// Provider facility or practice name
    /// </summary>
    [JsonPropertyName("provider_name")]
    public required string ProviderName { get; set; }

    /// <summary>
    /// Type of provider (e.g. hospital, ASC)
    /// </summary>
    [JsonPropertyName("provider_type")]
    public string? ProviderType { get; set; }

    /// <summary>
    /// National Provider Identifier
    /// </summary>
    [JsonPropertyName("npi")]
    public string? Npi { get; set; }

    /// <summary>
    /// Provider city
    /// </summary>
    [JsonPropertyName("city")]
    public required string City { get; set; }

    /// <summary>
    /// Two-letter state abbreviation
    /// </summary>
    [JsonPropertyName("state")]
    public required string State { get; set; }

    /// <summary>
    /// Provider ZIP code
    /// </summary>
    [JsonPropertyName("zip_code")]
    public required string ZipCode { get; set; }

    /// <summary>
    /// Location and quality details, populated for geo-enriched results
    /// </summary>
    [JsonPropertyName("location_details")]
    public ProviderLocationDetails? LocationDetails { get; set; }

    [JsonIgnore]
    public ReadOnlyAdditionalProperties AdditionalProperties { get; private set; } = new();

    void IJsonOnDeserialized.OnDeserialized() =>
        AdditionalProperties.CopyFromExtensionData(_extensionData);

    /// <inheritdoc />
    public override string ToString()
    {
        return JsonUtils.Serialize(this);
    }
}
