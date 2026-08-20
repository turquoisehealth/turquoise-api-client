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
              "id": "-3776001016975145508",
              "name": "National OAP",
              "payer": {
                "id": "76",
                "name": "Cigna"
              },
              "context": {
                "score": 1.1
              }
            }
            """;

        Server
            .Given(
                WireMock
                    .RequestBuilders.Request.Create()
                    .WithPath("/v3/networks/-3776001016975145508")
                    .UsingGet()
            )
            .RespondWith(
                WireMock
                    .ResponseBuilders.Response.Create()
                    .WithStatusCode(200)
                    .WithBody(mockResponse)
            );

        var response = await Client.ConsumerPricing.V3GetNetworkAsync(
            new V3GetNetworkRequest { NetworkId = "-3776001016975145508" }
        );
        Assert.That(
            response,
            Is.EqualTo(JsonUtils.Deserialize<Network>(mockResponse)).UsingDefaults()
        );
    }
}
