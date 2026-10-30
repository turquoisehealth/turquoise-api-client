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
              "id": "5754",
              "name": "HCA HealthONE Presbyterian St Lukes",
              "type": "Short Term Acute Care Hospital",
              "npi": "1720038946",
              "address": {
                "city": "Denver",
                "state": "CO",
                "zip_code": "80218",
                "latitude": 39.746549,
                "longitude": -104.966925
              },
              "context": {
                "score": 1.1,
                "distance_m": 1738.3
              }
            }
            """;

        Server
            .Given(
                WireMock.RequestBuilders.Request.Create().WithPath("/v3/providers/5754").UsingGet()
            )
            .RespondWith(
                WireMock
                    .ResponseBuilders.Response.Create()
                    .WithStatusCode(200)
                    .WithBody(mockResponse)
            );

        var response = await Client.ConsumerPricing.V3GetProviderAsync(
            new V3GetProviderRequest { ProviderId = "5754" }
        );
        Assert.That(
            response,
            Is.EqualTo(JsonUtils.Deserialize<ConsumerPricingProvider>(mockResponse)).UsingDefaults()
        );
    }
}
