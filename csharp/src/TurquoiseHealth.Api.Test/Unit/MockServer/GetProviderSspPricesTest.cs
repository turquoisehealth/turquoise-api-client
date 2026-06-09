using NUnit.Framework;
using OneOf;
using TurquoiseHealth.Api;
using TurquoiseHealth.Api.Core;

namespace TurquoiseHealth.Api.Test.Unit.MockServer;

[TestFixture]
public class GetProviderSspPricesTest : BaseMockServerTest
{
    [NUnit.Framework.Test]
    public async Task MockServerTest_1()
    {
        const string requestJson = """
            {}
            """;

        const string mockResponse = """
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
              "price": {
                "max_price": 1.1,
                "max_price_billing_code": "max_price_billing_code",
                "network_id": "network_id",
                "provider_id": 1,
                "ssp_id": "ssp_id",
                "line_items": [
                  {
                    "line_code": "line_code",
                    "code_type": "code_type",
                    "fee_type": "fee_type",
                    "description": "description",
                    "line_item_association_rate": 1.1,
                    "mrf_rate": 1.1,
                    "benefit_category": "benefit_category"
                  },
                  {
                    "line_code": "line_code",
                    "code_type": "code_type",
                    "fee_type": "fee_type",
                    "description": "description",
                    "line_item_association_rate": 1.1,
                    "mrf_rate": 1.1,
                    "benefit_category": "benefit_category"
                  }
                ]
              }
            }
            """;

        Server
            .Given(
                WireMock
                    .RequestBuilders.Request.Create()
                    .WithPath("/v1/consumer-pricing/ssps/ssp_id/providers/provider_id/prices")
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

        var response = await Client.ConsumerPricing.GetProviderSspPricesAsync(
            new ProviderPackageBreakdownRequest
            {
                SspId = "ssp_id",
                ProviderId = "provider_id",
                NetworkId = null,
            }
        );
        Assert.That(
            response.Value,
            Is.EqualTo(
                    JsonUtils
                        .Deserialize<OneOf<ProviderBreakdown, NoDataResponse>>(mockResponse)
                        .Value
                )
                .UsingDefaults()
        );
    }

    [NUnit.Framework.Test]
    public async Task MockServerTest_2()
    {
        const string requestJson = """
            {}
            """;

        const string mockResponse = """
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
              "price": {
                "max_price": 1.1,
                "max_price_billing_code": "max_price_billing_code",
                "network_id": "network_id",
                "provider_id": 1,
                "ssp_id": "ssp_id",
                "line_items": [
                  {
                    "line_code": "line_code",
                    "code_type": "code_type",
                    "fee_type": "fee_type",
                    "description": "description",
                    "line_item_association_rate": 1.1
                  }
                ]
              }
            }
            """;

        Server
            .Given(
                WireMock
                    .RequestBuilders.Request.Create()
                    .WithPath("/v1/consumer-pricing/ssps/DE000/providers/provider_id/prices")
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

        var response = await Client.ConsumerPricing.GetProviderSspPricesAsync(
            new ProviderPackageBreakdownRequest { SspId = "DE000", ProviderId = "provider_id" }
        );
        Assert.That(
            response.Value,
            Is.EqualTo(
                    JsonUtils
                        .Deserialize<OneOf<ProviderBreakdown, NoDataResponse>>(mockResponse)
                        .Value
                )
                .UsingDefaults()
        );
    }

    [NUnit.Framework.Test]
    public async Task MockServerTest_3()
    {
        const string requestJson = """
            {
              "network_id": "8361580493441765265"
            }
            """;

        const string mockResponse = """
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
              "price": {
                "max_price": 1.1,
                "max_price_billing_code": "max_price_billing_code",
                "network_id": "network_id",
                "provider_id": 1,
                "ssp_id": "ssp_id",
                "line_items": [
                  {
                    "line_code": "line_code",
                    "code_type": "code_type",
                    "fee_type": "fee_type",
                    "description": "description",
                    "line_item_association_rate": 1.1
                  }
                ]
              }
            }
            """;

        Server
            .Given(
                WireMock
                    .RequestBuilders.Request.Create()
                    .WithPath("/v1/consumer-pricing/ssps/DE000/providers/provider_id/prices")
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

        var response = await Client.ConsumerPricing.GetProviderSspPricesAsync(
            new ProviderPackageBreakdownRequest
            {
                SspId = "DE000",
                ProviderId = "provider_id",
                NetworkId = "8361580493441765265",
            }
        );
        Assert.That(
            response.Value,
            Is.EqualTo(
                    JsonUtils
                        .Deserialize<OneOf<ProviderBreakdown, NoDataResponse>>(mockResponse)
                        .Value
                )
                .UsingDefaults()
        );
    }
}
