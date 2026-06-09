using System.Text.Json.Serialization;
using TurquoiseHealth.Api.Core;

namespace TurquoiseHealth.Api;

[Serializable]
public record V2GetSspRequest
{
    /// <summary>
    /// SSP identifier.
    /// </summary>
    [JsonIgnore]
    public required string SspId { get; set; }

    /// <inheritdoc />
    public override string ToString()
    {
        return JsonUtils.Serialize(this);
    }
}
