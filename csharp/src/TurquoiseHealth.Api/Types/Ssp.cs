using System.Text.Json;
using System.Text.Json.Serialization;
using TurquoiseHealth.Api.Core;

namespace TurquoiseHealth.Api;

/// <summary>
/// Standard Service Package row.
///
/// Sparse by default. Component billing codes and pricing relationships are
/// reachable via the dedicated rate endpoints.
/// </summary>
[Serializable]
public record Ssp : IJsonOnDeserialized
{
    [JsonExtensionData]
    private readonly IDictionary<string, JsonElement> _extensionData =
        new Dictionary<string, JsonElement>();

    /// <summary>
    /// SSP identifier. Upstream-derived from the dataset and may change as the dataset is rebuilt. Do not bake into URLs, bookmarks, or persistent storage.
    /// </summary>
    [JsonPropertyName("id")]
    public required string Id { get; set; }

    /// <summary>
    /// Display name of the service package.
    /// </summary>
    [JsonPropertyName("name")]
    public required string Name { get; set; }

    /// <summary>
    /// Patient-facing description of the service package.
    /// </summary>
    [JsonPropertyName("patient_description")]
    public string? PatientDescription { get; set; }

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
