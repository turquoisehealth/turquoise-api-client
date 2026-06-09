using System.Text.Json;
using System.Text.Json.Serialization;
using TurquoiseHealth.Api.Core;

namespace TurquoiseHealth.Api;

[Serializable]
public record PackageLineItem : IJsonOnDeserialized
{
    [JsonExtensionData]
    private readonly IDictionary<string, JsonElement> _extensionData =
        new Dictionary<string, JsonElement>();

    [JsonPropertyName("line_code")]
    public required string LineCode { get; set; }

    [JsonPropertyName("code_type")]
    public required string CodeType { get; set; }

    [JsonPropertyName("fee_type")]
    public required string FeeType { get; set; }

    [JsonPropertyName("description")]
    public required string Description { get; set; }

    [JsonPropertyName("line_item_association_rate")]
    public required double LineItemAssociationRate { get; set; }

    [JsonPropertyName("mrf_rate")]
    public double? MrfRate { get; set; }

    [JsonPropertyName("benefit_category")]
    public string? BenefitCategory { get; set; }

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
