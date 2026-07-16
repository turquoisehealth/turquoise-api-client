using NUnit.Framework;
using TurquoiseHealth.Api;
using TurquoiseHealth.Api.Core;

namespace TurquoiseHealth.Api.Test.Unit.MockServer;

[TestFixture]
public class V2GetNetworkTest : BaseMockServerTest
{
    [NUnit.Framework.Test]
    public async Task MockServerTest()
    {
        const string mockResponse = """
            {
              "data": {
                "id": "8361580493441765265",
                "name": "UnitedHealthcare Choice Plus",
                "payer_id": "643",
                "payer_name": "UnitedHealthcare"
              },
              "no_data_reason": "no_data"
            }
            """;

        Server
            .Given(
                WireMock
                    .RequestBuilders.Request.Create()
                    .WithPath("/v2/consumer-pricing/payers/networks/8361580493441765265")
                    .UsingGet()
            )
            .RespondWith(
                WireMock
                    .ResponseBuilders.Response.Create()
                    .WithStatusCode(200)
                    .WithBody(mockResponse)
            );

        var response = await Client.ConsumerPricing.V2GetNetworkAsync(
            new V2GetNetworkRequest { NetworkId = "8361580493441765265" }
        );
        Assert.That(
            response,
            Is.EqualTo(JsonUtils.Deserialize<SingleResourceEnvelopeNetwork>(mockResponse))
                .UsingDefaults()
        );
    }
}
