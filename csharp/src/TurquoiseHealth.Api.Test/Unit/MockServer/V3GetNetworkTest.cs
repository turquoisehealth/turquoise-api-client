using NUnit.Framework;
using TurquoiseHealth.Api;
using TurquoiseHealth.Api.Core;

namespace TurquoiseHealth.Api.Test.Unit.MockServer;

[TestFixture]
public class V3GetNetworkTest : BaseMockServerTest
{
    [NUnit.Framework.Test]
    public async Task MockServerTest()
    {
        const string mockResponse = """
            {
              "object": "network",
              "id": "2010265101",
              "name": "National PPO",
              "payer": {
                "id": "7001",
                "name": "Meridian Health Plan"
              },
              "context": {
                "score": 1.1,
                "distance_m": 1.1
              }
            }
            """;

        Server
            .Given(
                WireMock
                    .RequestBuilders.Request.Create()
                    .WithPath("/v3/networks/2010265101")
                    .UsingGet()
            )
            .RespondWith(
                WireMock
                    .ResponseBuilders.Response.Create()
                    .WithStatusCode(200)
                    .WithBody(mockResponse)
            );

        var response = await Client.ConsumerPricing.V3GetNetworkAsync(
            new V3GetNetworkRequest { NetworkId = "2010265101" }
        );
        Assert.That(
            response,
            Is.EqualTo(JsonUtils.Deserialize<V3Network>(mockResponse)).UsingDefaults()
        );
    }
}
