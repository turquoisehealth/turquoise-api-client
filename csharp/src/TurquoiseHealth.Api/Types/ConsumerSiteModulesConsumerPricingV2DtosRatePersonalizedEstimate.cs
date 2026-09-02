using System.Text.Json;
using System.Text.Json.Serialization;
using TurquoiseHealth.Api.Core;

namespace TurquoiseHealth.Api;

/// <summary>
/// Member out-of-pocket estimate at the (provider × ssp × [network]) grain.  Inherits `Rate`; the inherited `amount`/`amount_cents` carry the member's cost-share total (out-of-pocket), not the full negotiated price. The breakdown fields explain how that total is composed.
/// </summary>
[Serializable]
public record ConsumerSiteModulesConsumerPricingV2DtosRatePersonalizedEstimate : IJsonOnDeserialized
{
    [JsonExtensionData]
    private readonly IDictionary<string, JsonElement> _extensionData =
        new Dictionary<string, JsonElement>();

    /// <summary>
    /// `cash` ⇔ `network_id is null`. `negotiated` otherwise.
    /// </summary>
    [JsonPropertyName("price_type")]
    public required string PriceType { get; set; }

    /// <summary>
    /// Provider identifier. Upstream-derived from the dataset and may change as the dataset is rebuilt. Do not bake into URLs, bookmarks, or persistent storage.
    /// </summary>
    [JsonPropertyName("provider_id")]
    public required string ProviderId { get; set; }

    /// <summary>
    /// SSP identifier. Upstream-derived from the dataset and may change as the dataset is rebuilt. Do not bake into URLs, bookmarks, or persistent storage.
    /// </summary>
    [JsonPropertyName("ssp_id")]
    public required string SspId { get; set; }

    /// <summary>
    /// Signed 64-bit integer serialized as a string to preserve precision in JSON.
    /// </summary>
    [JsonPropertyName("network_id")]
    public string? NetworkId { get; set; }

    /// <summary>
    /// String-decimal money value, e.g. "1250.00".
    /// </summary>
    [JsonPropertyName("amount")]
    public required string Amount { get; set; }

    /// <summary>
    /// Integer minor units, e.g. 125000.
    /// </summary>
    [JsonPropertyName("amount_cents")]
    public required int AmountCents { get; set; }

    /// <summary>
    /// ISO currency code. Always USD today.
    /// </summary>
    [JsonPropertyName("currency")]
    public string? Currency { get; set; }

    /// <summary>
    /// Portion of the estimate applied to the deductible.
    /// </summary>
    [JsonPropertyName("amount_towards_deductible")]
    public required string AmountTowardsDeductible { get; set; }

    /// <summary>
    /// Integer minor units of `amount_towards_deductible`.
    /// </summary>
    [JsonPropertyName("amount_towards_deductible_cents")]
    public required int AmountTowardsDeductibleCents { get; set; }

    /// <summary>
    /// Portion of the estimate that is a flat copay.
    /// </summary>
    [JsonPropertyName("amount_towards_copayment")]
    public required string AmountTowardsCopayment { get; set; }

    /// <summary>
    /// Integer minor units of `amount_towards_copayment`.
    /// </summary>
    [JsonPropertyName("amount_towards_copayment_cents")]
    public required int AmountTowardsCopaymentCents { get; set; }

    /// <summary>
    /// Portion of the estimate that is coinsurance.
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
    /// The sub-package the estimate was computed over.
    /// </summary>
    [JsonPropertyName("sub_package_code")]
    public required string SubPackageCode { get; set; }

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
