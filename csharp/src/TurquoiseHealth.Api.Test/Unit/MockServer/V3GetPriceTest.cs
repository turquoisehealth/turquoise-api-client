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
              "id": "prc_5756.OB002.-3776001016975145508",
              "provider": {
                "object": "provider",
                "id": "5756",
                "name": "Intermountain Health Saint Joseph Hospital",
                "type": "Short Term Acute Care Hospital",
                "npi": "1417946021",
                "address": {
                  "city": "Denver",
                  "state": "CO",
                  "zip_code": "80218",
                  "latitude": 39.745961,
                  "longitude": -104.971559
                },
                "context": {
                  "score": 1.1,
                  "distance_m": 1.1
                }
              },
              "package": {
                "id": "OB002",
                "name": "Delivery - caesarean"
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
                "amount": "34705.48",
                "minor_units": 3470548,
                "currency": "USD"
              },
              "line_items": [
                {
                  "code": "786",
                  "code_type": "MS-DRG",
                  "fee_type": "base_code",
                  "description": "CESAREAN SECTION WITHOUT STERILIZATION WITH MCC",
                  "association_rate": 0.4491
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
                    .WithPath("/v3/prices/prc_5756.OB002.-3776001016975145508")
                    .UsingGet()
            )
            .RespondWith(
                WireMock
                    .ResponseBuilders.Response.Create()
                    .WithStatusCode(200)
                    .WithBody(mockResponse)
            );

        var response = await Client.ConsumerPricing.V3GetPriceAsync(
            new V3GetPriceRequest { PriceId = "prc_5756.OB002.-3776001016975145508" }
        );
        Assert.That(
            response,
            Is.EqualTo(JsonUtils.Deserialize<V3ProviderPackagePrice>(mockResponse)).UsingDefaults()
        );
    }
}
