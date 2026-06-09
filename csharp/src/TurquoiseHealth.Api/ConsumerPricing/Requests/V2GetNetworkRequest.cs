using System.Text.Json.Serialization;
using TurquoiseHealth.Api.Core;

namespace TurquoiseHealth.Api;

[Serializable]
public record V2GetNetworkRequest
{
    /// <summary>
    /// Network identifier (Int64String on the wire).
    /// </summary>
    [JsonIgnore]
    public required string NetworkId { get; set; }

    /// <inheritdoc />
    public override string ToString()
    {
        return JsonUtils.Serialize(this);
    }
}
