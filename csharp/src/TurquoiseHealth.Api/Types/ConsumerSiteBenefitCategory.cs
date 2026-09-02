using System.Text.Json;
using System.Text.Json.Serialization;
using TurquoiseHealth.Api.Core;

namespace TurquoiseHealth.Api;

[Serializable]
public record ConsumerSiteBenefitCategory : IJsonOnDeserialized
{
    [JsonExtensionData]
    private readonly IDictionary<string, JsonElement> _extensionData =
        new Dictionary<string, JsonElement>();

    [JsonPropertyName("category")]
    public required string Category { get; set; }

    [JsonPropertyName("coinsurance")]
    public required double Coinsurance { get; set; }

    [JsonPropertyName("copayment")]
    public required double Copayment { get; set; }

    /// <summary>
    /// `0` means deductible does not apply. `null` means plan deductible applies
    /// </summary>
    [JsonPropertyName("deductible")]
    public double? Deductible { get; set; }

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
