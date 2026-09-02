using NUnit.Framework;
using TurquoiseHealth.Api;
using TurquoiseHealth.Api.Core;

namespace TurquoiseHealth.Api.Test.Unit.MockServer;

[TestFixture]
public class V3ComparePersonalizedEstimatesTest : BaseMockServerTest
{
    [NUnit.Framework.Test]
    public async Task MockServerTest()
    {
        const string requestJson = """
            {
              "package_id": "GA003",
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
              "location": {
                "zip": "80218"
              }
            }
            """;

        const string mockResponse = """
            {
              "object": "price_comparison",
              "count": 8,
              "stats": {
                "min": {
                  "amount": "120.00",
                  "minor_units": 12000,
                  "currency": "USD"
                },
                "max": {
                  "amount": "540.00",
                  "minor_units": 54000,
                  "currency": "USD"
                },
                "avg": {
                  "amount": "310.25",
                  "minor_units": 31025,
                  "currency": "USD"
                },
                "median": {
                  "amount": "295.00",
                  "minor_units": 29500,
                  "currency": "USD"
                },
                "q1": {
                  "amount": "200.00",
                  "minor_units": 20000,
                  "currency": "USD"
                },
                "q3": {
                  "amount": "410.00",
                  "minor_units": 41000,
                  "currency": "USD"
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
                    .WithPath("/v3/personalized-estimates/compare")
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

        var response = await Client.ConsumerPricing.V3ComparePersonalizedEstimatesAsync(
            new ConsumerSitePriceComparisonInput
            {
                PackageId = "GA003",
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
                Location = new ConsumerSiteLocation { Zip = "80218" },
            }
        );
        Assert.That(
            response,
            Is.EqualTo(JsonUtils.Deserialize<ConsumerSitePriceComparison>(mockResponse))
                .UsingDefaults()
        );
    }
}
