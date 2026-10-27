using NUnit.Framework;
using TurquoiseHealth.Api;
using TurquoiseHealth.Api.Core;

namespace TurquoiseHealth.Api.Test.Unit.MockServer;

[TestFixture]
public class V3PersonalizedEstimateStatisticsTest : BaseMockServerTest
{
    [NUnit.Framework.Test]
    public async Task MockServerTest()
    {
        const string requestJson = """
            {
              "package_id": "GP03Z",
              "pricing": {
                "type": "negotiated",
                "network_id": "-3776001016975145508"
              },
              "member_eligibility": {
                "first_name": "test123",
                "last_name": "test123",
                "date_of_birth": "2001-01-01",
                "member_id": "test123",
                "consent_attested": true
              },
              "location": {
                "zip": "80218"
              }
            }
            """;

        const string mockResponse = """
            {
              "object": "estimate_statistics",
              "count": 41,
              "stats": {
                "min": {
                  "amount": "635.98",
                  "minor_units": 63598,
                  "currency": "USD"
                },
                "max": {
                  "amount": "1465.21",
                  "minor_units": 146521,
                  "currency": "USD"
                },
                "avg": {
                  "amount": "985.30",
                  "minor_units": 98530,
                  "currency": "USD"
                },
                "median": {
                  "amount": "955.53",
                  "minor_units": 95553,
                  "currency": "USD"
                },
                "q1": {
                  "amount": "785.16",
                  "minor_units": 78516,
                  "currency": "USD"
                },
                "q3": {
                  "amount": "1142.26",
                  "minor_units": 114226,
                  "currency": "USD"
                }
              },
              "disclosures": [
                "Results reflect services Turquoise has priced. Absence of a provider, network, or package does not imply the service is unavailable or out of network.",
                "Distance-based results exclude providers without location data (about 6% of providers)."
              ],
              "benefits_summary": {
                "remaining_deductible": 500,
                "total_deductible": 500,
                "remaining_out_of_pocket_max": 7585,
                "total_out_of_pocket_max": 7800,
                "benefit_categories": [
                  {
                    "category": "advanced-imaging",
                    "coinsurance": 0.2,
                    "copayment": 0,
                    "deductible": null
                  },
                  {
                    "category": "ambulatory-surgery-center",
                    "coinsurance": 0.2,
                    "copayment": 0,
                    "deductible": null
                  },
                  {
                    "category": "inpatient",
                    "coinsurance": 0.2,
                    "copayment": 0,
                    "deductible": null
                  },
                  {
                    "category": "lab",
                    "coinsurance": 0,
                    "copayment": 0,
                    "deductible": 0
                  },
                  {
                    "category": "office-visit-clinic",
                    "coinsurance": 0,
                    "copayment": 50,
                    "deductible": 0
                  },
                  {
                    "category": "outpatient",
                    "coinsurance": 0.2,
                    "copayment": 0,
                    "deductible": null
                  },
                  {
                    "category": "radiology",
                    "coinsurance": 0,
                    "copayment": 0,
                    "deductible": 0
                  }
                ],
                "limitations": [
                  "limitations"
                ],
                "deductible_accumulator_type": "embedded",
                "out_of_pocket_accumulator_type": "embedded"
              }
            }
            """;

        Server
            .Given(
                WireMock
                    .RequestBuilders.Request.Create()
                    .WithPath("/v3/personalized-estimates/statistics")
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

        var response = await Client.ConsumerPricing.V3PersonalizedEstimateStatisticsAsync(
            new ConsumerSiteEstimateStatisticsInput
            {
                PackageId = "GP03Z",
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
                    ConsentAttested = true,
                },
                Location = new ConsumerSiteLocation { Zip = "80218" },
            }
        );
        Assert.That(
            response,
            Is.EqualTo(
                    JsonUtils.Deserialize<V3PersonalizedEstimateStatisticsResponse>(mockResponse)
                )
                .UsingDefaults()
        );
    }
}
