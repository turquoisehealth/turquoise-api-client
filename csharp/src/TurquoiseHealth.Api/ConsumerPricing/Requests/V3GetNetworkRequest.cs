using System.Text.Json.Serialization;
using TurquoiseHealth.Api.Core;

namespace TurquoiseHealth.Api;

[Serializable]
public record V3GetNetworkRequest
{
    /// <summary>
    /// Network identifier (string-wrapped 64-bit integer).
    /// </summary>
    [JsonIgnore]
    public required string NetworkId { get; set; }

    /// <inheritdoc />
    public override string ToString()
    {
        return JsonUtils.Serialize(this);
    }
}
