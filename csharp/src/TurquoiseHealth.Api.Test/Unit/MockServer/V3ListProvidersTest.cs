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
                  "id": "2743",
                  "name": "Loretto Hospital",
                  "type": "Short Term Acute Care Hospital",
                  "npi": "1447280284",
                  "address": {
                    "city": "Chicago",
                    "state": "IL",
                    "zip_code": "60644",
                    "latitude": 41.8721272,
                    "longitude": -87.7636486
                  },
                  "context": {
                    "distance_m": 1240
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
            Is.EqualTo(JsonUtils.Deserialize<V3ListEnvelopeProvider>(mockResponse)).UsingDefaults()
        );
    }
}
