using System.Text.Json;
using System.Text.Json.Serialization;
using TurquoiseHealth.Api.Core;

namespace TurquoiseHealth.Api;

/// <summary>
/// Inbound payload for the list-prices endpoint.
///
/// Extends eligibility fields with optional search parameters forwarded to the
/// personalized-estimates API.
/// </summary>
[Serializable]
public record ConsumerSiteListPricesInputV2 : IJsonOnDeserialized
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

    [JsonPropertyName("ssp_id")]
    public string? SspId { get; set; }

    [JsonPropertyName("provider_id")]
    public string? ProviderId { get; set; }

    [JsonPropertyName("location")]
    public ConsumerSiteRateCompareLocation? Location { get; set; }

    [JsonPropertyName("page_size")]
    public int? PageSize { get; set; }

    [JsonPropertyName("cursor")]
    public string? Cursor { get; set; }

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
