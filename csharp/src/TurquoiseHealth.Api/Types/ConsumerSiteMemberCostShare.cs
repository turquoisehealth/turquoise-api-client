using System.Text.Json;
using System.Text.Json.Serialization;
using TurquoiseHealth.Api.Core;

namespace TurquoiseHealth.Api;

/// <summary>
/// MemberCostShare
/// </summary>
[Serializable]
public record ConsumerSiteMemberCostShare : IJsonOnDeserialized
{
    [JsonExtensionData]
    private readonly IDictionary<string, JsonElement> _extensionData =
        new Dictionary<string, JsonElement>();

    /// <summary>
    /// Total out-of-pocket price.
    /// </summary>
    [JsonPropertyName("total")]
    public required ConsumerSiteMoney Total { get; set; }

    [JsonPropertyName("amount_towards_deductible")]
    public required ConsumerSiteMoney AmountTowardsDeductible { get; set; }

    [JsonPropertyName("amount_towards_copayment")]
    public required ConsumerSiteMoney AmountTowardsCopayment { get; set; }

    [JsonPropertyName("amount_towards_coinsurance")]
    public required ConsumerSiteMoney AmountTowardsCoinsurance { get; set; }

    /// <summary>
    /// Whether the deductible is fully met at/after this service.
    /// </summary>
    [JsonPropertyName("is_deductible_met")]
    public required bool IsDeductibleMet { get; set; }

    /// <summary>
    /// Whether the OOP maximum is met at/after this service.
    /// </summary>
    [JsonPropertyName("is_out_of_pocket_max_met")]
    public required bool IsOutOfPocketMaxMet { get; set; }

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
