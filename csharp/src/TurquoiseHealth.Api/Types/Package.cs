using System.Text.Json;
using System.Text.Json.Serialization;
using TurquoiseHealth.Api.Core;

namespace TurquoiseHealth.Api;

[Serializable]
public record Package : IJsonOnDeserialized
{
    [JsonExtensionData]
    private readonly IDictionary<string, JsonElement> _extensionData =
        new Dictionary<string, JsonElement>();

    [JsonPropertyName("object")]
    public PackageObject? Object { get; set; }

    /// <summary>
    /// Package identifier. Identifiers may change when the underlying data is refreshed. Look up the entity again through the API rather than storing its identifier long-term.
    /// </summary>
    [JsonPropertyName("id")]
    public required string Id { get; set; }

    /// <summary>
    /// Package taxonomy. The SSP Grouper is the main package type.  If searching via anchor code, a specific SSP will be returned.  Either package id works as the package_id in subsequent API requests.
    /// </summary>
    [JsonPropertyName("type")]
    public PackageType? Type { get; set; }

    /// <summary>
    /// Display name of the service package.
    /// </summary>
    [JsonPropertyName("name")]
    public required string Name { get; set; }

    /// <summary>
    /// Patient-facing description of the package.
    /// </summary>
    [JsonPropertyName("description")]
    public string? Description { get; set; }

    /// <summary>
    /// The package's comprehensive anchor (base) billing codes — the rows billed as the anchor itself, not its professional-fee counterparts.
    /// </summary>
    [JsonPropertyName("anchor_codes")]
    public IEnumerable<BillingCode>? AnchorCodes { get; set; }

    /// <summary>
    /// Query-relative metadata (search only).
    /// </summary>
    [JsonPropertyName("context")]
    public SearchContext? Context { get; set; }

    [JsonPropertyName("disclosures")]
    public IEnumerable<string>? Disclosures { get; set; }

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
