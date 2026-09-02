using NUnit.Framework;
using TurquoiseHealth.Api;
using TurquoiseHealth.Api.Core;

namespace TurquoiseHealth.Api.Test.Unit.MockServer;

[TestFixture]
public class V3ListPersonalizedEstimatesTest : BaseMockServerTest
{
    [NUnit.Framework.Test]
    public async Task MockServerTest()
    {
        const string requestJson = """
            {
              "package_id": "GA003",
              "provider_id": "5756",
              "pricing": {
                "type": "negotiated",
                "network_id": "-3776001016975145508"
              },
              "member_eligibility": {
                "first_name": "test123",
                "last_name": "test123",
                "date_of_birth": "2001-01-01",
                "member_id": "test123",
                "consent_attested": "consent_attested"
              },
              "sort_direction": "asc",
              "page_size": 25
            }
            """;

        const string mockResponse = """
            {
              "object": "list",
              "items": [
                {
                  "object": "personalized_estimate",
                  "provider": {
                    "id": "prov_01hxyz",
                    "name": "Example Medical Center"
                  },
                  "package": {
                    "id": "pkg_01hxyz",
                    "name": "Knee replacement - partial"
                  },
                  "pricing": {
                    "type": "negotiated",
                    "network": {
                      "id": "482910",
                      "name": "Example PPO Network"
                    },
                    "payer": {
                      "id": "payer_01hxyz",
                      "name": "Example Payer"
                    }
                  },
                  "total_allowed_amount": {
                    "amount": "620.00",
                    "minor_units": 62000,
                    "currency": "USD"
                  },
                  "member_cost_share": {
                    "total": {
                      "amount": "185.00",
                      "minor_units": 18500,
                      "currency": "USD"
                    },
                    "amount_towards_deductible": {
                      "amount": "50.00",
                      "minor_units": 5000,
                      "currency": "USD"
                    },
                    "amount_towards_copayment": {
                      "amount": "25.00",
                      "minor_units": 2500,
                      "currency": "USD"
                    },
                    "amount_towards_coinsurance": {
                      "amount": "110.00",
                      "minor_units": 11000,
                      "currency": "USD"
                    },
                    "is_deductible_met": false,
                    "is_out_of_pocket_max_met": false
                  },
                  "sub_package_id": "pkg_01hxyz-A",
                  "line_items": [
                    {
                      "code": "27442",
                      "code_type": "HCPCS",
                      "fee_type": "base_code",
                      "description": "Knee Joint Resurfacing or Reconstruction",
                      "association_rate": 0.91
                    },
                    {
                      "code": "27442",
                      "code_type": "HCPCS",
                      "fee_type": "professional_fee",
                      "description": "Knee Joint Resurfacing or Reconstruction",
                      "association_rate": 0.91
                    },
                    {
                      "code": "C1776",
                      "code_type": "HCPCS",
                      "fee_type": "facility_fee",
                      "description": "Implantable Joint Device for Motion Restoration",
                      "association_rate": 0.85
                    },
                    {
                      "code": "0370",
                      "code_type": "RC",
                      "fee_type": "facility_fee",
                      "description": "Anesthesia - General",
                      "association_rate": 0.94
                    },
                    {
                      "code": "97116",
                      "code_type": "HCPCS",
                      "fee_type": "optional_fee",
                      "description": "Therapeutic gait training of 1+ areas for 15 min each",
                      "association_rate": 0.35
                    }
                  ]
                }
              ],
              "page": {
                "size": 1,
                "total": 1
              },
              "disclosures": [
                "disclosures"
              ],
              "benefits_summary": {
                "remaining_deductible": 500,
                "total_deductible": 1500,
                "remaining_out_of_pocket_max": 2000,
                "total_out_of_pocket_max": 6000,
                "benefit_categories": [
                  {
                    "category": "office_visit",
                    "coinsurance": 0.2,
                    "copayment": 0,
                    "deductible": null
                  }
                ],
                "limitations": [
                  "limitations"
                ],
                "deductible_accumulator_type": "individual",
                "out_of_pocket_accumulator_type": "individual"
              }
            }
            """;

        Server
            .Given(
                WireMock
                    .RequestBuilders.Request.Create()
                    .WithPath("/v3/personalized-estimates")
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

        var response = await Client.ConsumerPricing.V3ListPersonalizedEstimatesAsync(
            new ConsumerSitePersonalizedEstimatesQueryRequest
            {
                PackageId = "GA003",
                ProviderId = "5756",
                Pricing = new ConsumerSitePricingNegotiated
                {
                    Type = "negotiated",
                    NetworkId = "-3776001016975145508",
                },
                MemberEligibility = new ConsumerSiteMemberEligibilityInput
                {
                    FirstName = "test123",
                    LastName = "test123",
                    DateOfBirth = new DateOnly(2001, 1, 1),
                    MemberId = "test123",
                    ConsentAttested = "consent_attested",
                },
                SortDirection = ConsumerSitePersonalizedEstimatesQueryRequestSortDirection.Asc,
                PageSize = 25,
            }
        );
        Assert.That(
            response,
            Is.EqualTo(JsonUtils.Deserialize<V3ListPersonalizedEstimatesResponse>(mockResponse))
                .UsingDefaults()
        );
    }
}
