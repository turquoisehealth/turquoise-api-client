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
                  "id": "GA002",
                  "type": "ssp",
                  "name": "Colonoscopy",
                  "description": "A colonoscopy is a standard medical procedure used to diagnose and treat gastrointestinal tract conditions.",
                  "anchor_codes": [
                    {
                      "code": "45378",
                      "code_type": "HCPCS"
                    },
                    {
                      "code": "45385",
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
              ],
              "meta": {
                "dataset_version": "dataset_version"
              }
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
            Is.EqualTo(JsonUtils.Deserialize<V3ListEnvelopePackage>(mockResponse)).UsingDefaults()
        );
    }
}
