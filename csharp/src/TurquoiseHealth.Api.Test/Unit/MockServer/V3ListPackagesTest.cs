using NUnit.Framework;
using TurquoiseHealth.Api;
using TurquoiseHealth.Api.Core;

namespace TurquoiseHealth.Api.Test.Unit.MockServer;

[TestFixture]
public class V3ListPackagesTest : BaseMockServerTest
{
    [NUnit.Framework.Test]
    public async Task MockServerTest()
    {
        const string mockResponse = """
            {
              "object": "list",
              "items": [
                {
                  "object": "package",
                  "id": "OB002",
                  "type": "ssp",
                  "name": "Delivery - caesarean",
                  "description": "Cesarean delivery (C-section) is a surgical procedure in which the baby is delivered through incisions made in the abdomen and uterus. It may be planned in advance due to specific medical conditions or performed as an emergency if complications arise during labor. The procedure is done under regional or general anesthesia, and most patients stay in the hospital for two to four days. Recovery from a C-section typically takes longer than from a vaginal delivery, and your care team will provide instructions for wound care and activity during healing.",
                  "anchor_codes": [
                    {
                      "code": "59510",
                      "code_type": "HCPCS"
                    },
                    {
                      "code": "784",
                      "code_type": "MS-DRG"
                    },
                    {
                      "code": "785",
                      "code_type": "MS-DRG"
                    },
                    {
                      "code": "786",
                      "code_type": "MS-DRG"
                    }
                  ],
                  "disclosures": [
                    "The full package depends on your provider and insurance plan. A final price includes additional institutional and professional fee codes."
                  ]
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
            .Given(WireMock.RequestBuilders.Request.Create().WithPath("/v3/packages").UsingGet())
            .RespondWith(
                WireMock
                    .ResponseBuilders.Response.Create()
                    .WithStatusCode(200)
                    .WithBody(mockResponse)
            );

        var response = await Client.ConsumerPricing.V3ListPackagesAsync(
            new V3ListPackagesRequest()
        );
        Assert.That(
            response,
            Is.EqualTo(JsonUtils.Deserialize<ListEnvelopePackage>(mockResponse)).UsingDefaults()
        );
    }
}
