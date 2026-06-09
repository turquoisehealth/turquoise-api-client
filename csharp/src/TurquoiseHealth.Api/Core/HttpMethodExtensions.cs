using System.Net.Http;

namespace TurquoiseHealth.Api.Core;

internal static class HttpMethodExtensions
{
    public static readonly HttpMethod Patch = new("PATCH");
}
