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
                  "code": "786",
                  "code_type": "MS-DRG",
                  "fee_type": "base_code",
                  "description": "CESAREAN SECTION WITHOUT STERILIZATION WITH MCC",
                  "association_rate": 0.4491
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
            .Given(
                WireMock
                    .RequestBuilders.Request.Create()
                    .WithPath("/v3/packages/OB002/line_items")
                    .UsingGet()
            )
            .RespondWith(
                WireMock
                    .ResponseBuilders.Response.Create()
                    .WithStatusCode(200)
                    .WithBody(mockResponse)
            );

        var response = await Client.ConsumerPricing.V3ListPackageLineItemsAsync(
            new V3ListPackageLineItemsRequest { PackageId = "OB002" }
        );
        Assert.That(
            response,
            Is.EqualTo(JsonUtils.Deserialize<V3ListEnvelopeLineItem>(mockResponse)).UsingDefaults()
        );
    }
}
