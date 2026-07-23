using NUnit.Framework;
using TurquoiseHealth.Api;
using TurquoiseHealth.Api.Core;

namespace TurquoiseHealth.Api.Test.Unit.MockServer;

[TestFixture]
public class V2ListNetworksTest : BaseMockServerTest
{
    [NUnit.Framework.Test]
    public async Task MockServerTest()
    {
        const string mockResponse = """
            {
              "items": [
                {
                  "id": "8361580493441765265",
                  "name": "UnitedHealthcare Choice Plus",
                  "payer_id": "643",
                  "payer_name": "UnitedHealthcare",
                  "score": 0.89
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
                    .WithPath("/v2/consumer-pricing/payers/networks")
                    .UsingGet()
            )
            .RespondWith(
                WireMock
                    .ResponseBuilders.Response.Create()
                    .WithStatusCode(200)
                    .WithBody(mockResponse)
            );

        var response = await Client.ConsumerPricing.V2ListNetworksAsync(
            new V2ListNetworksRequest()
        );
        Assert.That(
            response,
            Is.EqualTo(JsonUtils.Deserialize<EnvelopeNetwork>(mockResponse)).UsingDefaults()
        );
    }
}
