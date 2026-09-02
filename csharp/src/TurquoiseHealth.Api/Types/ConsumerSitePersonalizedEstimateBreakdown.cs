using System.Text.Json;
using System.Text.Json.Serialization;
using TurquoiseHealth.Api.Core;

namespace TurquoiseHealth.Api;

/// <summary>
/// Member out-of-pocket breakdown for one (provider x ssp x network) tuple  Totals are member's expected out-of-pocket for the winning sub-package. line_items reuse the provider-breakdown representation.
/// </summary>
[Serializable]
public record ConsumerSitePersonalizedEstimateBreakdown : IJsonOnDeserialized
{
    [JsonExtensionData]
    private readonly IDictionary<string, JsonElement> _extensionData =
        new Dictionary<string, JsonElement>();

    [JsonPropertyName("price_type")]
    public required string PriceType { get; set; }

    [JsonPropertyName("provider_id")]
    public required string ProviderId { get; set; }

    [JsonPropertyName("ssp_id")]
    public required string SspId { get; set; }

    /// <summary>
    /// Signed 64-bit integer serialized as a string to preserve precision in JSON.
    /// </summary>
    [JsonPropertyName("network_id")]
    public string? NetworkId { get; set; }

    /// <summary>
    /// ISO currency code. Always USD today.
    /// </summary>
    [JsonPropertyName("currency")]
    public string? Currency { get; set; }

    /// <summary>
    /// Member out-of-pocket total for the winning sub-package.
    /// </summary>
    [JsonPropertyName("amount")]
    public required string Amount { get; set; }

    /// <summary>
    /// Integer minor units of `amount`.
    /// </summary>
    [JsonPropertyName("amount_cents")]
    public required int AmountCents { get; set; }

    /// <summary>
    /// Portion of the out-of-pocket applied to the deductible.
    /// </summary>
    [JsonPropertyName("amount_towards_deductible")]
    public required string AmountTowardsDeductible { get; set; }

    /// <summary>
    /// Integer minor units of `amount_towards_deductible`.
    /// </summary>
    [JsonPropertyName("amount_towards_deductible_cents")]
    public required int AmountTowardsDeductibleCents { get; set; }

    /// <summary>
    /// Portion of the out-of-pocket that is a flat copay.
    /// </summary>
    [JsonPropertyName("amount_towards_copayment")]
    public required string AmountTowardsCopayment { get; set; }

    /// <summary>
    /// Integer minor units of `amount_towards_copayment`.
    /// </summary>
    [JsonPropertyName("amount_towards_copayment_cents")]
    public required int AmountTowardsCopaymentCents { get; set; }

    /// <summary>
    /// Portion of the out-of-pocket that is coinsurance.
    /// </summary>
    [JsonPropertyName("amount_towards_coinsurance")]
    public required string AmountTowardsCoinsurance { get; set; }

    /// <summary>
    /// Integer minor units of `amount_towards_coinsurance`.
    /// </summary>
    [JsonPropertyName("amount_towards_coinsurance_cents")]
    public required int AmountTowardsCoinsuranceCents { get; set; }

    /// <summary>
    /// Whether the member's deductible is fully met by/at this estimate.
    /// </summary>
    [JsonPropertyName("is_deductible_met")]
    public required bool IsDeductibleMet { get; set; }

    /// <summary>
    /// Whether the member's out-of-pocket max is met by/at this estimate.
    /// </summary>
    [JsonPropertyName("is_out_of_pocket_met")]
    public required bool IsOutOfPocketMet { get; set; }

    /// <summary>
    /// Full pre-insurance negotiated price for context.
    /// </summary>
    [JsonPropertyName("total_negotiated_price")]
    public required string TotalNegotiatedPrice { get; set; }

    /// <summary>
    /// Integer minor units of `total_negotiated_price`.
    /// </summary>
    [JsonPropertyName("total_negotiated_price_cents")]
    public required int TotalNegotiatedPriceCents { get; set; }

    /// <summary>
    /// The winning sub-package the breakdown was computed over.
    /// </summary>
    [JsonPropertyName("sub_package_code")]
    public required string SubPackageCode { get; set; }

    [JsonPropertyName("line_items")]
    public IEnumerable<ConsumerSiteRateBreakdownLineItem> LineItems { get; set; } =
        new List<ConsumerSiteRateBreakdownLineItem>();

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
