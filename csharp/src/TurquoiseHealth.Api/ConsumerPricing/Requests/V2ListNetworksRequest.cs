using System.Text.Json.Serialization;
using TurquoiseHealth.Api.Core;

namespace TurquoiseHealth.Api;

[Serializable]
public record V2ListNetworksRequest
{
    /// <summary>
    /// Case-insensitive substring match on network or payer name.
    /// </summary>
    [JsonIgnore]
    public string? Name { get; set; }

    /// <summary>
    /// Filter to networks under this payer.
    /// </summary>
    [JsonIgnore]
    public string? PayerId { get; set; }

    /// <summary>
    /// Page size.
    /// </summary>
    [JsonIgnore]
    public int? PageSize { get; set; }

    /// <summary>
    /// Opaque cursor from a previous response's page.next_cursor.
    /// </summary>
    [JsonIgnore]
    public string? Cursor { get; set; }

    /// <inheritdoc />
    public override string ToString()
    {
        return JsonUtils.Serialize(this);
    }
}
