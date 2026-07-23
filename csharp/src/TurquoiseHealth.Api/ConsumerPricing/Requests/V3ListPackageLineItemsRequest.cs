using System.Text.Json.Serialization;
using TurquoiseHealth.Api.Core;

namespace TurquoiseHealth.Api;

[Serializable]
public record V3ListPackageLineItemsRequest
{
    /// <summary>
    /// Package identifier.
    /// </summary>
    [JsonIgnore]
    public required string PackageId { get; set; }

    /// <inheritdoc />
    public override string ToString()
    {
        return JsonUtils.Serialize(this);
    }
}
