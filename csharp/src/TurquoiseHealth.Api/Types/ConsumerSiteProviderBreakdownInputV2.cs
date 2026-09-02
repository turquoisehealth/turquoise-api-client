using System.Text.Json;
using System.Text.Json.Serialization;
using TurquoiseHealth.Api.Core;

namespace TurquoiseHealth.Api;

/// <summary>
/// Inbound payload for the provider-breakdown endpoint.
///
/// Extends eligibility fields with required provider and service identifiers.
/// </summary>
[Serializable]
public record ConsumerSiteProviderBreakdownInputV2 : IJsonOnDeserialized
{
    [JsonExtensionData]
    private readonly IDictionary<string, JsonElement> _extensionData =
        new Dictionary<string, JsonElement>();

    [JsonPropertyName("first_name")]
    public required string FirstName { get; set; }

    [JsonPropertyName("last_name")]
    public required string LastName { get; set; }

    [JsonPropertyName("date_of_birth")]
    public required DateOnly DateOfBirth { get; set; }

    [JsonPropertyName("member_id")]
    public required string MemberId { get; set; }

    [JsonPropertyName("consent_attested")]
    public required string ConsentAttested { get; set; }

    [JsonPropertyName("network_id")]
    public required int NetworkId { get; set; }

    [JsonPropertyName("provider_id")]
    public required string ProviderId { get; set; }

    [JsonPropertyName("ssp_id")]
    public required string SspId { get; set; }

    [JsonPropertyName("location")]
    public ConsumerSiteRateCompareLocation? Location { get; set; }

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
