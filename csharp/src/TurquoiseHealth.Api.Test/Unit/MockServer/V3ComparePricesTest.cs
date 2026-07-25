using NUnit.Framework;
using TurquoiseHealth.Api;
using TurquoiseHealth.Api.Core;

namespace TurquoiseHealth.Api.Test.Unit.MockServer;

[TestFixture]
public class V3ComparePricesTest : BaseMockServerTest
{
    [NUnit.Framework.Test]
    public async Task MockServerTest_1()
    {
        const string requestJson = """
            {
              "package_id": "OB002",
              "provider_id": "5756",
              "pricing": {
                "network_id": "-3776001016975145508",
                "type": "negotiated"
              }
            }
            """;

        const string mockResponse = """
            {
              "object": "price_comparison",
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
                    .WithPath("/v3/prices/compare")
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

        var response = await Client.ConsumerPricing.V3ComparePricesAsync(
            new V3PricesCompareRequest
            {
                PackageId = "OB002",
                ProviderId = "5756",
                Pricing = new V3PricesCompareRequestPricing(
                    new V3PricesCompareRequestPricing.Negotiated(
                        new V3PricingNegotiated { NetworkId = "-3776001016975145508" }
                    )
                ),
            }
        );
        Assert.That(
            response,
            Is.EqualTo(JsonUtils.Deserialize<V3PriceComparison>(mockResponse)).UsingDefaults()
        );
    }

    [NUnit.Framework.Test]
    public async Task MockServerTest_2()
    {
        const string requestJson = """
            {
              "package_id": "RA008",
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
              "object": "price_comparison",
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
                    .WithPath("/v3/prices/compare")
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

        var response = await Client.ConsumerPricing.V3ComparePricesAsync(
            new V3PricesCompareRequest
            {
                PackageId = "RA008",
                Pricing = new V3PricesCompareRequestPricing(
                    new V3PricesCompareRequestPricing.Cash(new V3PricingCash())
                ),
                Location = new V3Location
                {
                    Within = new V3LocationWithin
                    {
                        ZipCodes = new List<string>() { "80218", "80210" },
                    },
                },
            }
        );
        Assert.That(
            response,
            Is.EqualTo(JsonUtils.Deserialize<V3PriceComparison>(mockResponse)).UsingDefaults()
        );
    }

    [NUnit.Framework.Test]
    public async Task MockServerTest_3()
    {
        const string requestJson = """
            {
              "package_id": "RA008",
              "pricing": {
                "type": "cash"
              },
              "location": {
                "near": {
                  "lat": 39.745961,
                  "lng": -104.971559,
                  "radius_m": 25000
                }
              }
            }
            """;

        const string mockResponse = """
            {
              "object": "price_comparison",
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
                    .WithPath("/v3/prices/compare")
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

        var response = await Client.ConsumerPricing.V3ComparePricesAsync(
            new V3PricesCompareRequest
            {
                PackageId = "RA008",
                Pricing = new V3PricesCompareRequestPricing(
                    new V3PricesCompareRequestPricing.Cash(new V3PricingCash())
                ),
                Location = new V3Location
                {
                    Near = new V3LocationNear
                    {
                        Lat = 39.745961,
                        Lng = -104.971559,
                        RadiusM = 25000,
                    },
                },
            }
        );
        Assert.That(
            response,
            Is.EqualTo(JsonUtils.Deserialize<V3PriceComparison>(mockResponse)).UsingDefaults()
        );
    }
}
