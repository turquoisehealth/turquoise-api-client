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
              "id": "5756",
              "name": "Intermountain Health Saint Joseph Hospital",
              "type": "Short Term Acute Care Hospital",
              "npi": "1417946021",
              "address": {
                "city": "Denver",
                "state": "CO",
                "zip_code": "80218",
                "latitude": 39.745961,
                "longitude": -104.971559
              },
              "context": {
                "score": 1.1,
                "distance_m": 1644
              }
            }
            """;

        Server
            .Given(
                WireMock.RequestBuilders.Request.Create().WithPath("/v3/providers/5756").UsingGet()
            )
            .RespondWith(
                WireMock
                    .ResponseBuilders.Response.Create()
                    .WithStatusCode(200)
                    .WithBody(mockResponse)
            );

        var response = await Client.ConsumerPricing.V3GetProviderAsync(
            new V3GetProviderRequest { ProviderId = "5756" }
        );
        Assert.That(
            response,
            Is.EqualTo(JsonUtils.Deserialize<Provider>(mockResponse)).UsingDefaults()
        );
    }
}
