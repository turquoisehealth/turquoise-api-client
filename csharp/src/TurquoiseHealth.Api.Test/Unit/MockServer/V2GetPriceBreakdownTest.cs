using NUnit.Framework;
using TurquoiseHealth.Api;
using TurquoiseHealth.Api.Core;

namespace TurquoiseHealth.Api.Test.Unit.MockServer;

[TestFixture]
public class V2GetPriceBreakdownTest : BaseMockServerTest
{
    [NUnit.Framework.Test]
    public async Task MockServerTest_1()
    {
        const string requestJson = """
            {
              "provider_id": "21929",
              "ssp_id": "RA011"
            }
            """;

        const string mockResponse = """
            {
              "data": {
                "rate_type": "cash",
                "provider_id": "0001",
                "ssp_id": "GA002",
                "network_id": "999999999999",
                "total_amount": "335.16",
                "total_amount_cents": 33516,
                "currency": "USD",
                "line_items": [
                  {
                    "line_code": "45385",
                    "code_type": "HCPCS",
                    "fee_type": "Base Code",
                    "description": "Colonoscopy",
                    "line_item_association_rate": 1
                  },
                  {
                    "line_code": "45385",
                    "code_type": "HCPCS",
                    "fee_type": "Professional Fee",
                    "description": "Colonoscopy",
                    "line_item_association_rate": 1
                  },
                  {
                    "line_code": "0636",
                    "code_type": "RC",
                    "fee_type": "Facility Fee",
                    "description": "Pharmacy - Extension of 025X - Drugs requiring detailed coding",
                    "line_item_association_rate": 0.633119
                  }
                ]
              },
              "no_data_reason": "no_data"
            }
            """;

        Server
            .Given(
                WireMock
                    .RequestBuilders.Request.Create()
                    .WithPath("/v2/consumer-pricing/prices/provider-breakdown")
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

        var response = await Client.ConsumerPricing.V2GetPriceBreakdownAsync(
            new ProviderBreakdownRequest { ProviderId = "21929", SspId = "RA011" }
        );
        Assert.That(
            response,
            Is.EqualTo(JsonUtils.Deserialize<SingleResourceEnvelopeRateBreakdown>(mockResponse))
                .UsingDefaults()
        );
    }

    [NUnit.Framework.Test]
    public async Task MockServerTest_2()
    {
        const string requestJson = """
            {
              "provider_id": "21929",
              "ssp_id": "RA011",
              "network_id": "8361580493441765265"
            }
            """;

        const string mockResponse = """
            {
              "data": {
                "rate_type": "cash",
                "provider_id": "0001",
                "ssp_id": "GA002",
                "network_id": "999999999999",
                "total_amount": "335.16",
                "total_amount_cents": 33516,
                "currency": "USD",
                "line_items": [
                  {
                    "line_code": "45385",
                    "code_type": "HCPCS",
                    "fee_type": "Base Code",
                    "description": "Colonoscopy",
                    "line_item_association_rate": 1
                  },
                  {
                    "line_code": "45385",
                    "code_type": "HCPCS",
                    "fee_type": "Professional Fee",
                    "description": "Colonoscopy",
                    "line_item_association_rate": 1
                  },
                  {
                    "line_code": "0636",
                    "code_type": "RC",
                    "fee_type": "Facility Fee",
                    "description": "Pharmacy - Extension of 025X - Drugs requiring detailed coding",
                    "line_item_association_rate": 0.633119
                  }
                ]
              },
              "no_data_reason": "no_data"
            }
            """;

        Server
            .Given(
                WireMock
                    .RequestBuilders.Request.Create()
                    .WithPath("/v2/consumer-pricing/prices/provider-breakdown")
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

        var response = await Client.ConsumerPricing.V2GetPriceBreakdownAsync(
            new ProviderBreakdownRequest
            {
                ProviderId = "21929",
                SspId = "RA011",
                NetworkId = "8361580493441765265",
            }
        );
        Assert.That(
            response,
            Is.EqualTo(JsonUtils.Deserialize<SingleResourceEnvelopeRateBreakdown>(mockResponse))
                .UsingDefaults()
        );
    }
}
