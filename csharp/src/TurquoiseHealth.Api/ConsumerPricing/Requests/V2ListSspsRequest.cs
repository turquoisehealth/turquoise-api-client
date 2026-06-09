using System.Text.Json.Serialization;
using TurquoiseHealth.Api.Core;

namespace TurquoiseHealth.Api;

[Serializable]
public record V2ListSspsRequest
{
    /// <summary>
    /// Case-insensitive substring match on SSP name.
    /// </summary>
    [JsonIgnore]
    public string? Name { get; set; }

    /// <summary>
    /// Case-insensitive substring match on SSP patient description.
    /// </summary>
    [JsonIgnore]
    public string? Description { get; set; }

    /// <summary>
    /// Case-insensitive substring match across name OR patient description.
    /// </summary>
    [JsonIgnore]
    public string? Search { get; set; }

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
