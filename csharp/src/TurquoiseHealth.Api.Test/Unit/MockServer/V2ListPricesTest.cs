using NUnit.Framework;
using TurquoiseHealth.Api;
using TurquoiseHealth.Api.Core;

namespace TurquoiseHealth.Api.Test.Unit.MockServer;

[TestFixture]
public class V2ListPricesTest : BaseMockServerTest
{
    [NUnit.Framework.Test]
    public async Task MockServerTest_1()
    {
        const string requestJson = """
            {
              "ssp_id": "GA002",
              "network_id": "8361580493441765265",
              "location": {
                "zip_anchor": "80202"
              }
            }
            """;

        const string mockResponse = """
            {
              "items": [
                {
                  "price_type": "cash",
                  "provider_id": "0001",
                  "ssp_id": "GA002",
                  "network_id": "network_id",
                  "amount": "335.16",
                  "amount_cents": 33516,
                  "currency": "USD"
                }
              ],
              "page": {
                "size": 2,
                "total": 2,
                "next_cursor": "next_cursor"
              },
              "no_data_reason": "no_data"
            }
            """;

        Server
            .Given(
                WireMock
                    .RequestBuilders.Request.Create()
                    .WithPath("/v2/consumer-pricing/prices")
                    .WithHeader("Content-Type", "application/json")
                    .UsingPost()
                    .WithBodyAsJson(requestJson)
            )
            .RespondWith(
                WireMock
                    .ResponseBuilders.Response.Create()
                    .WithStatusCode(200)
                    .WithBody(mockResponse)
            );

        var response = await Client.ConsumerPricing.V2ListPricesAsync(
            new PricesRequest
            {
                SspId = "GA002",
                NetworkId = "8361580493441765265",
                Location = new RateCompareLocation { ZipAnchor = "80202" },
            }
        );
        Assert.That(
            response,
            Is.EqualTo(JsonUtils.Deserialize<EnvelopeRate>(mockResponse)).UsingDefaults()
        );
    }

    [NUnit.Framework.Test]
    public async Task MockServerTest_2()
    {
        const string requestJson = """
            {
              "ssp_id": "GA002",
              "location": {
                "within_state": "CO"
              }
            }
            """;

        const string mockResponse = """
            {
              "items": [
                {
                  "price_type": "cash",
                  "provider_id": "0001",
                  "ssp_id": "GA002",
                  "network_id": "network_id",
                  "amount": "335.16",
                  "amount_cents": 33516,
                  "currency": "USD"
                }
              ],
              "page": {
                "size": 2,
                "total": 2,
                "next_cursor": "next_cursor"
              },
              "no_data_reason": "no_data"
            }
            """;

        Server
            .Given(
                WireMock
                    .RequestBuilders.Request.Create()
                    .WithPath("/v2/consumer-pricing/prices")
                    .WithHeader("Content-Type", "application/json")
                    .UsingPost()
                    .WithBodyAsJson(requestJson)
            )
            .RespondWith(
                WireMock
                    .ResponseBuilders.Response.Create()
                    .WithStatusCode(200)
                    .WithBody(mockResponse)
            );

        var response = await Client.ConsumerPricing.V2ListPricesAsync(
            new PricesRequest
            {
                SspId = "GA002",
                Location = new RateCompareLocation { WithinState = "CO" },
            }
        );
        Assert.That(
            response,
            Is.EqualTo(JsonUtils.Deserialize<EnvelopeRate>(mockResponse)).UsingDefaults()
        );
    }

    [NUnit.Framework.Test]
    public async Task MockServerTest_3()
    {
        const string requestJson = """
            {
              "ssp_id": "RA005",
              "location": {
                "near_lat": 39.625533,
                "near_lng": -104.898823,
                "near_radius_m": 25000
              },
              "page_size": 50
            }
            """;

        const string mockResponse = """
            {
              "items": [
                {
                  "price_type": "cash",
                  "provider_id": "0001",
                  "ssp_id": "GA002",
                  "network_id": "network_id",
                  "amount": "335.16",
                  "amount_cents": 33516,
                  "currency": "USD"
                }
              ],
              "page": {
                "size": 2,
                "total": 2,
                "next_cursor": "next_cursor"
              },
              "no_data_reason": "no_data"
            }
            """;

        Server
            .Given(
                WireMock
                    .RequestBuilders.Request.Create()
                    .WithPath("/v2/consumer-pricing/prices")
                    .WithHeader("Content-Type", "application/json")
                    .UsingPost()
                    .WithBodyAsJson(requestJson)
            )
            .RespondWith(
                WireMock
                    .ResponseBuilders.Response.Create()
                    .WithStatusCode(200)
                    .WithBody(mockResponse)
            );

        var response = await Client.ConsumerPricing.V2ListPricesAsync(
            new PricesRequest
            {
                SspId = "RA005",
                Location = new RateCompareLocation
                {
                    NearLat = 39.625533,
                    NearLng = -104.898823,
                    NearRadiusM = 25000,
                },
                PageSize = 50,
            }
        );
        Assert.That(
            response,
            Is.EqualTo(JsonUtils.Deserialize<EnvelopeRate>(mockResponse)).UsingDefaults()
        );
    }

    [NUnit.Framework.Test]
    public async Task MockServerTest_4()
    {
        const string requestJson = """
            {
              "provider_id": "21929"
            }
            """;

        const string mockResponse = """
            {
              "items": [
                {
                  "price_type": "cash",
                  "provider_id": "0001",
                  "ssp_id": "GA002",
                  "network_id": "network_id",
                  "amount": "335.16",
                  "amount_cents": 33516,
                  "currency": "USD"
                }
              ],
              "page": {
                "size": 2,
                "total": 2,
                "next_cursor": "next_cursor"
              },
              "no_data_reason": "no_data"
            }
            """;

        Server
            .Given(
                WireMock
                    .RequestBuilders.Request.Create()
                    .WithPath("/v2/consumer-pricing/prices")
                    .WithHeader("Content-Type", "application/json")
                    .UsingPost()
                    .WithBodyAsJson(requestJson)
            )
            .RespondWith(
                WireMock
                    .ResponseBuilders.Response.Create()
                    .WithStatusCode(200)
                    .WithBody(mockResponse)
            );

        var response = await Client.ConsumerPricing.V2ListPricesAsync(
            new PricesRequest { ProviderId = "21929" }
        );
        Assert.That(
            response,
            Is.EqualTo(JsonUtils.Deserialize<EnvelopeRate>(mockResponse)).UsingDefaults()
        );
    }
}
