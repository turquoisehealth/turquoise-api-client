using System.Text.Json;
using System.Text.Json.Serialization;
using TurquoiseHealth.Api.Core;

namespace TurquoiseHealth.Api;

/// <summary>
/// The single money shape used everywhere a monetary value appears.
/// </summary>
[Serializable]
public record V3Money : IJsonOnDeserialized
{
    [JsonExtensionData]
    private readonly IDictionary<string, JsonElement> _extensionData =
        new Dictionary<string, JsonElement>();

    /// <summary>
    /// String-decimal value, e.g. "1250.00".
    /// </summary>
    [JsonPropertyName("amount")]
    public required string Amount { get; set; }

    /// <summary>
    /// Integer minor units, e.g. 125000.
    /// </summary>
    [JsonPropertyName("minor_units")]
    public required int MinorUnits { get; set; }

    /// <summary>
    /// ISO currency code. Always USD today.
    /// </summary>
    [JsonPropertyName("currency")]
    public string? Currency { get; set; }

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
