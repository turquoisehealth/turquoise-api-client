using NUnit.Framework;
using TurquoiseHealth.Api;
using TurquoiseHealth.Api.Core;

namespace TurquoiseHealth.Api.Test.Unit.MockServer;

[TestFixture]
public class V2ListSspsTest : BaseMockServerTest
{
    [NUnit.Framework.Test]
    public async Task MockServerTest_1()
    {
        const string mockResponse = """
            {
              "items": [
                {
                  "id": "id",
                  "name": "name",
                  "patient_description": "patient_description"
                },
                {
                  "id": "id",
                  "name": "name",
                  "patient_description": "patient_description"
                }
              ],
              "page": {
                "size": 1,
                "total": 1,
                "next_cursor": "next_cursor"
              },
              "no_data_reason": "no_data"
            }
            """;

        Server
            .Given(
                WireMock
                    .RequestBuilders.Request.Create()
                    .WithPath("/v2/consumer-pricing/ssps")
                    .UsingGet()
            )
            .RespondWith(
                WireMock
                    .ResponseBuilders.Response.Create()
                    .WithStatusCode(200)
                    .WithBody(mockResponse)
            );

        var response = await Client.ConsumerPricing.V2ListSspsAsync(new V2ListSspsRequest());
        Assert.That(
            response,
            Is.EqualTo(JsonUtils.Deserialize<EnvelopeSsp>(mockResponse)).UsingDefaults()
        );
    }

    [NUnit.Framework.Test]
    public async Task MockServerTest_2()
    {
        const string mockResponse = """
            {
              "items": [
                {
                  "id": "GA002",
                  "name": "Colonoscopy",
                  "patient_description": "A colonoscopy is a standard medical procedure used to diagnose and treat gastrointestinal tract conditions, particularly those affecting the large intestine (rectum, colon, and anus) and a portion of the small intestine (the ileum)."
                }
              ],
              "page": {
                "size": 2,
                "total": 2,
                "next_cursor": "next_cursor"
              },
              "no_data_reason": "no_data"
            }
            """;

        Server
            .Given(
                WireMock
                    .RequestBuilders.Request.Create()
                    .WithPath("/v2/consumer-pricing/ssps")
                    .UsingGet()
            )
            .RespondWith(
                WireMock
                    .ResponseBuilders.Response.Create()
                    .WithStatusCode(200)
                    .WithBody(mockResponse)
            );

        var response = await Client.ConsumerPricing.V2ListSspsAsync(new V2ListSspsRequest());
        Assert.That(
            response,
            Is.EqualTo(JsonUtils.Deserialize<EnvelopeSsp>(mockResponse)).UsingDefaults()
        );
    }
}
