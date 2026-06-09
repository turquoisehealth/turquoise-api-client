using NUnit.Framework;
using TurquoiseHealth.Api;
using TurquoiseHealth.Api.Core;

namespace TurquoiseHealth.Api.Test.Unit.MockServer;

[TestFixture]
public class GetProvidersTest : BaseMockServerTest
{
    [NUnit.Framework.Test]
    public async Task MockServerTest_1()
    {
        const string mockResponse = """
            {
              "results": [
                {
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
                {
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
                }
              ],
              "page_size": 1,
              "count": 1,
              "no_data_reason": "no_data"
            }
            """;

        Server
            .Given(
                WireMock
                    .RequestBuilders.Request.Create()
                    .WithPath("/v1/consumer-pricing/providers")
                    .UsingGet()
            )
            .RespondWith(
                WireMock
                    .ResponseBuilders.Response.Create()
                    .WithStatusCode(200)
                    .WithBody(mockResponse)
            );

        var response = await Client.ConsumerPricing.GetProvidersAsync(new GetProvidersRequest());
        Assert.That(
            response,
            Is.EqualTo(JsonUtils.Deserialize<ProviderPage>(mockResponse)).UsingDefaults()
        );
    }

    [NUnit.Framework.Test]
    public async Task MockServerTest_2()
    {
        const string mockResponse = """
            {
              "results": [
                {
                  "provider_id": "provider_id",
                  "provider_name": "provider_name",
                  "provider_type": "provider_type",
                  "npi": "npi",
                  "city": "city",
                  "state": "state",
                  "zip_code": "zip_code",
                  "location_details": {
                    "street_address": "street_address"
                  }
                }
              ],
              "page_size": 1,
              "count": 1,
              "no_data_reason": "no_data"
            }
            """;

        Server
            .Given(
                WireMock
                    .RequestBuilders.Request.Create()
                    .WithPath("/v1/consumer-pricing/providers")
                    .UsingGet()
            )
            .RespondWith(
                WireMock
                    .ResponseBuilders.Response.Create()
                    .WithStatusCode(200)
                    .WithBody(mockResponse)
            );

        var response = await Client.ConsumerPricing.GetProvidersAsync(new GetProvidersRequest());
        Assert.That(
            response,
            Is.EqualTo(JsonUtils.Deserialize<ProviderPage>(mockResponse)).UsingDefaults()
        );
    }
}
