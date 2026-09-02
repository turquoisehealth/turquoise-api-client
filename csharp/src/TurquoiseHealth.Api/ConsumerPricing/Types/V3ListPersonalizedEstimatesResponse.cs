using System.Text.Json;
using System.Text.Json.Serialization;
using TurquoiseHealth.Api.Core;

namespace TurquoiseHealth.Api;

[Serializable]
public record V3ListPersonalizedEstimatesResponse : IJsonOnDeserialized
{
    [JsonExtensionData]
    private readonly IDictionary<string, JsonElement> _extensionData =
        new Dictionary<string, JsonElement>();

    [JsonPropertyName("benefits_summary")]
    public required ConsumerSiteBenefitsSummary BenefitsSummary { get; set; }

    [JsonPropertyName("object")]
    public string? Object { get; set; }

    [JsonPropertyName("items")]
    public IEnumerable<ConsumerSiteModulesConsumerPricingV3DtosPersonalizedEstimatePersonalizedEstimate> Items { get; set; } =
        new List<ConsumerSiteModulesConsumerPricingV3DtosPersonalizedEstimatePersonalizedEstimate>();

    [JsonPropertyName("page")]
    public required ConsumerSiteModulesConsumerPricingV3DtosCommonPageMeta Page { get; set; }

    [JsonPropertyName("no_data_reason")]
    public ConsumerSiteNoDataReason? NoDataReason { get; set; }

    /// <summary>
    /// Human-readable caveats about what this result does and does not imply.
    /// </summary>
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
