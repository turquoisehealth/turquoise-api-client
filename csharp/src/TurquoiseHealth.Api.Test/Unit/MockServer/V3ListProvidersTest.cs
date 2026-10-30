using NUnit.Framework;
using TurquoiseHealth.Api;
using TurquoiseHealth.Api.Core;

namespace TurquoiseHealth.Api.Test.Unit.MockServer;

[TestFixture]
public class V3ListProvidersTest : BaseMockServerTest
{
    [NUnit.Framework.Test]
    public async Task MockServerTest()
    {
        const string mockResponse = """
            {
              "object": "list",
              "items": [
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
                    "distance_m": 1738.3
                  }
                }
              ],
              "page": {
                "size": 1,
                "total": 1,
                "next_cursor": "next_cursor"
              },
              "no_data_reason": "no_data",
              "disclosures": [
                "disclosures"
              ]
            }
            """;

        Server
            .Given(WireMock.RequestBuilders.Request.Create().WithPath("/v3/providers").UsingGet())
            .RespondWith(
                WireMock
                    .ResponseBuilders.Response.Create()
                    .WithStatusCode(200)
                    .WithBody(mockResponse)
            );

        var response = await Client.ConsumerPricing.V3ListProvidersAsync(
            new V3ListProvidersRequest()
        );
        Assert.That(
            response,
            Is.EqualTo(JsonUtils.Deserialize<ListEnvelopeConsumerPricingProvider>(mockResponse))
                .UsingDefaults()
        );
    }
}
