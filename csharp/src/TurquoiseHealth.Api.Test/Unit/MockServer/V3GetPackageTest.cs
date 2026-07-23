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
              ],
              "context": {
                "score": 1.1,
                "distance_m": 1.1
              }
            }
            """;

        Server
            .Given(
                WireMock.RequestBuilders.Request.Create().WithPath("/v3/packages/GA002").UsingGet()
            )
            .RespondWith(
                WireMock
                    .ResponseBuilders.Response.Create()
                    .WithStatusCode(200)
                    .WithBody(mockResponse)
            );

        var response = await Client.ConsumerPricing.V3GetPackageAsync(
            new V3GetPackageRequest { PackageId = "GA002" }
        );
        Assert.That(
            response,
            Is.EqualTo(JsonUtils.Deserialize<V3Package>(mockResponse)).UsingDefaults()
        );
    }
}
