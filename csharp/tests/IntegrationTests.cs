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
    private TurquoisehealthApiClient? _client;
    private readonly string? _apiToken = Environment.GetEnvironmentVariable("TURQUOISE_API_TOKEN");

    [SetUp]
    public void Setup()
    {
        if (string.IsNullOrEmpty(_apiToken))
        {
            Assert.Ignore("TURQUOISE_API_TOKEN environment variable not set");
        }

        _client = new TurquoisehealthApiClient(_apiToken, new ClientOptions
        {
            BaseUrl = BaseUrl
        });
    }

    #region V1 Endpoints
    // V1 endpoints have been removed. Only V2 and V3 endpoints are available.
    // See V2 Endpoints section below for updated tests.
    #endregion

    #region V2 Endpoints

    [Test]
    public async Task V2ListSsps_ShouldReturnResults()
    {
        // Arrange & Act
        var response = await _client!.ConsumerPricing.V2ListSspsAsync(new V2ListSspsRequest
        {
            PageSize = 5
        });

        // Assert
        Assert.That(response, Is.Not.Null);
        Assert.That(response.Items, Is.Not.Null);
        Assert.That(response.Items, Is.Not.Empty);
        Assert.That(response.Page, Is.Not.Null);

        // Verify SSP structure
        var ssp = response.Items.First();
        Assert.That(ssp.Id, Is.Not.Null);
        Assert.That(ssp.Name, Is.Not.Null);

        Console.WriteLine($"✓ V2: Retrieved {response.Items.Count()} SSPs");
    }

    [Test]
    public async Task V2ListNetworks_ShouldReturnResults()
    {
        // Arrange & Act
        var response = await _client!.ConsumerPricing.V2ListNetworksAsync(new V2ListNetworksRequest
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

        Console.WriteLine($"✓ V2: Retrieved {response.Items.Count()} networks");
    }

    [Test]
    public async Task V2ListProviders_ShouldReturnResults()
    {
        // Arrange & Act
        var response = await _client!.ConsumerPricing.V2ListProvidersAsync(new V2ListProvidersRequest
        {
            WithinState = "CA",
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

        Console.WriteLine($"✓ V2: Retrieved {response.Items.Count()} providers");
    }

    #endregion
}
