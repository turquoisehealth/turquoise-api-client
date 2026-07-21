using System.Text.Json.Serialization;
using TurquoiseHealth.Api.Core;

namespace TurquoiseHealth.Api;

[Serializable]
public record V3GetProviderRequest
{
    /// <summary>
    /// Provider identifier.
    /// </summary>
    [JsonIgnore]
    public required string ProviderId { get; set; }

    /// <inheritdoc />
    public override string ToString()
    {
        return JsonUtils.Serialize(this);
    }
}
