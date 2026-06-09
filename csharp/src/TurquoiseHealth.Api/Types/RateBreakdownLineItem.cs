using System.Text.Json;
using System.Text.Json.Serialization;
using TurquoiseHealth.Api.Core;

namespace TurquoiseHealth.Api;

[Serializable]
public record RateBreakdownLineItem : IJsonOnDeserialized
{
    [JsonExtensionData]
    private readonly IDictionary<string, JsonElement> _extensionData =
        new Dictionary<string, JsonElement>();

    /// <summary>
    /// CPT or HCPCS code.
    /// </summary>
    [JsonPropertyName("line_code")]
    public required string LineCode { get; set; }

    /// <summary>
    /// Billing code type.
    /// </summary>
    [JsonPropertyName("code_type")]
    public required string CodeType { get; set; }

    /// <summary>
    /// Fee category (professional, facility, etc).
    /// </summary>
    [JsonPropertyName("fee_type")]
    public required string FeeType { get; set; }

    /// <summary>
    /// Human-readable line item description.
    /// </summary>
    [JsonPropertyName("description")]
    public required string Description { get; set; }

    /// <summary>
    /// Likelihood this line item is part of the SSP.
    /// </summary>
    [JsonPropertyName("line_item_association_rate")]
    public required double LineItemAssociationRate { get; set; }

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
