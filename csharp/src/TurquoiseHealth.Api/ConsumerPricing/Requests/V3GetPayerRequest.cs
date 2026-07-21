using System.Text.Json.Serialization;
using TurquoiseHealth.Api.Core;

namespace TurquoiseHealth.Api;

[Serializable]
public record V3GetPayerRequest
{
    /// <summary>
    /// Payer identifier.
    /// </summary>
    [JsonIgnore]
    public required string PayerId { get; set; }

    /// <inheritdoc />
    public override string ToString()
    {
        return JsonUtils.Serialize(this);
    }
}
