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
              "object": "list",
              "items": [
                {
                  "object": "price",
                  "id": "prc_2751.RA007.2010265101",
                  "provider": {
                    "id": "2751",
                    "name": "Provident Hospital of Cook County",
                    "address": {
                      "city": "city",
                      "state": "state",
                      "zip_code": "zip_code"
                    }
                  },
                  "package": {
                    "id": "RA007",
                    "name": "MRI with Contrast"
                  },
                  "pricing": {
                    "type": "negotiated",
                    "network": {
                      "id": "2010265101",
                      "name": "Example PPO"
                    },
                    "payer": {
                      "id": "643",
                      "name": "Example Payer"
                    }
                  },
                  "total": {
                    "amount": "1250.00",
                    "minor_units": 125000,
                    "currency": "USD"
                  },
                  "line_items": [
                    {
                      "code": "45385",
                      "code_type": "HCPCS",
                      "fee_type": "base_code",
                      "description": "Colonoscopy",
                      "association_rate": 1
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
            new V3PricesQueryRequest
            {
                PackageId = "RA007",
                ProviderId = "2751",
                Pricing = new V3PricesQueryRequestPricing(
                    new V3PricesQueryRequestPricing.Negotiated(
                        new V3PricingNegotiated { NetworkId = "network_id" }
                    )
                ),
            }
        );
        Assert.That(
            response,
            Is.EqualTo(JsonUtils.Deserialize<V3ListEnvelopeProviderPackagePrice>(mockResponse))
                .UsingDefaults()
        );
    }

    [NUnit.Framework.Test]
    public async Task MockServerTest_2()
    {
        const string requestJson = """
            {
              "package_id": "RA007",
              "provider_id": "2751",
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
                  "id": "prc_2751.RA007.2010265101",
                  "provider": {
                    "id": "2751",
                    "name": "Provident Hospital of Cook County",
                    "address": {
                      "city": "city",
                      "state": "state",
                      "zip_code": "zip_code"
                    }
                  },
                  "package": {
                    "id": "RA007",
                    "name": "MRI with Contrast"
                  },
                  "pricing": {
                    "type": "negotiated",
                    "network": {
                      "id": "2010265101",
                      "name": "Example PPO"
                    },
                    "payer": {
                      "id": "643",
                      "name": "Example Payer"
                    }
                  },
                  "total": {
                    "amount": "1250.00",
                    "minor_units": 125000,
                    "currency": "USD"
                  },
                  "line_items": [
                    {
                      "code": "45385",
                      "code_type": "HCPCS",
                      "fee_type": "base_code",
                      "description": "Colonoscopy",
                      "association_rate": 1
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
            new V3PricesQueryRequest
            {
                PackageId = "RA007",
                ProviderId = "2751",
                Pricing = new V3PricesQueryRequestPricing(
                    new V3PricesQueryRequestPricing.Cash(new V3PricingCash())
                ),
            }
        );
        Assert.That(
            response,
            Is.EqualTo(JsonUtils.Deserialize<V3ListEnvelopeProviderPackagePrice>(mockResponse))
                .UsingDefaults()
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
                  "id": "prc_2751.RA007.2010265101",
                  "provider": {
                    "id": "2751",
                    "name": "Provident Hospital of Cook County",
                    "address": {
                      "city": "city",
                      "state": "state",
                      "zip_code": "zip_code"
                    }
                  },
                  "package": {
                    "id": "RA007",
                    "name": "MRI with Contrast"
                  },
                  "pricing": {
                    "type": "negotiated",
                    "network": {
                      "id": "2010265101",
                      "name": "Example PPO"
                    },
                    "payer": {
                      "id": "643",
                      "name": "Example Payer"
                    }
                  },
                  "total": {
                    "amount": "1250.00",
                    "minor_units": 125000,
                    "currency": "USD"
                  },
                  "line_items": [
                    {
                      "code": "45385",
                      "code_type": "HCPCS",
                      "fee_type": "base_code",
                      "description": "Colonoscopy",
                      "association_rate": 1
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
            new V3PricesQueryRequest
            {
                PackageId = "RA007",
                Pricing = new V3PricesQueryRequestPricing(
                    new V3PricesQueryRequestPricing.Cash(new V3PricingCash())
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
                Sort = V3PriceSort.Distance,
            }
        );
        Assert.That(
            response,
            Is.EqualTo(JsonUtils.Deserialize<V3ListEnvelopeProviderPackagePrice>(mockResponse))
                .UsingDefaults()
        );
    }

    [NUnit.Framework.Test]
    public async Task MockServerTest_4()
    {
        const string requestJson = """
            {
              "package_id": "RA007",
              "pricing": {
                "type": "cash"
              },
              "location": {
                "zip": "60644"
              }
            }
            """;

        const string mockResponse = """
            {
              "object": "list",
              "items": [
                {
                  "object": "price",
                  "id": "prc_2751.RA007.2010265101",
                  "provider": {
                    "id": "2751",
                    "name": "Provident Hospital of Cook County",
                    "address": {
                      "city": "city",
                      "state": "state",
                      "zip_code": "zip_code"
                    }
                  },
                  "package": {
                    "id": "RA007",
                    "name": "MRI with Contrast"
                  },
                  "pricing": {
                    "type": "negotiated",
                    "network": {
                      "id": "2010265101",
                      "name": "Example PPO"
                    },
                    "payer": {
                      "id": "643",
                      "name": "Example Payer"
                    }
                  },
                  "total": {
                    "amount": "1250.00",
                    "minor_units": 125000,
                    "currency": "USD"
                  },
                  "line_items": [
                    {
                      "code": "45385",
                      "code_type": "HCPCS",
                      "fee_type": "base_code",
                      "description": "Colonoscopy",
                      "association_rate": 1
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
            new V3PricesQueryRequest
            {
                PackageId = "RA007",
                Pricing = new V3PricesQueryRequestPricing(
                    new V3PricesQueryRequestPricing.Cash(new V3PricingCash())
                ),
                Location = new V3Location { Zip = "60644" },
            }
        );
        Assert.That(
            response,
            Is.EqualTo(JsonUtils.Deserialize<V3ListEnvelopeProviderPackagePrice>(mockResponse))
                .UsingDefaults()
        );
    }

    [NUnit.Framework.Test]
    public async Task MockServerTest_5()
    {
        const string requestJson = """
            {
              "package_id": "RA007",
              "pricing": {
                "network_id": "network_id",
                "type": "negotiated"
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
              "object": "list",
              "items": [
                {
                  "object": "price",
                  "id": "prc_2751.RA007.2010265101",
                  "provider": {
                    "id": "2751",
                    "name": "Provident Hospital of Cook County",
                    "address": {
                      "city": "city",
                      "state": "state",
                      "zip_code": "zip_code"
                    }
                  },
                  "package": {
                    "id": "RA007",
                    "name": "MRI with Contrast"
                  },
                  "pricing": {
                    "type": "negotiated",
                    "network": {
                      "id": "2010265101",
                      "name": "Example PPO"
                    },
                    "payer": {
                      "id": "643",
                      "name": "Example Payer"
                    }
                  },
                  "total": {
                    "amount": "1250.00",
                    "minor_units": 125000,
                    "currency": "USD"
                  },
                  "line_items": [
                    {
                      "code": "45385",
                      "code_type": "HCPCS",
                      "fee_type": "base_code",
                      "description": "Colonoscopy",
                      "association_rate": 1
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
            new V3PricesQueryRequest
            {
                PackageId = "RA007",
                Pricing = new V3PricesQueryRequestPricing(
                    new V3PricesQueryRequestPricing.Negotiated(
                        new V3PricingNegotiated { NetworkId = "network_id" }
                    )
                ),
                Location = new V3Location { Within = new V3LocationWithin { State = "IL" } },
            }
        );
        Assert.That(
            response,
            Is.EqualTo(JsonUtils.Deserialize<V3ListEnvelopeProviderPackagePrice>(mockResponse))
                .UsingDefaults()
        );
    }
}
