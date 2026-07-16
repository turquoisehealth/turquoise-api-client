using NUnit.Framework;
using OneOf;
using TurquoiseHealth.Api;
using TurquoiseHealth.Api.Core;

namespace TurquoiseHealth.Api.Test.Unit.MockServer;

[TestFixture]
public class CompareSspPricesTest : BaseMockServerTest
{
    [NUnit.Framework.Test]
    public async Task MockServerTest_1()
    {
        const string requestJson = """
            {
              "location": {
                "geo_space": {
                  "state": "CO"
                }
              },
              "network_id": "-7695283351826393948"
            }
            """;

        const string mockResponse = """
            {
              "min_price": 1.1,
              "max_price": 1.1,
              "avg_price": 1.1,
              "q1_price": 1.1,
              "median_price": 1.1,
              "q3_price": 1.1
            }
            """;

        Server
            .Given(
                WireMock
                    .RequestBuilders.Request.Create()
                    .WithPath("/v1/consumer-pricing/ssps/DE000/price-comparison")
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

        var response = await Client.ConsumerPricing.CompareSspPricesAsync(
            new PriceComparisonRequest
            {
                SspId = "DE000",
                Location = new LocationInput { GeoSpace = new GeoSpace { State = "CO" } },
                NetworkId = "-7695283351826393948",
            }
        );
        Assert.That(
            response.Value,
            Is.EqualTo(
                    JsonUtils
                        .Deserialize<OneOf<PricesComparison, NoDataResponse>>(mockResponse)
                        .Value
                )
                .UsingDefaults()
        );
    }

    [NUnit.Framework.Test]
    public async Task MockServerTest_2()
    {
        const string requestJson = """
            {
              "location": {
                "geo_space": {
                  "zip_codes": [
                    "10601",
                    "10701"
                  ]
                }
              }
            }
            """;

        const string mockResponse = """
            {
              "min_price": 1.1,
              "max_price": 1.1,
              "avg_price": 1.1,
              "q1_price": 1.1,
              "median_price": 1.1,
              "q3_price": 1.1
            }
            """;

        Server
            .Given(
                WireMock
                    .RequestBuilders.Request.Create()
                    .WithPath("/v1/consumer-pricing/ssps/DE000/price-comparison")
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

        var response = await Client.ConsumerPricing.CompareSspPricesAsync(
            new PriceComparisonRequest
            {
                SspId = "DE000",
                Location = new LocationInput
                {
                    GeoSpace = new GeoSpace
                    {
                        ZipCodes = new List<string>() { "10601", "10701" },
                    },
                },
            }
        );
        Assert.That(
            response.Value,
            Is.EqualTo(
                    JsonUtils
                        .Deserialize<OneOf<PricesComparison, NoDataResponse>>(mockResponse)
                        .Value
                )
                .UsingDefaults()
        );
    }
}
