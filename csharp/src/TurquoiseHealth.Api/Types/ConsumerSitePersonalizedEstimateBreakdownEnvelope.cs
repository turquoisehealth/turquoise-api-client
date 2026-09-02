using System.Text.Json;
using System.Text.Json.Serialization;
using TurquoiseHealth.Api.Core;

namespace TurquoiseHealth.Api;

/// <summary>
/// Named envelope for shorter PersonalizedEstimateBreakdownEnvelope
/// </summary>
[Serializable]
public record ConsumerSitePersonalizedEstimateBreakdownEnvelope : IJsonOnDeserialized
{
    [JsonExtensionData]
    private readonly IDictionary<string, JsonElement> _extensionData =
        new Dictionary<string, JsonElement>();

    [JsonPropertyName("data")]
    public ConsumerSitePersonalizedEstimateBreakdown? Data { get; set; }

    [JsonPropertyName("no_data_reason")]
    public ConsumerSiteNoDataReason? NoDataReason { get; set; }

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
