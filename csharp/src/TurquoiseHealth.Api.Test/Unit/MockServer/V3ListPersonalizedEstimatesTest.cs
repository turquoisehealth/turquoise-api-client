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
              "package_id": "GP03Z",
              "provider_id": "5754",
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
              },
              "sort": "total",
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
                    },
                    "context": {
                      "distance_m": 1738.3
                    }
                  },
                  "package": {
                    "id": "GP03Z",
                    "name": "Colonoscopy"
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
                  "total_allowed_amount": {
                    "amount": "3400.94",
                    "minor_units": 340094,
                    "currency": "USD"
                  },
                  "member_cost_share": {
                    "total": {
                      "amount": "1035.66",
                      "minor_units": 103566,
                      "currency": "USD"
                    },
                    "amount_towards_deductible": {
                      "amount": "500.00",
                      "minor_units": 50000,
                      "currency": "USD"
                    },
                    "amount_towards_copayment": {
                      "amount": "0.00",
                      "minor_units": 0,
                      "currency": "USD"
                    },
                    "amount_towards_coinsurance": {
                      "amount": "535.66",
                      "minor_units": 53566,
                      "currency": "USD"
                    },
                    "is_deductible_met": true,
                    "is_out_of_pocket_max_met": false
                  },
                  "sub_package_id": "133GA",
                  "line_items": [
                    {
                      "code": "45385",
                      "code_type": "HCPCS",
                      "fee_type": "base_code",
                      "description": "Colonoscopy with Snare Resection of Tumor/Polyps/lesions",
                      "association_rate": 0.964
                    },
                    {
                      "code": "45385",
                      "code_type": "HCPCS",
                      "fee_type": "professional_fee",
                      "description": "Colonoscopy with Snare Resection of Tumor/Polyps/lesions",
                      "association_rate": 0.964
                    },
                    {
                      "code": "45384",
                      "code_type": "HCPCS",
                      "fee_type": "base_code",
                      "description": "Flexible Endoscopic Colonoscopy with Hot Biopsy Forceps Excision of Lesion",
                      "association_rate": 0.0255
                    },
                    {
                      "code": "45384",
                      "code_type": "HCPCS",
                      "fee_type": "professional_fee",
                      "description": "Flexible Endoscopic Colonoscopy with Hot Biopsy Forceps Excision of Lesion",
                      "association_rate": 0.0255
                    },
                    {
                      "code": "45388",
                      "code_type": "HCPCS",
                      "fee_type": "base_code",
                      "description": "Flexible Colonoscopy with Ablation of Lesions (including Dilation/Guidewire)",
                      "association_rate": 0.0097
                    },
                    {
                      "code": "45388",
                      "code_type": "HCPCS",
                      "fee_type": "professional_fee",
                      "description": "Flexible Colonoscopy with Ablation of Lesions (including Dilation/Guidewire)",
                      "association_rate": 0.0097
                    },
                    {
                      "code": "45379",
                      "code_type": "HCPCS",
                      "fee_type": "base_code",
                      "description": "Flexible Colonoscopy with Foreign Body Removal",
                      "association_rate": 0.0008
                    },
                    {
                      "code": "45379",
                      "code_type": "HCPCS",
                      "fee_type": "professional_fee",
                      "description": "Flexible Colonoscopy with Foreign Body Removal",
                      "association_rate": 0.0008
                    },
                    {
                      "code": "88305",
                      "code_type": "HCPCS",
                      "fee_type": "facility_fee",
                      "description": "Microscopic/gross-exam of surgical pathology biopsies/exam/resections",
                      "association_rate": 0.7991
                    },
                    {
                      "code": "0636",
                      "code_type": "RC",
                      "fee_type": "facility_fee",
                      "description": "Pharmacy - Extension of 025X - Drugs requiring detailed coding",
                      "association_rate": 0.6621
                    },
                    {
                      "code": "0370",
                      "code_type": "RC",
                      "fee_type": "facility_fee",
                      "description": "Anesthesia - General",
                      "association_rate": 0.6471
                    },
                    {
                      "code": "J2704",
                      "code_type": "HCPCS",
                      "fee_type": "facility_fee",
                      "description": "Intramuscular injection of 10mg propofol",
                      "association_rate": 0.6354
                    },
                    {
                      "code": "0750",
                      "code_type": "RC",
                      "fee_type": "facility_fee",
                      "description": "Gastrointestinal Services - General",
                      "association_rate": 0.6023
                    },
                    {
                      "code": "0710",
                      "code_type": "RC",
                      "fee_type": "facility_fee",
                      "description": "Recovery Room - General",
                      "association_rate": 0.5556
                    },
                    {
                      "code": "0250",
                      "code_type": "RC",
                      "fee_type": "facility_fee",
                      "description": "Pharmacy (Also see 063X, an extension of 250X) - General",
                      "association_rate": 0.4192
                    },
                    {
                      "code": "0272",
                      "code_type": "RC",
                      "fee_type": "optional_fee",
                      "description": "Medical/Surgical Supplies and Devices (Also see 062X, an extension of 027X) - Sterile",
                      "association_rate": 0.3769
                    },
                    {
                      "code": "0310",
                      "code_type": "RC",
                      "fee_type": "optional_fee",
                      "description": "Laboratory Pathology - General",
                      "association_rate": 0.3528
                    }
                  ]
                }
              ],
              "page": {
                "size": 1,
                "total": 1
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
                PackageId = "GP03Z",
                ProviderId = "5754",
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
                Sort = ConsumerSitePriceSort.Total,
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
