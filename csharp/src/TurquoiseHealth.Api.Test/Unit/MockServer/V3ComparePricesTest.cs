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
              "package_id": "RA007",
              "provider_id": "2751",
              "pricing": {
                "network_id": "network_id",
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
                PackageId = "RA007",
                ProviderId = "2751",
                Pricing = new V3PricesCompareRequestPricing(
                    new V3PricesCompareRequestPricing.Negotiated(
                        new V3PricingNegotiated { NetworkId = "network_id" }
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
              "package_id": "RA007",
              "pricing": {
                "type": "cash"
              },
              "location": {
                "within": {
                  "state": "IL"
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
                PackageId = "RA007",
                Pricing = new V3PricesCompareRequestPricing(
                    new V3PricesCompareRequestPricing.Cash(new V3PricingCash())
                ),
                Location = new V3Location { Within = new V3LocationWithin { State = "IL" } },
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
              "package_id": "RA007",
              "pricing": {
                "type": "cash"
              },
              "location": {
                "near": {
                  "lat": 41.8781,
                  "lng": -87.6298,
                  "radius_m": 40000
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
                PackageId = "RA007",
                Pricing = new V3PricesCompareRequestPricing(
                    new V3PricesCompareRequestPricing.Cash(new V3PricingCash())
                ),
                Location = new V3Location
                {
                    Near = new V3LocationNear
                    {
                        Lat = 41.8781,
                        Lng = -87.6298,
                        RadiusM = 40000,
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
