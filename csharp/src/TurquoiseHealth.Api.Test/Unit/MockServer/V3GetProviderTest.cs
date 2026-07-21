using NUnit.Framework;
using TurquoiseHealth.Api;
using TurquoiseHealth.Api.Core;

namespace TurquoiseHealth.Api.Test.Unit.MockServer;

[TestFixture]
public class V3GetProviderTest : BaseMockServerTest
{
    [NUnit.Framework.Test]
    public async Task MockServerTest()
    {
        const string mockResponse = """
            {
              "object": "provider",
              "id": "2743",
              "name": "Loretto Hospital",
              "type": "Short Term Acute Care Hospital",
              "npi": "1447280284",
              "address": {
                "city": "Chicago",
                "state": "IL",
                "zip_code": "60644",
                "latitude": 41.8721272,
                "longitude": -87.7636486
              },
              "context": {
                "score": 1.1,
                "distance_m": 1240
              }
            }
            """;

        Server
            .Given(
                WireMock.RequestBuilders.Request.Create().WithPath("/v3/providers/2743").UsingGet()
            )
            .RespondWith(
                WireMock
                    .ResponseBuilders.Response.Create()
                    .WithStatusCode(200)
                    .WithBody(mockResponse)
            );

        var response = await Client.ConsumerPricing.V3GetProviderAsync(
            new V3GetProviderRequest { ProviderId = "2743" }
        );
        Assert.That(
            response,
            Is.EqualTo(JsonUtils.Deserialize<V3Provider>(mockResponse)).UsingDefaults()
        );
    }
}
