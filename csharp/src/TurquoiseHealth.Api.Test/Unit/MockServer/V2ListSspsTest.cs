using NUnit.Framework;
using TurquoiseHealth.Api;
using TurquoiseHealth.Api.Core;

namespace TurquoiseHealth.Api.Test.Unit.MockServer;

[TestFixture]
public class V2ListSspsTest : BaseMockServerTest
{
    [NUnit.Framework.Test]
    public async Task MockServerTest()
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
