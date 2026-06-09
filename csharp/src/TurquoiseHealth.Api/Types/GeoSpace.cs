using System.Text.Json;
using System.Text.Json.Serialization;
using TurquoiseHealth.Api.Core;

namespace TurquoiseHealth.Api;

[Serializable]
public record GeoSpace : IJsonOnDeserialized
{
    [JsonExtensionData]
    private readonly IDictionary<string, JsonElement> _extensionData =
        new Dictionary<string, JsonElement>();

    /// <summary>
    /// ZIP codes to anchor location search
    /// </summary>
    [JsonPropertyName("zip_codes")]
    public IEnumerable<string>? ZipCodes { get; set; }

    /// <summary>
    /// Core-Based Statistical Area identifier
    /// </summary>
    [JsonPropertyName("cbsa_id")]
    public string? CbsaId { get; set; }

    /// <summary>
    /// Two-letter USPS abbreviation
    /// </summary>
    [JsonPropertyName("state")]
    public string? State { get; set; }

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
