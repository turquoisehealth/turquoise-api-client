using NUnit.Framework;
using TurquoiseHealth.Api;
using TurquoiseHealth.Api.Core;

namespace TurquoiseHealth.Api.Test.Unit.MockServer;

[TestFixture]
public class V3PriceStatisticsTest : BaseMockServerTest
{
    [NUnit.Framework.Test]
    public async Task MockServerTest_1()
    {
        const string requestJson = """
            {
              "package_id": "R017Z",
              "provider_id": "5754",
              "pricing": {
                "network_id": "-3776001016975145508",
                "type": "negotiated"
              }
            }
            """;

        const string mockResponse = """
            {
              "object": "price_statistics",
              "count": 1,
              "stats": {
                "min": {
                  "amount": "amount",
                  "minor_units": 1,
                  "currency": "currency"
                },
                "max": {
                  "amount": "amount",
                  "minor_units": 1,
                  "currency": "currency"
                },
                "avg": {
                  "amount": "amount",
                  "minor_units": 1,
                  "currency": "currency"
                },
                "median": {
                  "amount": "amount",
                  "minor_units": 1,
                  "currency": "currency"
                },
                "q1": {
                  "amount": "amount",
                  "minor_units": 1,
                  "currency": "currency"
                },
                "q3": {
                  "amount": "amount",
                  "minor_units": 1,
                  "currency": "currency"
                }
              },
              "disclosures": [
                "disclosures"
              ],
              "meta": {
                "dataset_version": "dataset_version"
              }
            }
            """;

        Server
            .Given(
                WireMock
                    .RequestBuilders.Request.Create()
                    .WithPath("/v3/prices/statistics")
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

        var response = await Client.ConsumerPricing.V3PriceStatisticsAsync(
            new PricesStatisticsRequest
            {
                PackageId = "R017Z",
                ProviderId = "5754",
                Pricing = new PricesStatisticsRequestPricing(
                    new PricesStatisticsRequestPricing.Negotiated(
                        new PricingNegotiated { NetworkId = "-3776001016975145508" }
                    )
                ),
            }
        );
        Assert.That(
            response,
            Is.EqualTo(JsonUtils.Deserialize<PriceStatistics>(mockResponse)).UsingDefaults()
        );
    }

    [NUnit.Framework.Test]
    public async Task MockServerTest_2()
    {
        const string requestJson = """
            {
              "package_id": "R017Z",
              "pricing": {
                "type": "cash"
              },
              "location": {
                "within": {
                  "zip_codes": [
                    "80218",
                    "80210"
                  ]
                }
              }
            }
            """;

        const string mockResponse = """
            {
              "object": "price_statistics",
              "count": 1,
              "stats": {
                "min": {
                  "amount": "amount",
                  "minor_units": 1,
                  "currency": "currency"
                },
                "max": {
                  "amount": "amount",
                  "minor_units": 1,
                  "currency": "currency"
                },
                "avg": {
                  "amount": "amount",
                  "minor_units": 1,
                  "currency": "currency"
                },
                "median": {
                  "amount": "amount",
                  "minor_units": 1,
                  "currency": "currency"
                },
                "q1": {
                  "amount": "amount",
                  "minor_units": 1,
                  "currency": "currency"
                },
                "q3": {
                  "amount": "amount",
                  "minor_units": 1,
                  "currency": "currency"
                }
              },
              "disclosures": [
                "disclosures"
              ],
              "meta": {
                "dataset_version": "dataset_version"
              }
            }
            """;

        Server
            .Given(
                WireMock
                    .RequestBuilders.Request.Create()
                    .WithPath("/v3/prices/statistics")
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

        var response = await Client.ConsumerPricing.V3PriceStatisticsAsync(
            new PricesStatisticsRequest
            {
                PackageId = "R017Z",
                Pricing = new PricesStatisticsRequestPricing(
                    new PricesStatisticsRequestPricing.Cash(new PricingCash())
                ),
                Location = new Location
                {
                    Within = new LocationWithin
                    {
                        ZipCodes = new List<string>() { "80218", "80210" },
                    },
                },
            }
        );
        Assert.That(
            response,
            Is.EqualTo(JsonUtils.Deserialize<PriceStatistics>(mockResponse)).UsingDefaults()
        );
    }

    [NUnit.Framework.Test]
    public async Task MockServerTest_3()
    {
        const string requestJson = """
            {
              "package_id": "R017Z",
              "pricing": {
                "type": "cash"
              },
              "location": {
                "near": {
                  "lat": 39.746549,
                  "lng": -104.966925,
                  "radius_m": 25000
                }
              }
            }
            """;

        const string mockResponse = """
            {
              "object": "price_statistics",
              "count": 1,
              "stats": {
                "min": {
                  "amount": "amount",
                  "minor_units": 1,
                  "currency": "currency"
                },
                "max": {
                  "amount": "amount",
                  "minor_units": 1,
                  "currency": "currency"
                },
                "avg": {
                  "amount": "amount",
                  "minor_units": 1,
                  "currency": "currency"
                },
                "median": {
                  "amount": "amount",
                  "minor_units": 1,
                  "currency": "currency"
                },
                "q1": {
                  "amount": "amount",
                  "minor_units": 1,
                  "currency": "currency"
                },
                "q3": {
                  "amount": "amount",
                  "minor_units": 1,
                  "currency": "currency"
                }
              },
              "disclosures": [
                "disclosures"
              ],
              "meta": {
                "dataset_version": "dataset_version"
              }
            }
            """;

        Server
            .Given(
                WireMock
                    .RequestBuilders.Request.Create()
                    .WithPath("/v3/prices/statistics")
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

        var response = await Client.ConsumerPricing.V3PriceStatisticsAsync(
            new PricesStatisticsRequest
            {
                PackageId = "R017Z",
                Pricing = new PricesStatisticsRequestPricing(
                    new PricesStatisticsRequestPricing.Cash(new PricingCash())
                ),
                Location = new Location
                {
                    Near = new LocationNear
                    {
                        Lat = 39.746549,
                        Lng = -104.966925,
                        RadiusM = 25000,
                    },
                },
            }
        );
        Assert.That(
            response,
            Is.EqualTo(JsonUtils.Deserialize<PriceStatistics>(mockResponse)).UsingDefaults()
        );
    }
}
