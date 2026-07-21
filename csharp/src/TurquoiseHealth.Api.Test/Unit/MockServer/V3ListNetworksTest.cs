using NUnit.Framework;
using TurquoiseHealth.Api;
using TurquoiseHealth.Api.Core;

namespace TurquoiseHealth.Api.Test.Unit.MockServer;

[TestFixture]
public class V3ListNetworksTest : BaseMockServerTest
{
    [NUnit.Framework.Test]
    public async Task MockServerTest()
    {
        const string mockResponse = """
            {
              "object": "list",
              "items": [
                {
                  "object": "network",
                  "id": "2010265101",
                  "name": "National PPO",
                  "payer": {
                    "id": "7001",
                    "name": "Meridian Health Plan"
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
              ],
              "meta": {
                "dataset_version": "dataset_version"
              }
            }
            """;

        Server
            .Given(WireMock.RequestBuilders.Request.Create().WithPath("/v3/networks").UsingGet())
            .RespondWith(
                WireMock
                    .ResponseBuilders.Response.Create()
                    .WithStatusCode(200)
                    .WithBody(mockResponse)
            );

        var response = await Client.ConsumerPricing.V3ListNetworksAsync(
            new V3ListNetworksRequest()
        );
        Assert.That(
            response,
            Is.EqualTo(JsonUtils.Deserialize<V3ListEnvelopeNetwork>(mockResponse)).UsingDefaults()
        );
    }
}
