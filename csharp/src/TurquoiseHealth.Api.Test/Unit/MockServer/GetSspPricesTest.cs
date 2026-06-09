using NUnit.Framework;
using TurquoiseHealth.Api;
using TurquoiseHealth.Api.Core;

namespace TurquoiseHealth.Api.Test.Unit.MockServer;

[TestFixture]
public class GetSspPricesTest : BaseMockServerTest
{
    [NUnit.Framework.Test]
    public async Task MockServerTest_1()
    {
        const string requestJson = """
            {
              "location": {}
            }
            """;

        const string mockResponse = """
            {
              "results": [
                {
                  "provider": {
                    "provider_id": "provider_id",
                    "provider_name": "provider_name",
                    "provider_type": "provider_type",
                    "npi": "npi",
                    "city": "city",
                    "state": "state",
                    "zip_code": "zip_code",
                    "location_details": {
                      "street_address": "street_address",
                      "latitude": 1.1,
                      "longitude": 1.1,
                      "distance_in_meters": 1.1,
                      "hospital_overall_rating": 1
                    }
                  },
                  "package_price": 1.1
                },
                {
                  "provider": {
                    "provider_id": "provider_id",
                    "provider_name": "provider_name",
                    "provider_type": "provider_type",
                    "npi": "npi",
                    "city": "city",
                    "state": "state",
                    "zip_code": "zip_code",
                    "location_details": {
                      "street_address": "street_address",
                      "latitude": 1.1,
                      "longitude": 1.1,
                      "distance_in_meters": 1.1,
                      "hospital_overall_rating": 1
                    }
                  },
                  "package_price": 1.1
                }
              ],
              "page_size": 1,
              "count": 1,
              "no_data_reason": "no_data",
              "min_price": 1.1,
              "max_price": 1.1
            }
            """;

        Server
            .Given(
                WireMock
                    .RequestBuilders.Request.Create()
                    .WithPath("/v1/consumer-pricing/ssps/ssp_id/prices")
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

        var response = await Client.ConsumerPricing.GetSspPricesAsync(
            new PackagePricesRequest
            {
                SspId = "ssp_id",
                Location = new LocationInput { Coordinates = null, GeoSpace = null },
                NetworkId = null,
                PriceFilter = null,
                Npis = null,
                SortBy = null,
            }
        );
        Assert.That(
            response,
            Is.EqualTo(JsonUtils.Deserialize<SspPricePage>(mockResponse)).UsingDefaults()
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
                    "80129"
                  ]
                }
              },
              "network_id": "-7695283351826393948",
              "sort_by": "price-asc"
            }
            """;

        const string mockResponse = """
            {
              "results": [
                {
                  "provider": {
                    "provider_id": "provider_id",
                    "provider_name": "provider_name",
                    "city": "city",
                    "state": "state",
                    "zip_code": "zip_code"
                  },
                  "package_price": 1.1
                }
              ],
              "page_size": 1,
              "count": 1,
              "no_data_reason": "no_data",
              "min_price": 1.1,
              "max_price": 1.1
            }
            """;

        Server
            .Given(
                WireMock
                    .RequestBuilders.Request.Create()
                    .WithPath("/v1/consumer-pricing/ssps/DE000/prices")
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

        var response = await Client.ConsumerPricing.GetSspPricesAsync(
            new PackagePricesRequest
            {
                SspId = "DE000",
                Location = new LocationInput
                {
                    GeoSpace = new GeoSpace { ZipCodes = new List<string>() { "80129" } },
                },
                NetworkId = "-7695283351826393948",
                SortBy = CareNavSortType.PriceAsc,
            }
        );
        Assert.That(
            response,
            Is.EqualTo(JsonUtils.Deserialize<SspPricePage>(mockResponse)).UsingDefaults()
        );
    }

    [NUnit.Framework.Test]
    public async Task MockServerTest_3()
    {
        const string requestJson = """
            {
              "location": {
                "coordinates": {
                  "latitude": 39.434789,
                  "longitude": -104.901073,
                  "distance_in_meters": 40000
                }
              }
            }
            """;

        const string mockResponse = """
            {
              "results": [
                {
                  "provider": {
                    "provider_id": "provider_id",
                    "provider_name": "provider_name",
                    "city": "city",
                    "state": "state",
                    "zip_code": "zip_code"
                  },
                  "package_price": 1.1
                }
              ],
              "page_size": 1,
              "count": 1,
              "no_data_reason": "no_data",
              "min_price": 1.1,
              "max_price": 1.1
            }
            """;

        Server
            .Given(
                WireMock
                    .RequestBuilders.Request.Create()
                    .WithPath("/v1/consumer-pricing/ssps/DE000/prices")
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

        var response = await Client.ConsumerPricing.GetSspPricesAsync(
            new PackagePricesRequest
            {
                SspId = "DE000",
                Location = new LocationInput
                {
                    Coordinates = new Coordinates
                    {
                        Latitude = 39.434789,
                        Longitude = -104.901073,
                        DistanceInMeters = 40000,
                    },
                },
            }
        );
        Assert.That(
            response,
            Is.EqualTo(JsonUtils.Deserialize<SspPricePage>(mockResponse)).UsingDefaults()
        );
    }

    [NUnit.Framework.Test]
    public async Task MockServerTest_4()
    {
        const string requestJson = """
            {
              "location": {
                "geo_space": {
                  "state": "CO"
                }
              },
              "price_filter": {
                "min_price": 1000,
                "max_price": 10000
              }
            }
            """;

        const string mockResponse = """
            {
              "results": [
                {
                  "provider": {
                    "provider_id": "provider_id",
                    "provider_name": "provider_name",
                    "city": "city",
                    "state": "state",
                    "zip_code": "zip_code"
                  },
                  "package_price": 1.1
                }
              ],
              "page_size": 1,
              "count": 1,
              "no_data_reason": "no_data",
              "min_price": 1.1,
              "max_price": 1.1
            }
            """;

        Server
            .Given(
                WireMock
                    .RequestBuilders.Request.Create()
                    .WithPath("/v1/consumer-pricing/ssps/DE000/prices")
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

        var response = await Client.ConsumerPricing.GetSspPricesAsync(
            new PackagePricesRequest
            {
                SspId = "DE000",
                Location = new LocationInput { GeoSpace = new GeoSpace { State = "CO" } },
                PriceFilter = new PriceFilter { MinPrice = 1000, MaxPrice = 10000 },
            }
        );
        Assert.That(
            response,
            Is.EqualTo(JsonUtils.Deserialize<SspPricePage>(mockResponse)).UsingDefaults()
        );
    }
}
