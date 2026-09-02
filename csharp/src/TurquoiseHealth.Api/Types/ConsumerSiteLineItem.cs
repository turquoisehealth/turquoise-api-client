using System.Text.Json;
using System.Text.Json.Serialization;
using OneOf;
using TurquoiseHealth.Api.Core;

namespace TurquoiseHealth.Api;

/// <summary>
/// A component of a package's composition (from `api_ssp_contents`).
/// </summary>
[Serializable]
public record ConsumerSiteLineItem : IJsonOnDeserialized
{
    [JsonExtensionData]
    private readonly IDictionary<string, JsonElement> _extensionData =
        new Dictionary<string, JsonElement>();

    /// <summary>
    /// Billing code.
    /// </summary>
    [JsonPropertyName("code")]
    public required string Code { get; set; }

    /// <summary>
    /// Billing code type.
    /// </summary>
    [JsonPropertyName("code_type")]
    public required string CodeType { get; set; }

    /// <summary>
    /// Fee category: base_code, facility_fee, professional_fee, optional_fee.
    /// </summary>
    [JsonPropertyName("fee_type")]
    public required string FeeType { get; set; }

    /// <summary>
    /// Human-readable line item description.
    /// </summary>
    [JsonPropertyName("description")]
    public required string Description { get; set; }

    /// <summary>
    /// Likelihood this line item is part of the package (0-1).
    /// </summary>
    [JsonPropertyName("association_rate")]
    public required OneOf<double, int> AssociationRate { get; set; }

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
