using System.Text.Json;
using System.Text.Json.Serialization;
using TurquoiseHealth.Api.Core;

namespace TurquoiseHealth.Api;

[Serializable]
public record ProviderLocationDetails : IJsonOnDeserialized
{
    [JsonExtensionData]
    private readonly IDictionary<string, JsonElement> _extensionData =
        new Dictionary<string, JsonElement>();

    /// <summary>
    /// Provider street address
    /// </summary>
    [JsonPropertyName("street_address")]
    public required string StreetAddress { get; set; }

    /// <summary>
    /// Provider latitude
    /// </summary>
    [JsonPropertyName("latitude")]
    public double? Latitude { get; set; }

    /// <summary>
    /// Provider longitude
    /// </summary>
    [JsonPropertyName("longitude")]
    public double? Longitude { get; set; }

    /// <summary>
    /// Distance from the search location in meters
    /// </summary>
    [JsonPropertyName("distance_in_meters")]
    public double? DistanceInMeters { get; set; }

    /// <summary>
    /// CMS overall hospital quality rating (1-5)
    /// </summary>
    [JsonPropertyName("hospital_overall_rating")]
    public int? HospitalOverallRating { get; set; }

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
