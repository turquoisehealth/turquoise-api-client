using System.Text.Json;
using System.Text.Json.Serialization;
using TurquoiseHealth.Api.Core;

namespace TurquoiseHealth.Api;

/// <summary>
/// ModulesConsumerPricingV3DtosPersonalizedEstimatePersonalizedEstimate
/// </summary>
[Serializable]
public record ConsumerSiteModulesConsumerPricingV3DtosPersonalizedEstimatePersonalizedEstimate
    : IJsonOnDeserialized
{
    [JsonExtensionData]
    private readonly IDictionary<string, JsonElement> _extensionData =
        new Dictionary<string, JsonElement>();

    [JsonPropertyName("object")]
    public string? Object { get; set; }

    [JsonPropertyName("provider")]
    public required ConsumerSiteEntityRef Provider { get; set; }

    [JsonPropertyName("package")]
    public required ConsumerSiteEntityRef Package { get; set; }

    [JsonPropertyName("pricing")]
    public required ConsumerSitePricingResolved Pricing { get; set; }

    /// <summary>
    /// Amount that insurance company + patient pay for the service.
    /// </summary>
    [JsonPropertyName("total_allowed_amount")]
    public required ConsumerSiteMoney TotalAllowedAmount { get; set; }

    [JsonPropertyName("member_cost_share")]
    public required ConsumerSiteMemberCostShare MemberCostShare { get; set; }

    [JsonPropertyName("sub_package_id")]
    public string? SubPackageId { get; set; }

    [JsonPropertyName("line_items")]
    public IEnumerable<ConsumerSiteLineItem>? LineItems { get; set; }

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
