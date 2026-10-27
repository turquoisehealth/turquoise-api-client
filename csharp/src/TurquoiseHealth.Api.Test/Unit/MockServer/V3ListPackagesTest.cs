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
                  "id": "R017Z",
                  "type": "ssp_grouper",
                  "name": "Ultrasound",
                  "description": "An ultrasound uses high-frequency sound waves to create real-time images of internal organs, tissues, and blood flow. This painless outpatient test requires no radiation and no needles. A technician applies gel to your skin and moves a small probe, instantly showing images on a screen. The procedure takes 15-45 minutes depending on what's being evaluated. Ultrasound safely evaluates pregnancy, organs, blood vessels, joints, and soft tissues. Results guide diagnosis and treatment planning. You go home immediately and can resume all activities. This estimate is based on the most common variation of this service, a breast ultrasound. Your price may differ depending on the type of ultrasound you have.",
                  "anchor_codes": [
                    {
                      "code": "76641",
                      "code_type": "HCPCS"
                    },
                    {
                      "code": "76642",
                      "code_type": "HCPCS"
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
