using NUnit.Framework;
using TurquoiseHealth.Api;
using TurquoiseHealth.Api.Core;

namespace TurquoiseHealth.Api.Test.Unit.MockServer;

[TestFixture]
public class V2ComparePricesTest : BaseMockServerTest
{
    [NUnit.Framework.Test]
    public async Task MockServerTest_1()
    {
        const string requestJson = """
            {
              "ssp_id": "RA011",
              "network_id": "8361580493441765265",
              "location": {
                "within_state": "CO"
              }
            }
            """;

        const string mockResponse = """
            {
              "count": 44,
              "currency": "USD",
              "min_amount": "447.14",
              "min_amount_cents": 44714,
              "max_amount": "5123.59",
              "max_amount_cents": 512359,
              "avg_amount": "2060.44",
              "avg_amount_cents": 206044,
              "median_amount": "1818.48",
              "median_amount_cents": 181848,
              "q1_amount": "1016.93",
              "q1_amount_cents": 101693,
              "q3_amount": "3051.33",
              "q3_amount_cents": 305133
            }
            """;

        Server
            .Given(
                WireMock
                    .RequestBuilders.Request.Create()
                    .WithPath("/v2/consumer-pricing/prices/price-comparison")
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

        var response = await Client.ConsumerPricing.V2ComparePricesAsync(
            new RateCompareRequest
            {
                SspId = "RA011",
                NetworkId = "8361580493441765265",
                Location = new RateCompareLocation { WithinState = "CO" },
            }
        );
        Assert.That(
            response,
            Is.EqualTo(JsonUtils.Deserialize<RateComparison>(mockResponse)).UsingDefaults()
        );
    }

    [NUnit.Framework.Test]
    public async Task MockServerTest_2()
    {
        const string requestJson = """
            {
              "ssp_id": "GA002",
              "location": {
                "within_cbsa_name": "Denver-Aurora-Centennial, CO"
              }
            }
            """;

        const string mockResponse = """
            {
              "count": 44,
              "currency": "USD",
              "min_amount": "447.14",
              "min_amount_cents": 44714,
              "max_amount": "5123.59",
              "max_amount_cents": 512359,
              "avg_amount": "2060.44",
              "avg_amount_cents": 206044,
              "median_amount": "1818.48",
              "median_amount_cents": 181848,
              "q1_amount": "1016.93",
              "q1_amount_cents": 101693,
              "q3_amount": "3051.33",
              "q3_amount_cents": 305133
            }
            """;

        Server
            .Given(
                WireMock
                    .RequestBuilders.Request.Create()
                    .WithPath("/v2/consumer-pricing/prices/price-comparison")
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

        var response = await Client.ConsumerPricing.V2ComparePricesAsync(
            new RateCompareRequest
            {
                SspId = "GA002",
                Location = new RateCompareLocation
                {
                    WithinCbsaName = "Denver-Aurora-Centennial, CO",
                },
            }
        );
        Assert.That(
            response,
            Is.EqualTo(JsonUtils.Deserialize<RateComparison>(mockResponse)).UsingDefaults()
        );
    }

    [NUnit.Framework.Test]
    public async Task MockServerTest_3()
    {
        const string requestJson = """
            {
              "ssp_id": "RA005",
              "location": {
                "within_zip_codes": [
                  "80129",
                  "80237"
                ]
              }
            }
            """;

        const string mockResponse = """
            {
              "count": 44,
              "currency": "USD",
              "min_amount": "447.14",
              "min_amount_cents": 44714,
              "max_amount": "5123.59",
              "max_amount_cents": 512359,
              "avg_amount": "2060.44",
              "avg_amount_cents": 206044,
              "median_amount": "1818.48",
              "median_amount_cents": 181848,
              "q1_amount": "1016.93",
              "q1_amount_cents": 101693,
              "q3_amount": "3051.33",
              "q3_amount_cents": 305133
            }
            """;

        Server
            .Given(
                WireMock
                    .RequestBuilders.Request.Create()
                    .WithPath("/v2/consumer-pricing/prices/price-comparison")
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

        var response = await Client.ConsumerPricing.V2ComparePricesAsync(
            new RateCompareRequest
            {
                SspId = "RA005",
                Location = new RateCompareLocation
                {
                    WithinZipCodes = new List<string>() { "80129", "80237" },
                },
            }
        );
        Assert.That(
            response,
            Is.EqualTo(JsonUtils.Deserialize<RateComparison>(mockResponse)).UsingDefaults()
        );
    }
}
