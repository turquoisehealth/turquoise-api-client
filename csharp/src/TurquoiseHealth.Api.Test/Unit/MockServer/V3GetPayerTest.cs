using NUnit.Framework;
using TurquoiseHealth.Api;
using TurquoiseHealth.Api.Core;

namespace TurquoiseHealth.Api.Test.Unit.MockServer;

[TestFixture]
public class V3GetPayerTest : BaseMockServerTest
{
    [NUnit.Framework.Test]
    public async Task MockServerTest()
    {
        const string mockResponse = """
            {
              "object": "payer",
              "id": "7001",
              "name": "Meridian Health Plan",
              "context": {
                "score": 1.1,
                "distance_m": 1.1
              }
            }
            """;

        Server
            .Given(WireMock.RequestBuilders.Request.Create().WithPath("/v3/payers/7001").UsingGet())
            .RespondWith(
                WireMock
                    .ResponseBuilders.Response.Create()
                    .WithStatusCode(200)
                    .WithBody(mockResponse)
            );

        var response = await Client.ConsumerPricing.V3GetPayerAsync(
            new V3GetPayerRequest { PayerId = "7001" }
        );
        Assert.That(
            response,
            Is.EqualTo(JsonUtils.Deserialize<V3Payer>(mockResponse)).UsingDefaults()
        );
    }
}
