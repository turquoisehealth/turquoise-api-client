using NUnit.Framework;
using TurquoiseHealth.Api;
using TurquoiseHealth.Api.Core;

namespace TurquoiseHealth.Api.Test.Unit.MockServer;

[TestFixture]
public class V2ListProvidersTest : BaseMockServerTest
{
    [NUnit.Framework.Test]
    public async Task MockServerTest()
    {
        const string mockResponse = """
            {
              "items": [
                {
                  "id": "21929",
                  "name": "Mountain View Surgical Specialists",
                  "type": "ASC",
                  "npi": "1144903113",
                  "city": "Denver",
                  "state": "CO",
                  "zip_code": "80237",
                  "distance_m": 1,
                  "latitude": 39.625533,
                  "longitude": -104.898823
                }
              ],
              "page": {
                "size": 2,
                "total": 2,
                "next_cursor": "next_cursor"
              },
              "no_data_reason": "no_data"
            }
            """;

        Server
            .Given(
                WireMock
                    .RequestBuilders.Request.Create()
                    .WithPath("/v2/consumer-pricing/providers")
                    .UsingGet()
            )
            .RespondWith(
                WireMock
                    .ResponseBuilders.Response.Create()
                    .WithStatusCode(200)
                    .WithBody(mockResponse)
            );

        var response = await Client.ConsumerPricing.V2ListProvidersAsync(
            new V2ListProvidersRequest()
        );
        Assert.That(
            response,
            Is.EqualTo(JsonUtils.Deserialize<EnvelopeProvider>(mockResponse)).UsingDefaults()
        );
    }
}
