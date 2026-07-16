using NUnit.Framework;
using TurquoiseHealth.Api;
using TurquoiseHealth.Api.Core;

namespace TurquoiseHealth.Api.Test.Unit.MockServer;

[TestFixture]
public class GetInsuranceNetworksTest : BaseMockServerTest
{
    [NUnit.Framework.Test]
    public async Task MockServerTest()
    {
        const string mockResponse = """
            {
              "results": [
                {
                  "network_id": "network_id",
                  "network_name": "network_name",
                  "payer_name": "payer_name",
                  "payer_id": "payer_id"
                }
              ],
              "page_size": 1,
              "count": 1,
              "no_data_reason": "no_data"
            }
            """;

        Server
            .Given(
                WireMock
                    .RequestBuilders.Request.Create()
                    .WithPath("/v1/consumer-pricing/networks")
                    .UsingGet()
            )
            .RespondWith(
                WireMock
                    .ResponseBuilders.Response.Create()
                    .WithStatusCode(200)
                    .WithBody(mockResponse)
            );

        var response = await Client.ConsumerPricing.GetInsuranceNetworksAsync(
            new GetInsuranceNetworksRequest()
        );
        Assert.That(
            response,
            Is.EqualTo(JsonUtils.Deserialize<InsuranceNetworkPage>(mockResponse)).UsingDefaults()
        );
    }
}
