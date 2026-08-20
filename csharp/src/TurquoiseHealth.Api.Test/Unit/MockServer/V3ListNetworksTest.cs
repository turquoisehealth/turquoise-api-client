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
                  "id": "-3776001016975145508",
                  "name": "National OAP",
                  "payer": {
                    "id": "76",
                    "name": "Cigna"
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
            Is.EqualTo(JsonUtils.Deserialize<ListEnvelopeNetwork>(mockResponse)).UsingDefaults()
        );
    }
}
