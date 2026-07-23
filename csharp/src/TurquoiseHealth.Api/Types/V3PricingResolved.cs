using System.Text.Json;
using System.Text.Json.Serialization;
using TurquoiseHealth.Api.Core;

namespace TurquoiseHealth.Api;

/// <summary>
/// The resolved payment arrangement on a price. Cash prices omit
/// network/payer; negotiated prices carry both as reference stubs.
/// </summary>
[Serializable]
public record V3PricingResolved : IJsonOnDeserialized
{
    [JsonExtensionData]
    private readonly IDictionary<string, JsonElement> _extensionData =
        new Dictionary<string, JsonElement>();

    [JsonPropertyName("type")]
    public required V3PricingResolvedType Type { get; set; }

    [JsonPropertyName("network")]
    public V3EntityRef? Network { get; set; }

    [JsonPropertyName("payer")]
    public V3EntityRef? Payer { get; set; }

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
