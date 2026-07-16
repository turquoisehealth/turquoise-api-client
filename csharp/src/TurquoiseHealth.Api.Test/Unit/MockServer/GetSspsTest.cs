using NUnit.Framework;
using TurquoiseHealth.Api;
using TurquoiseHealth.Api.Core;

namespace TurquoiseHealth.Api.Test.Unit.MockServer;

[TestFixture]
public class GetSspsTest : BaseMockServerTest
{
    [NUnit.Framework.Test]
    public async Task MockServerTest()
    {
        const string mockResponse = """
            {
              "results": [
                {
                  "id": "id",
                  "name": "name",
                  "patient_description": "patient_description"
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
                    .WithPath("/v1/consumer-pricing/ssps")
                    .UsingGet()
            )
            .RespondWith(
                WireMock
                    .ResponseBuilders.Response.Create()
                    .WithStatusCode(200)
                    .WithBody(mockResponse)
            );

        var response = await Client.ConsumerPricing.GetSspsAsync(new GetSspsRequest());
        Assert.That(
            response,
            Is.EqualTo(JsonUtils.Deserialize<ServicePackagePage>(mockResponse)).UsingDefaults()
        );
    }
}
