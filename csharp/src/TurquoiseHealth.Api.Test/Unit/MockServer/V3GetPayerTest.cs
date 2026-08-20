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
              "id": "76",
              "name": "Cigna",
              "context": {
                "score": 1.1
              }
            }
            """;

        Server
            .Given(WireMock.RequestBuilders.Request.Create().WithPath("/v3/payers/76").UsingGet())
            .RespondWith(
                WireMock
                    .ResponseBuilders.Response.Create()
                    .WithStatusCode(200)
                    .WithBody(mockResponse)
            );

        var response = await Client.ConsumerPricing.V3GetPayerAsync(
            new V3GetPayerRequest { PayerId = "76" }
        );
        Assert.That(
            response,
            Is.EqualTo(JsonUtils.Deserialize<Payer>(mockResponse)).UsingDefaults()
        );
    }
}
