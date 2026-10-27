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
              "object": "list",
              "items": [
                {
                  "object": "price",
                  "id": "prc_5754.RA007.-3776001016975145508",
                  "provider": {
                    "object": "provider",
                    "id": "5754",
                    "name": "HCA HealthONE Presbyterian St Lukes",
                    "type": "Short Term Acute Care Hospital",
                    "npi": "1720038946",
                    "address": {
                      "city": "Denver",
                      "state": "CO",
                      "zip_code": "80218",
                      "latitude": 39.746549,
                      "longitude": -104.966925
                    }
                  },
                  "package": {
                    "id": "R017Z",
                    "name": "Ultrasound"
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
                    "amount": "526.10",
                    "minor_units": 52610,
                    "currency": "USD"
                  },
                  "line_items": [
                    {
                      "code": "76642",
                      "code_type": "HCPCS",
                      "fee_type": "base_code",
                      "description": "Ultrasound imaging of one breast including axilla with image documentation",
                      "association_rate": 0.7245
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
                PackageId = "R017Z",
                ProviderId = "5754",
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
              "package_id": "R017Z",
              "provider_id": "5754",
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
                  "id": "prc_5754.RA007.-3776001016975145508",
                  "provider": {
                    "object": "provider",
                    "id": "5754",
                    "name": "HCA HealthONE Presbyterian St Lukes",
                    "type": "Short Term Acute Care Hospital",
                    "npi": "1720038946",
                    "address": {
                      "city": "Denver",
                      "state": "CO",
                      "zip_code": "80218",
                      "latitude": 39.746549,
                      "longitude": -104.966925
                    }
                  },
                  "package": {
                    "id": "R017Z",
                    "name": "Ultrasound"
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
                    "amount": "526.10",
                    "minor_units": 52610,
                    "currency": "USD"
                  },
                  "line_items": [
                    {
                      "code": "76642",
                      "code_type": "HCPCS",
                      "fee_type": "base_code",
                      "description": "Ultrasound imaging of one breast including axilla with image documentation",
                      "association_rate": 0.7245
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
                PackageId = "R017Z",
                ProviderId = "5754",
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
                  "id": "prc_5754.RA007.-3776001016975145508",
                  "provider": {
                    "object": "provider",
                    "id": "5754",
                    "name": "HCA HealthONE Presbyterian St Lukes",
                    "type": "Short Term Acute Care Hospital",
                    "npi": "1720038946",
                    "address": {
                      "city": "Denver",
                      "state": "CO",
                      "zip_code": "80218",
                      "latitude": 39.746549,
                      "longitude": -104.966925
                    }
                  },
                  "package": {
                    "id": "R017Z",
                    "name": "Ultrasound"
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
                    "amount": "526.10",
                    "minor_units": 52610,
                    "currency": "USD"
                  },
                  "line_items": [
                    {
                      "code": "76642",
                      "code_type": "HCPCS",
                      "fee_type": "base_code",
                      "description": "Ultrasound imaging of one breast including axilla with image documentation",
                      "association_rate": 0.7245
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
                PackageId = "R017Z",
                Pricing = new PricesQueryRequestPricing(
                    new PricesQueryRequestPricing.Cash(new PricingCash())
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
              "package_id": "R017Z",
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
                  "id": "prc_5754.RA007.-3776001016975145508",
                  "provider": {
                    "object": "provider",
                    "id": "5754",
                    "name": "HCA HealthONE Presbyterian St Lukes",
                    "type": "Short Term Acute Care Hospital",
                    "npi": "1720038946",
                    "address": {
                      "city": "Denver",
                      "state": "CO",
                      "zip_code": "80218",
                      "latitude": 39.746549,
                      "longitude": -104.966925
                    }
                  },
                  "package": {
                    "id": "R017Z",
                    "name": "Ultrasound"
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
                    "amount": "526.10",
                    "minor_units": 52610,
                    "currency": "USD"
                  },
                  "line_items": [
                    {
                      "code": "76642",
                      "code_type": "HCPCS",
                      "fee_type": "base_code",
                      "description": "Ultrasound imaging of one breast including axilla with image documentation",
                      "association_rate": 0.7245
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
                PackageId = "R017Z",
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
              "package_id": "R017Z",
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
                  "id": "prc_5754.RA007.-3776001016975145508",
                  "provider": {
                    "object": "provider",
                    "id": "5754",
                    "name": "HCA HealthONE Presbyterian St Lukes",
                    "type": "Short Term Acute Care Hospital",
                    "npi": "1720038946",
                    "address": {
                      "city": "Denver",
                      "state": "CO",
                      "zip_code": "80218",
                      "latitude": 39.746549,
                      "longitude": -104.966925
                    }
                  },
                  "package": {
                    "id": "R017Z",
                    "name": "Ultrasound"
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
                    "amount": "526.10",
                    "minor_units": 52610,
                    "currency": "USD"
                  },
                  "line_items": [
                    {
                      "code": "76642",
                      "code_type": "HCPCS",
                      "fee_type": "base_code",
                      "description": "Ultrasound imaging of one breast including axilla with image documentation",
                      "association_rate": 0.7245
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
                PackageId = "R017Z",
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
