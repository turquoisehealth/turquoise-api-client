using NUnit.Framework;
using TurquoiseHealth.Api;
using TurquoiseHealth.Api.Core;

namespace TurquoiseHealth.Api.Test.Unit.MockServer;

[TestFixture]
public class V3QueryPricesTest : BaseMockServerTest
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
              "object": "list",
              "items": [
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
                  ]
                }
              ],
              "page": {
                "size": 1,
                "total": 1,
                "next_cursor": "next_cursor"
              },
              "no_data_reason": "no_data",
              "disclosures": [
                "disclosures"
              ]
            }
            """;

        Server
            .Given(
                WireMock
                    .RequestBuilders.Request.Create()
                    .WithPath("/v3/prices/query")
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

        var response = await Client.ConsumerPricing.V3QueryPricesAsync(
            new PricesQueryRequest
            {
                PackageId = "OB002",
                ProviderId = "5756",
                Pricing = new PricesQueryRequestPricing(
                    new PricesQueryRequestPricing.Negotiated(
                        new PricingNegotiated { NetworkId = "-3776001016975145508" }
                    )
                ),
            }
        );
        Assert.That(
            response,
            Is.EqualTo(JsonUtils.Deserialize<ListEnvelopeProviderPackagePrice>(mockResponse))
                .UsingDefaults()
        );
    }

    [NUnit.Framework.Test]
    public async Task MockServerTest_2()
    {
        const string requestJson = """
            {
              "package_id": "RA008",
              "provider_id": "5756",
              "pricing": {
                "type": "cash"
              }
            }
            """;

        const string mockResponse = """
            {
              "object": "list",
              "items": [
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
                  ]
                }
              ],
              "page": {
                "size": 1,
                "total": 1,
                "next_cursor": "next_cursor"
              },
              "no_data_reason": "no_data",
              "disclosures": [
                "disclosures"
              ]
            }
            """;

        Server
            .Given(
                WireMock
                    .RequestBuilders.Request.Create()
                    .WithPath("/v3/prices/query")
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

        var response = await Client.ConsumerPricing.V3QueryPricesAsync(
            new PricesQueryRequest
            {
                PackageId = "RA008",
                ProviderId = "5756",
                Pricing = new PricesQueryRequestPricing(
                    new PricesQueryRequestPricing.Cash(new PricingCash())
                ),
            }
        );
        Assert.That(
            response,
            Is.EqualTo(JsonUtils.Deserialize<ListEnvelopeProviderPackagePrice>(mockResponse))
                .UsingDefaults()
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
              },
              "sort": "distance"
            }
            """;

        const string mockResponse = """
            {
              "object": "list",
              "items": [
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
                  ]
                }
              ],
              "page": {
                "size": 1,
                "total": 1,
                "next_cursor": "next_cursor"
              },
              "no_data_reason": "no_data",
              "disclosures": [
                "disclosures"
              ]
            }
            """;

        Server
            .Given(
                WireMock
                    .RequestBuilders.Request.Create()
                    .WithPath("/v3/prices/query")
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

        var response = await Client.ConsumerPricing.V3QueryPricesAsync(
            new PricesQueryRequest
            {
                PackageId = "RA008",
                Pricing = new PricesQueryRequestPricing(
                    new PricesQueryRequestPricing.Cash(new PricingCash())
                ),
                Location = new Location
                {
                    Near = new LocationNear
                    {
                        Lat = 39.745961,
                        Lng = -104.971559,
                        RadiusM = 25000,
                    },
                },
                Sort = PriceSort.Distance,
            }
        );
        Assert.That(
            response,
            Is.EqualTo(JsonUtils.Deserialize<ListEnvelopeProviderPackagePrice>(mockResponse))
                .UsingDefaults()
        );
    }

    [NUnit.Framework.Test]
    public async Task MockServerTest_4()
    {
        const string requestJson = """
            {
              "package_id": "RA008",
              "pricing": {
                "type": "cash"
              },
              "location": {
                "zip": "80218"
              }
            }
            """;

        const string mockResponse = """
            {
              "object": "list",
              "items": [
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
                  ]
                }
              ],
              "page": {
                "size": 1,
                "total": 1,
                "next_cursor": "next_cursor"
              },
              "no_data_reason": "no_data",
              "disclosures": [
                "disclosures"
              ]
            }
            """;

        Server
            .Given(
                WireMock
                    .RequestBuilders.Request.Create()
                    .WithPath("/v3/prices/query")
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

        var response = await Client.ConsumerPricing.V3QueryPricesAsync(
            new PricesQueryRequest
            {
                PackageId = "RA008",
                Pricing = new PricesQueryRequestPricing(
                    new PricesQueryRequestPricing.Cash(new PricingCash())
                ),
                Location = new Location { Zip = "80218" },
            }
        );
        Assert.That(
            response,
            Is.EqualTo(JsonUtils.Deserialize<ListEnvelopeProviderPackagePrice>(mockResponse))
                .UsingDefaults()
        );
    }

    [NUnit.Framework.Test]
    public async Task MockServerTest_5()
    {
        const string requestJson = """
            {
              "package_id": "OB002",
              "pricing": {
                "network_id": "-3776001016975145508",
                "type": "negotiated"
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
              "object": "list",
              "items": [
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
                  ]
                }
              ],
              "page": {
                "size": 1,
                "total": 1,
                "next_cursor": "next_cursor"
              },
              "no_data_reason": "no_data",
              "disclosures": [
                "disclosures"
              ]
            }
            """;

        Server
            .Given(
                WireMock
                    .RequestBuilders.Request.Create()
                    .WithPath("/v3/prices/query")
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

        var response = await Client.ConsumerPricing.V3QueryPricesAsync(
            new PricesQueryRequest
            {
                PackageId = "OB002",
                Pricing = new PricesQueryRequestPricing(
                    new PricesQueryRequestPricing.Negotiated(
                        new PricingNegotiated { NetworkId = "-3776001016975145508" }
                    )
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
            Is.EqualTo(JsonUtils.Deserialize<ListEnvelopeProviderPackagePrice>(mockResponse))
                .UsingDefaults()
        );
    }
}
