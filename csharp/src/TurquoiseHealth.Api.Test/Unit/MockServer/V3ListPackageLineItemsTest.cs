using NUnit.Framework;
using TurquoiseHealth.Api;
using TurquoiseHealth.Api.Core;

namespace TurquoiseHealth.Api.Test.Unit.MockServer;

[TestFixture]
public class V3ListPackageLineItemsTest : BaseMockServerTest
{
    [NUnit.Framework.Test]
    public async Task MockServerTest()
    {
        const string mockResponse = """
            {
              "object": "list",
              "items": [
                {
                  "code": "45385",
                  "code_type": "HCPCS",
                  "fee_type": "base_code",
                  "description": "Colonoscopy",
                  "association_rate": 1
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
            .Given(
                WireMock
                    .RequestBuilders.Request.Create()
                    .WithPath("/v3/packages/GA002/line_items")
                    .UsingGet()
            )
            .RespondWith(
                WireMock
                    .ResponseBuilders.Response.Create()
                    .WithStatusCode(200)
                    .WithBody(mockResponse)
            );

        var response = await Client.ConsumerPricing.V3ListPackageLineItemsAsync(
            new V3ListPackageLineItemsRequest { PackageId = "GA002" }
        );
        Assert.That(
            response,
            Is.EqualTo(JsonUtils.Deserialize<V3ListEnvelopeLineItem>(mockResponse)).UsingDefaults()
        );
    }
}
