using System.Text.Json;
using System.Text.Json.Serialization;
using TurquoiseHealth.Api.Core;

namespace TurquoiseHealth.Api;

[Serializable]
public record InsuranceNetwork : IJsonOnDeserialized
{
    [JsonExtensionData]
    private readonly IDictionary<string, JsonElement> _extensionData =
        new Dictionary<string, JsonElement>();

    /// <summary>
    /// Insurance network identifier
    /// </summary>
    [JsonPropertyName("network_id")]
    public required string NetworkId { get; set; }

    /// <summary>
    /// Insurance network name
    /// </summary>
    [JsonPropertyName("network_name")]
    public required string NetworkName { get; set; }

    /// <summary>
    /// Insurance payer name
    /// </summary>
    [JsonPropertyName("payer_name")]
    public required string PayerName { get; set; }

    /// <summary>
    /// Insurance payer identifier
    /// </summary>
    [JsonPropertyName("payer_id")]
    public required string PayerId { get; set; }

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
