using System.Text.Json.Serialization;
using TurquoiseHealth.Api.Core;

namespace TurquoiseHealth.Api;

[Serializable]
public record V3GetPriceRequest
{
    /// <summary>
    /// Price identifier.
    /// </summary>
    [JsonIgnore]
    public required string PriceId { get; set; }

    /// <summary>
    /// Relations to inline. Repeat the param to request several.
    /// </summary>
    [JsonIgnore]
    public IEnumerable<PriceExpand>? Expand { get; set; }

    /// <inheritdoc />
    public override string ToString()
    {
        return JsonUtils.Serialize(this);
    }
}
