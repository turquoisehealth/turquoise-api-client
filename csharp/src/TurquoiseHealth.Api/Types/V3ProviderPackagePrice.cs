using System.Text.Json;
using System.Text.Json.Serialization;
using TurquoiseHealth.Api.Core;

namespace TurquoiseHealth.Api;

[Serializable]
public record V3ProviderPackagePrice : IJsonOnDeserialized
{
    [JsonExtensionData]
    private readonly IDictionary<string, JsonElement> _extensionData =
        new Dictionary<string, JsonElement>();

    [JsonPropertyName("object")]
    public V3ProviderPackagePriceObject? Object { get; set; }

    /// <summary>
    /// Price identifier (encodes the provider × package × pricing grain). Upstream-derived from the dataset and may change as the dataset is rebuilt. Re-resolve via search rather than persisting long-term.
    /// </summary>
    [JsonPropertyName("id")]
    public required string Id { get; set; }

    /// <summary>
    /// Full provider entity (always inlined).
    /// </summary>
    [JsonPropertyName("provider")]
    public required V3Provider Provider { get; set; }

    /// <summary>
    /// Reference stub.
    /// </summary>
    [JsonPropertyName("package")]
    public required V3EntityRef Package { get; set; }

    [JsonPropertyName("pricing")]
    public required V3PricingResolved Pricing { get; set; }

    /// <summary>
    /// The package-total price at this grain.
    /// </summary>
    [JsonPropertyName("total")]
    public required V3Money Total { get; set; }

    /// <summary>
    /// Package composition; present when expand=line_items (detail only).
    /// </summary>
    [JsonPropertyName("line_items")]
    public IEnumerable<V3LineItem>? LineItems { get; set; }

    [JsonPropertyName("context")]
    public V3MatchContext? Context { get; set; }

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
