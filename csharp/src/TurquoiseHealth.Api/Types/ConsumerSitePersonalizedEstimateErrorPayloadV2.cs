using System.Text.Json;
using System.Text.Json.Serialization;
using TurquoiseHealth.Api.Core;

namespace TurquoiseHealth.Api;

/// <summary>
/// Details about an error returned by a v2 personalized-estimate endpoint.
/// </summary>
[Serializable]
public record ConsumerSitePersonalizedEstimateErrorPayloadV2 : IJsonOnDeserialized
{
    [JsonExtensionData]
    private readonly IDictionary<string, JsonElement> _extensionData =
        new Dictionary<string, JsonElement>();

    /// <summary>
    /// Stable, documented error code. Switch on this in code.
    /// </summary>
    [JsonPropertyName("code")]
    public required ConsumerSitePersonalizedEstimateErrorCodeV2 Code { get; set; }

    /// <summary>
    /// Human-readable error summary, suitable for narration to an end user.
    /// </summary>
    [JsonPropertyName("message")]
    public required string Message { get; set; }

    /// <summary>
    /// Actionable suggestion for the caller, when applicable.
    /// </summary>
    [JsonPropertyName("hint")]
    public string? Hint { get; set; }

    /// <summary>
    /// What the API expected (format, value range, etc).
    /// </summary>
    [JsonPropertyName("expected")]
    public string? Expected { get; set; }

    /// <summary>
    /// What the API actually received.
    /// </summary>
    [JsonPropertyName("received")]
    public string? Received { get; set; }

    /// <summary>
    /// Dot-path of the offending request field, when applicable.
    /// </summary>
    [JsonPropertyName("field")]
    public string? Field { get; set; }

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
