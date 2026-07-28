using NUnit.Framework;
using TurquoiseHealth.Api;
using TurquoiseHealth.Api.Core;

namespace TurquoiseHealth.Api.Test.Unit.MockServer;

[TestFixture]
public class V3GetPackageTest : BaseMockServerTest
{
    [NUnit.Framework.Test]
    public async Task MockServerTest()
    {
        const string mockResponse = """
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
              ],
              "context": {
                "score": 1.1
              }
            }
            """;

        Server
            .Given(
                WireMock.RequestBuilders.Request.Create().WithPath("/v3/packages/OB002").UsingGet()
            )
            .RespondWith(
                WireMock
                    .ResponseBuilders.Response.Create()
                    .WithStatusCode(200)
                    .WithBody(mockResponse)
            );

        var response = await Client.ConsumerPricing.V3GetPackageAsync(
            new V3GetPackageRequest { PackageId = "OB002" }
        );
        Assert.That(
            response,
            Is.EqualTo(JsonUtils.Deserialize<V3Package>(mockResponse)).UsingDefaults()
        );
    }
}
