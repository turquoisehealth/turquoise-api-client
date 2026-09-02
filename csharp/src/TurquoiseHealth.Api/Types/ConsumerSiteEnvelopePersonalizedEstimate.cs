using System.Text.Json;
using System.Text.Json.Serialization;
using TurquoiseHealth.Api.Core;

namespace TurquoiseHealth.Api;

/// <summary>
/// EnvelopePersonalizedEstimate
/// </summary>
[Serializable]
public record ConsumerSiteEnvelopePersonalizedEstimate : IJsonOnDeserialized
{
    [JsonExtensionData]
    private readonly IDictionary<string, JsonElement> _extensionData =
        new Dictionary<string, JsonElement>();

    [JsonPropertyName("items")]
    public IEnumerable<ConsumerSiteModulesConsumerPricingV2DtosRatePersonalizedEstimate> Items { get; set; } =
        new List<ConsumerSiteModulesConsumerPricingV2DtosRatePersonalizedEstimate>();

    [JsonPropertyName("page")]
    public required ConsumerSiteModulesConsumerPricingV2DtosCommonPageMeta Page { get; set; }

    [JsonPropertyName("no_data_reason")]
    public ConsumerSiteNoDataReason? NoDataReason { get; set; }

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
