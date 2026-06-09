using System;
using System.Threading.Tasks;
using NUnit.Framework;
using TurquoiseHealth.Api;

namespace TurquoiseHealth.Api.Test.Integration;

/// <summary>
/// Integration tests for Turquoise Health API C# client.
/// These tests validate the client can successfully communicate with the API
/// and call various endpoints. Set the TURQUOISE_API_TOKEN environment variable
/// to run these tests.
/// </summary>
[TestFixture]
public class ConsumerPricingIntegrationTests
{
    private TurquoiseHealthApiClient? _client;

    [SetUp]
    public void Setup()
    {
        var token = Environment.GetEnvironmentVariable("TURQUOISE_API_TOKEN");
        if (string.IsNullOrEmpty(token))
        {
            Assert.Ignore("TURQUOISE_API_TOKEN environment variable not set");
        }

        _client = new TurquoiseHealthApiClient(
            token: token,
            clientOptions: new ClientOptions
            {
                BaseUrl = "https://api.turquoise.health"
            }
        );
    }

    [Test]
    public async Task TestListSSPs()
    {
        var response = await _client!.ConsumerPricing.GetSspsAsync(new GetSspsRequest
        {
            PageSize = 5
        });

        Assert.That(response, Is.Not.Null);
        Assert.That(response.Results, Is.Not.Null);
        Assert.That(response.Results, Is.InstanceOf<System.Collections.IEnumerable>());

        TestContext.WriteLine($"✓ Listed {response.Results.Count} SSPs");
    }

    [Test]
    public async Task TestSearchSSPsByName()
    {
        var response = await _client!.ConsumerPricing.GetSspsAsync(new GetSspsRequest
        {
            Search = "MRI",
            PageSize = 3
        });

        Assert.That(response, Is.Not.Null);
        Assert.That(response.Results, Is.Not.Null);

        TestContext.WriteLine($"✓ Found {response.Results.Count} SSPs matching 'MRI'");
    }

    [Test]
    public async Task TestListInsuranceNetworks()
    {
        var response = await _client!.ConsumerPricing.GetInsuranceNetworksAsync(new GetInsuranceNetworksRequest
        {
            PageSize = 5
        });

        Assert.That(response, Is.Not.Null);
        Assert.That(response.Results, Is.Not.Null);
        Assert.That(response.Results, Is.InstanceOf<System.Collections.IEnumerable>());

        TestContext.WriteLine($"✓ Listed {response.Results.Count} insurance networks");
    }

    [Test]
    public async Task TestListProviders()
    {
        var response = await _client!.ConsumerPricing.GetProvidersAsync(new GetProvidersRequest
        {
            PageSize = 5
        });

        Assert.That(response, Is.Not.Null);
        Assert.That(response.Results, Is.Not.Null);
        Assert.That(response.Results, Is.InstanceOf<System.Collections.IEnumerable>());

        TestContext.WriteLine($"✓ Listed {response.Results.Count} providers");
    }

    [Test]
    public async Task TestV2ListSSPs()
    {
        var response = await _client!.ConsumerPricing.V2ListSspsAsync(new V2ListSspsRequest
        {
            PageSize = 5
        });

        Assert.That(response, Is.Not.Null);
        Assert.That(response.Items, Is.Not.Null);
        Assert.That(response.Items, Is.InstanceOf<System.Collections.IEnumerable>());

        TestContext.WriteLine($"✓ V2: Listed {response.Items.Count} SSPs");
    }

    [Test]
    public async Task TestV2ListNetworks()
    {
        var response = await _client!.ConsumerPricing.V2ListNetworksAsync(new V2ListNetworksRequest
        {
            PageSize = 5
        });

        Assert.That(response, Is.Not.Null);
        Assert.That(response.Items, Is.Not.Null);
        Assert.That(response.Items, Is.InstanceOf<System.Collections.IEnumerable>());

        TestContext.WriteLine($"✓ V2: Listed {response.Items.Count} networks");
    }

    [Test]
    public async Task TestV2ListProviders()
    {
        var response = await _client!.ConsumerPricing.V2ListProvidersAsync(new V2ListProvidersRequest
        {
            PageSize = 5
        });

        Assert.That(response, Is.Not.Null);
        Assert.That(response.Items, Is.Not.Null);
        Assert.That(response.Items, Is.InstanceOf<System.Collections.IEnumerable>());

        TestContext.WriteLine($"✓ V2: Listed {response.Items.Count} providers");
    }
}
