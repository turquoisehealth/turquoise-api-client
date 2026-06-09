using System.Text.Json;
using System.Text.Json.Serialization;
using TurquoiseHealth.Api.Core;

namespace TurquoiseHealth.Api;

/// <summary>
/// Provider list/detail row.
/// </summary>
[Serializable]
public record Provider : IJsonOnDeserialized
{
    [JsonExtensionData]
    private readonly IDictionary<string, JsonElement> _extensionData =
        new Dictionary<string, JsonElement>();

    /// <summary>
    /// Provider identifier. Upstream-derived from the dataset and may change as the dataset is rebuilt. Do not bake into URLs, bookmarks, or persistent storage.
    /// </summary>
    [JsonPropertyName("id")]
    public required string Id { get; set; }

    /// <summary>
    /// Provider facility or organization name.
    /// </summary>
    [JsonPropertyName("name")]
    public required string Name { get; set; }

    /// <summary>
    /// Provider type (hospital, ASC, imaging center, etc).
    /// </summary>
    [JsonPropertyName("type")]
    public string? Type { get; set; }

    /// <summary>
    /// National Provider Identifier.
    /// </summary>
    [JsonPropertyName("npi")]
    public string? Npi { get; set; }

    /// <summary>
    /// City the provider is located in.
    /// </summary>
    [JsonPropertyName("city")]
    public required string City { get; set; }

    /// <summary>
    /// Two-letter USPS state code.
    /// </summary>
    [JsonPropertyName("state")]
    public required string State { get; set; }

    /// <summary>
    /// Provider ZIP code.
    /// </summary>
    [JsonPropertyName("zip_code")]
    public required string ZipCode { get; set; }

    /// <summary>
    /// Distance from the search anchor in meters. Null when no `near`/`zip_anchor` location was supplied.
    /// </summary>
    [JsonPropertyName("distance_m")]
    public double? DistanceM { get; set; }

    /// <summary>
    /// Provider HQ latitude.
    /// </summary>
    [JsonPropertyName("latitude")]
    public double? Latitude { get; set; }

    /// <summary>
    /// Provider HQ longitude.
    /// </summary>
    [JsonPropertyName("longitude")]
    public double? Longitude { get; set; }

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
