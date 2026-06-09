using System.Text.Json;
using System.Text.Json.Serialization;
using OneOf;
using TurquoiseHealth.Api.Core;

namespace TurquoiseHealth.Api;

/// <summary>
/// Care-navigation (v1) error envelope.
///
/// Named `CareNavErrorResponse` (not `ErrorResponse`) so it doesn't collide
/// with `modules.external.consumer_pricing_v2.dtos.common.ErrorResponse` in
/// the shared external OpenAPI export. The two surfaces have different error
/// wire formats (v1 is flat; v2 wraps in `{ "error": {...} }`).
/// </summary>
[Serializable]
public record CareNavErrorResponse : IJsonOnDeserialized
{
    [JsonExtensionData]
    private readonly IDictionary<string, JsonElement> _extensionData =
        new Dictionary<string, JsonElement>();

    /// <summary>
    /// Machine-readable error code
    /// </summary>
    [JsonPropertyName("code")]
    public string? Code { get; set; }

    /// <summary>
    /// Human-readable error summary
    /// </summary>
    [JsonPropertyName("message")]
    public string? Message { get; set; }

    /// <summary>
    /// FastAPI validation error detail
    /// </summary>
    [JsonPropertyName("detail")]
    public OneOf<IEnumerable<object>, string>? Detail { get; set; }

    /// <summary>
    /// Field-level error details, if applicable
    /// </summary>
    [JsonPropertyName("details")]
    public IEnumerable<ErrorDetail>? Details { get; set; }

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
