using System.Text.Json;
using System.Text.Json.Serialization;
using TurquoiseHealth.Api.Core;

namespace TurquoiseHealth.Api;

[Serializable]
public record ProviderPackagePrice : IJsonOnDeserialized
{
    [JsonExtensionData]
    private readonly IDictionary<string, JsonElement> _extensionData =
        new Dictionary<string, JsonElement>();

    [JsonPropertyName("object")]
    public ProviderPackagePriceObject? Object { get; set; }

    /// <summary>
    /// Price identifier (encodes the provider × package × pricing grain). Identifiers may change when the underlying data is refreshed. Look up the entity again through the API rather than storing its identifier long-term.
    /// </summary>
    [JsonPropertyName("id")]
    public required string Id { get; set; }

    /// <summary>
    /// Full provider entity (always inlined).
    /// </summary>
    [JsonPropertyName("provider")]
    public required ConsumerPricingProvider Provider { get; set; }

    /// <summary>
    /// The package this price is for.
    /// </summary>
    [JsonPropertyName("package")]
    public required EntityRef Package { get; set; }

    [JsonPropertyName("pricing")]
    public required PricingResolved Pricing { get; set; }

    /// <summary>
    /// The package-total price at this grain.
    /// </summary>
    [JsonPropertyName("total")]
    public required Money Total { get; set; }

    /// <summary>
    /// Package composition; present when expand=line_items (detail only).
    /// </summary>
    [JsonPropertyName("line_items")]
    public IEnumerable<LineItem>? LineItems { get; set; }

    [JsonPropertyName("context")]
    public MatchContext? Context { get; set; }

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
