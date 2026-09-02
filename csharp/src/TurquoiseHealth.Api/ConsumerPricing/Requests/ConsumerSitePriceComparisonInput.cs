using System.Text.Json.Serialization;
using TurquoiseHealth.Api.Core;

namespace TurquoiseHealth.Api;

[Serializable]
public record ConsumerSitePriceComparisonInput
{
    [JsonPropertyName("package_id")]
    public required string PackageId { get; set; }

    [JsonPropertyName("provider_id")]
    public string? ProviderId { get; set; }

    [JsonPropertyName("pricing")]
    public required ConsumerSitePricingNegotiated Pricing { get; set; }

    [JsonPropertyName("member_eligibility")]
    public required ConsumerSiteMemberEligibilityInput MemberEligibility { get; set; }

    [JsonPropertyName("location")]
    public ConsumerSiteLocation? Location { get; set; }

    /// <inheritdoc />
    public override string ToString()
    {
        return JsonUtils.Serialize(this);
    }
}
