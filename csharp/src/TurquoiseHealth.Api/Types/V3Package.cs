using System.Text.Json;
using System.Text.Json.Serialization;
using TurquoiseHealth.Api.Core;

namespace TurquoiseHealth.Api;

[Serializable]
public record V3Package : IJsonOnDeserialized
{
    [JsonExtensionData]
    private readonly IDictionary<string, JsonElement> _extensionData =
        new Dictionary<string, JsonElement>();

    [JsonPropertyName("object")]
    public V3PackageObject? Object { get; set; }

    /// <summary>
    /// Package identifier. Upstream-derived from the dataset and may change as the dataset is rebuilt. Re-resolve via search rather than persisting long-term.
    /// </summary>
    [JsonPropertyName("id")]
    public required string Id { get; set; }

    /// <summary>
    /// Package taxonomy. Only Standard Service Packages ('ssp') are served today; other package types (e.g. consumer bundles) may be added as they become servable.
    /// </summary>
    [JsonPropertyName("type")]
    public V3PackageType? Type { get; set; }

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
    /// The package's comprehensive anchor (base) billing codes — the rows billed as the anchor itself, not its professional-fee counterparts. Every package is expected to carry at least one. In the current catalog every anchor code maps to exactly one package (measured 07/2026), so ?anchor_code= behaves as a unique lookup — but that is an observed property, not a guarantee: the lookup returns every package anchored by the code.
    /// </summary>
    [JsonPropertyName("anchor_codes")]
    public IEnumerable<V3BillingCode>? AnchorCodes { get; set; }

    [JsonPropertyName("disclosures")]
    public IEnumerable<string>? Disclosures { get; set; }

    /// <summary>
    /// Query-relative metadata (search only).
    /// </summary>
    [JsonPropertyName("context")]
    public V3SearchContext? Context { get; set; }

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
