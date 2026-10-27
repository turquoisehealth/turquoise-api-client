using NUnit.Framework;
using TurquoiseHealth.Api;
using TurquoiseHealth.Api.Core;

namespace TurquoiseHealth.Api.Test.Unit.MockServer;

[TestFixture]
public class V3GetPriceTest : BaseMockServerTest
{
    [NUnit.Framework.Test]
    public async Task MockServerTest()
    {
        const string mockResponse = """
            {
              "object": "price",
              "id": "prc_5754.RA007.-3776001016975145508",
              "provider": {
                "object": "provider",
                "id": "5754",
                "name": "HCA HealthONE Presbyterian St Lukes",
                "type": "Short Term Acute Care Hospital",
                "npi": "1720038946",
                "address": {
                  "city": "Denver",
                  "state": "CO",
                  "zip_code": "80218",
                  "latitude": 39.746549,
                  "longitude": -104.966925
                },
                "context": {
                  "score": 1.1,
                  "distance_m": 1.1
                }
              },
              "package": {
                "id": "R017Z",
                "name": "Ultrasound"
              },
              "pricing": {
                "type": "negotiated",
                "network": {
                  "id": "-3776001016975145508",
                  "name": "National OAP"
                },
                "payer": {
                  "id": "76",
                  "name": "Cigna"
                }
              },
              "total": {
                "amount": "526.10",
                "minor_units": 52610,
                "currency": "USD"
              },
              "line_items": [
                {
                  "code": "76642",
                  "code_type": "HCPCS",
                  "fee_type": "base_code",
                  "description": "Ultrasound imaging of one breast including axilla with image documentation",
                  "association_rate": 0.7245
                }
              ],
              "context": {
                "score": 1.1,
                "distance_m": 1.1
              }
            }
            """;

        Server
            .Given(
                WireMock
                    .RequestBuilders.Request.Create()
                    .WithPath("/v3/prices/prc_5754.RA007.-3776001016975145508")
                    .UsingGet()
            )
            .RespondWith(
                WireMock
                    .ResponseBuilders.Response.Create()
                    .WithStatusCode(200)
                    .WithBody(mockResponse)
            );

        var response = await Client.ConsumerPricing.V3GetPriceAsync(
            new V3GetPriceRequest { PriceId = "prc_5754.RA007.-3776001016975145508" }
        );
        Assert.That(
            response,
            Is.EqualTo(JsonUtils.Deserialize<ProviderPackagePrice>(mockResponse)).UsingDefaults()
        );
    }
}
