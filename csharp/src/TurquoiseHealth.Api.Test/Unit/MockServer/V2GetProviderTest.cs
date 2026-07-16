using NUnit.Framework;
using TurquoiseHealth.Api;
using TurquoiseHealth.Api.Core;

namespace TurquoiseHealth.Api.Test.Unit.MockServer;

[TestFixture]
public class V2GetProviderTest : BaseMockServerTest
{
    [NUnit.Framework.Test]
    public async Task MockServerTest()
    {
        const string mockResponse = """
            {
              "data": {
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
              },
              "no_data_reason": "no_data"
            }
            """;

        Server
            .Given(
                WireMock
                    .RequestBuilders.Request.Create()
                    .WithPath("/v2/consumer-pricing/providers/21929")
                    .UsingGet()
            )
            .RespondWith(
                WireMock
                    .ResponseBuilders.Response.Create()
                    .WithStatusCode(200)
                    .WithBody(mockResponse)
            );

        var response = await Client.ConsumerPricing.V2GetProviderAsync(
            new V2GetProviderRequest { ProviderId = "21929" }
        );
        Assert.That(
            response,
            Is.EqualTo(JsonUtils.Deserialize<SingleResourceEnvelopeProvider>(mockResponse))
                .UsingDefaults()
        );
    }
}
