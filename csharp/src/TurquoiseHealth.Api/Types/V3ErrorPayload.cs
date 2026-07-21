using System.Text.Json;
using System.Text.Json.Serialization;
using TurquoiseHealth.Api.Core;

namespace TurquoiseHealth.Api;

[Serializable]
public record V3ErrorPayload : IJsonOnDeserialized
{
    [JsonExtensionData]
    private readonly IDictionary<string, JsonElement> _extensionData =
        new Dictionary<string, JsonElement>();

    /// <summary>
    /// Stable, documented error code.
    /// </summary>
    [JsonPropertyName("code")]
    public required V3ErrorCode Code { get; set; }

    /// <summary>
    /// Human-readable error summary.
    /// </summary>
    [JsonPropertyName("message")]
    public required string Message { get; set; }

    /// <summary>
    /// Actionable suggestion for the caller, when applicable.
    /// </summary>
    [JsonPropertyName("hint")]
    public string? Hint { get; set; }

    /// <summary>
    /// What the API expected.
    /// </summary>
    [JsonPropertyName("expected")]
    public string? Expected { get; set; }

    /// <summary>
    /// What the API actually received.
    /// </summary>
    [JsonPropertyName("received")]
    public string? Received { get; set; }

    /// <summary>
    /// Dot-path of the offending request field.
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
