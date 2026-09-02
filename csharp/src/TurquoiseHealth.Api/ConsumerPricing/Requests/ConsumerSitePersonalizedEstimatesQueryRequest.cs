using System.Text.Json.Serialization;
using TurquoiseHealth.Api.Core;

namespace TurquoiseHealth.Api;

[Serializable]
public record ConsumerSitePersonalizedEstimatesQueryRequest
{
    /// <summary>
    /// Optional items to expand on
    /// </summary>
    [JsonIgnore]
    public IEnumerable<V3ListPersonalizedEstimatesRequestExpandItem> Expand { get; set; } =
        new List<V3ListPersonalizedEstimatesRequestExpandItem>();

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

    [JsonPropertyName("sort")]
    public ConsumerSitePriceSort? Sort { get; set; }

    [JsonPropertyName("sort_direction")]
    public ConsumerSitePersonalizedEstimatesQueryRequestSortDirection? SortDirection { get; set; }

    [JsonPropertyName("page_size")]
    public int? PageSize { get; set; }

    [JsonPropertyName("cursor")]
    public string? Cursor { get; set; }

    /// <inheritdoc />
    public override string ToString()
    {
        return JsonUtils.Serialize(this);
    }
}
