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
              "id": "prc_2751.RA007.2010265101",
              "provider": {
                "object": "provider",
                "id": "2751",
                "name": "Provident Hospital of Cook County",
                "type": "type",
                "npi": "npi",
                "address": {
                  "city": "city",
                  "state": "state",
                  "zip_code": "zip_code",
                  "latitude": 1.1,
                  "longitude": 1.1
                },
                "context": {
                  "score": 1.1,
                  "distance_m": 1.1
                }
              },
              "package": {
                "id": "RA007",
                "name": "MRI with Contrast"
              },
              "pricing": {
                "type": "negotiated",
                "network": {
                  "id": "2010265101",
                  "name": "Example PPO"
                },
                "payer": {
                  "id": "643",
                  "name": "Example Payer"
                }
              },
              "total": {
                "amount": "1250.00",
                "minor_units": 125000,
                "currency": "USD"
              },
              "line_items": [
                {
                  "code": "45385",
                  "code_type": "HCPCS",
                  "fee_type": "base_code",
                  "description": "Colonoscopy",
                  "association_rate": 1
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
                    .WithPath("/v3/prices/prc_2751.RA007.8361580493441765265")
                    .UsingGet()
            )
            .RespondWith(
                WireMock
                    .ResponseBuilders.Response.Create()
                    .WithStatusCode(200)
                    .WithBody(mockResponse)
            );

        var response = await Client.ConsumerPricing.V3GetPriceAsync(
            new V3GetPriceRequest { PriceId = "prc_2751.RA007.8361580493441765265" }
        );
        Assert.That(
            response,
            Is.EqualTo(JsonUtils.Deserialize<V3ProviderPackagePrice>(mockResponse)).UsingDefaults()
        );
    }
}
