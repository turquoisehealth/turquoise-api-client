using NUnit.Framework;
using TurquoiseHealth.Api;

namespace TurquoiseHealth.Api.IntegrationTests;

/// <summary>
/// Integration tests for the Turquoise Health API C# client.
///
/// These tests perform smoke testing by calling real API endpoints to validate that
/// the client can successfully initialize, authenticate, make requests, and parse responses.
///
/// Prerequisites:
///   - Set TURQUOISE_API_TOKEN environment variable
///   - Install dependencies: dotnet restore
///
/// Run tests:
///   dotnet test --verbosity normal
/// </summary>
public class IntegrationTests
{
    private const string BaseUrl = "https://api.turquoise.health";
    private TurquoiseHealthApiClient? _client;
    private readonly string? _apiToken = Environment.GetEnvironmentVariable("TURQUOISE_API_TOKEN");

    [SetUp]
    public void Setup()
    {
        if (string.IsNullOrEmpty(_apiToken))
        {
            Assert.Ignore("TURQUOISE_API_TOKEN environment variable not set");
        }

        _client = new TurquoiseHealthApiClient(_apiToken, new ClientOptions
        {
            BaseUrl = BaseUrl
        });
    }

    #region V3 Endpoints

    [Test]
    public async Task V3ListPackages_ShouldReturnResults()
    {
        // Arrange & Act
        var response = await _client!.ConsumerPricing.V3ListPackagesAsync(new V3ListPackagesRequest
        {
            PageSize = 5
        });

        // Assert
        Assert.That(response, Is.Not.Null);
        Assert.That(response.Items, Is.Not.Null);
        Assert.That(response.Items, Is.Not.Empty);
        Assert.That(response.Page, Is.Not.Null);

        // Verify package structure
        var package = response.Items.First();
        Assert.That(package.Id, Is.Not.Null);
        Assert.That(package.Name, Is.Not.Null);

        Console.WriteLine($"✓ V3: Retrieved {response.Items.Count()} packages");
    }

    [Test]
    public async Task V3ListNetworks_ShouldReturnResults()
    {
        // Arrange & Act
        var response = await _client!.ConsumerPricing.V3ListNetworksAsync(new V3ListNetworksRequest
        {
            PageSize = 5
        });

        // Assert
        Assert.That(response, Is.Not.Null);
        Assert.That(response.Items, Is.Not.Null);
        Assert.That(response.Items, Is.Not.Empty);
        Assert.That(response.Page, Is.Not.Null);

        // Verify network structure
        var network = response.Items.First();
        Assert.That(network.Id, Is.Not.Null);

        Console.WriteLine($"✓ V3: Retrieved {response.Items.Count()} networks");
    }

    [Test]
    public async Task V3ListProviders_ShouldReturnResults()
    {
        // Arrange & Act
        var response = await _client!.ConsumerPricing.V3ListProvidersAsync(new V3ListProvidersRequest
        {
            LocationWithinState = "CA",
            PageSize = 5
        });

        // Assert
        Assert.That(response, Is.Not.Null);
        Assert.That(response.Items, Is.Not.Null);
        Assert.That(response.Items, Is.Not.Empty);
        Assert.That(response.Page, Is.Not.Null);

        // Verify provider structure
        var provider = response.Items.First();
        Assert.That(provider.Id, Is.Not.Null);

        Console.WriteLine($"✓ V3: Retrieved {response.Items.Count()} providers");
    }

    #endregion
}
