using System.Text.Json;
using System.Text.Json.Serialization;
using TurquoiseHealth.Api.Core;

namespace TurquoiseHealth.Api;

[Serializable]
public record ConsumerSiteBenefitsSummary : IJsonOnDeserialized
{
    [JsonExtensionData]
    private readonly IDictionary<string, JsonElement> _extensionData =
        new Dictionary<string, JsonElement>();

    /// <summary>
    /// Remaining deductible depends on accumulator type:
    ///
    /// - **Embedded**: Both individual and family deductibles exist. Met when either is exceeded; uses whichever remaining amount (individual or family) is smaller.
    /// - **Aggregate**: Only a family deductible exists. Met only when the family accumulator is exceeded; uses remaining family deductible.
    /// - **Individual**: No family accumulator exists. Met once individual accumulator is reached; uses the remaining individual deductible.
    /// - **Zero**: The deductible is $0. Coverage kicks in immediately.
    ///
    /// `remaining_deductible` and `total_deductible` use the same accumulator value (individual or family)
    /// </summary>
    [JsonPropertyName("remaining_deductible")]
    public required double RemainingDeductible { get; set; }

    /// <summary>
    /// Total deductible depends on accumulator type:
    ///
    /// - **Embedded**: Both individual and family deductibles exist. Met when either is exceeded.
    /// - **Aggregate**: Only a family deductible exists. Met only when the family accumulator is exceeded.
    /// - **Individual**: No family accumulator exists. Met once individual accumulator is reached.
    /// - **Zero**: The deductible is $0. Coverage kicks in immediately.
    ///
    /// `total_deductible` and `remaining_deductible` use the same accumulator value (individual or family)
    /// </summary>
    [JsonPropertyName("total_deductible")]
    public double? TotalDeductible { get; set; }

    /// <summary>
    /// Remaining out-of-pocket max depends on accumulator type:
    ///
    /// - **Embedded**: Both individual and family out-of-pocket maxes exist. Met when either is exceeded, using whichever remaining amount is lower.
    /// - **Aggregate**: Only a family out-of-pocket max exists. Met when the family accumulator is exceeded.
    /// - **Individual**: No family accumulator exists. Met once individual accumulator is reached.
    /// - **Zero**: The out-of-pocket max is $0.
    ///
    /// `remaining_out_of_pocket_max` and `total_out_of_pocket_max` use the same accumulator value (individual or family)
    /// </summary>
    [JsonPropertyName("remaining_out_of_pocket_max")]
    public double? RemainingOutOfPocketMax { get; set; }

    /// <summary>
    /// Total out-of-pocket max depends on accumulator type:
    ///
    /// - **Embedded**: Both individual and family out-of-pocket maxes exist. Met when either is exceeded, using whichever remaining amount is lower.
    /// - **Aggregate**: Only a family out-of-pocket max exists. Met when the family accumulator is exceeded.
    /// - **Individual**: No family accumulator exists. Met once individual accumulator is reached.
    /// - **Zero**: The out-of-pocket max is $0.
    ///
    /// `total_out_of_pocket_max` and `remaining_out_of_pocket_max` use the same accumulator value (individual or family)
    /// </summary>
    [JsonPropertyName("total_out_of_pocket_max")]
    public double? TotalOutOfPocketMax { get; set; }

    [JsonPropertyName("benefit_categories")]
    public IEnumerable<ConsumerSiteBenefitCategory> BenefitCategories { get; set; } =
        new List<ConsumerSiteBenefitCategory>();

    [JsonPropertyName("limitations")]
    public IEnumerable<string> Limitations { get; set; } = new List<string>();

    [JsonPropertyName("deductible_accumulator_type")]
    public required string DeductibleAccumulatorType { get; set; }

    [JsonPropertyName("out_of_pocket_accumulator_type")]
    public required string OutOfPocketAccumulatorType { get; set; }

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
