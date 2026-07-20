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

        _client = new TurquoiseHealthApiClient(new ClientOptions
        {
            Environment = BaseUrl,
            Token = _apiToken
        });
    }

    #region V1 Endpoints

    [Test]
    public async Task GetSSPs_ShouldReturnResults()
    {
        // Arrange & Act
        var response = await _client!.ConsumerPricing.GetSSPsAsync(new GetSSPsRequest
        {
            PageSize = 5
        });

        // Assert
        Assert.That(response, Is.Not.Null);
        Assert.That(response.Data, Is.Not.Null);
        Assert.That(response.Data, Is.Not.Empty);
        Assert.That(response.Meta, Is.Not.Null);

        // Verify SSP structure
        var ssp = response.Data[0];
        Assert.That(ssp.Id, Is.Not.Null);
        Assert.That(ssp.Name, Is.Not.Null);

        Console.WriteLine($"✓ Retrieved {response.Data.Count} SSPs");
    }

    [Test]
    public async Task GetSSPs_WithSearch_ShouldReturnResults()
    {
        // Arrange & Act
        var response = await _client!.ConsumerPricing.GetSSPsAsync(new GetSSPsRequest
        {
            Search = "MRI",
            PageSize = 5
        });

        // Assert
        Assert.That(response, Is.Not.Null);
        Assert.That(response.Data, Is.Not.Null);

        Console.WriteLine($"✓ Search returned {response.Data.Count} SSPs");
    }

    [Test]
    public async Task GetInsuranceNetworks_ShouldReturnResults()
    {
        // Arrange & Act
        var response = await _client!.ConsumerPricing.GetInsuranceNetworksAsync(new GetInsuranceNetworksRequest
        {
            PageSize = 5
        });

        // Assert
        Assert.That(response, Is.Not.Null);
        Assert.That(response.Data, Is.Not.Null);
        Assert.That(response.Data, Is.Not.Empty);
        Assert.That(response.Meta, Is.Not.Null);

        // Verify network structure
        var network = response.Data[0];
        Assert.That(network.Id, Is.Not.Null);

        Console.WriteLine($"✓ Retrieved {response.Data.Count} insurance networks");
    }

    [Test]
    public async Task GetProviders_ShouldReturnResults()
    {
        // Arrange & Act
        var response = await _client!.ConsumerPricing.GetProvidersAsync(new GetProvidersRequest
        {
            State = "CA",
            PageSize = 5
        });

        // Assert
        Assert.That(response, Is.Not.Null);
        Assert.That(response.Data, Is.Not.Null);
        Assert.That(response.Data, Is.Not.Empty);
        Assert.That(response.Meta, Is.Not.Null);

        // Verify provider structure
        var provider = response.Data[0];
        Assert.That(provider.Id, Is.Not.Null);

        Console.WriteLine($"✓ Retrieved {response.Data.Count} providers");
    }

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
        Assert.That(response.Data, Is.Not.Null);
        Assert.That(response.Data, Is.Not.Empty);

        // Verify SSP structure
        var ssp = response.Data[0];
        Assert.That(ssp.Id, Is.Not.Null);
        Assert.That(ssp.Name, Is.Not.Null);

        Console.WriteLine($"✓ V2: Retrieved {response.Data.Count} SSPs");
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
        Assert.That(response.Data, Is.Not.Null);
        Assert.That(response.Data, Is.Not.Empty);

        // Verify network structure
        var network = response.Data[0];
        Assert.That(network.Id, Is.Not.Null);

        Console.WriteLine($"✓ V2: Retrieved {response.Data.Count} networks");
    }

    [Test]
    public async Task V2ListProviders_ShouldReturnResults()
    {
        // Arrange & Act
        var response = await _client!.ConsumerPricing.V2ListProvidersAsync(new V2ListProvidersRequest
        {
            State = "CA",
            PageSize = 5
        });

        // Assert
        Assert.That(response, Is.Not.Null);
        Assert.That(response.Data, Is.Not.Null);
        Assert.That(response.Data, Is.Not.Empty);

        // Verify provider structure
        var provider = response.Data[0];
        Assert.That(provider.Id, Is.Not.Null);

        Console.WriteLine($"✓ V2: Retrieved {response.Data.Count} providers");
    }

    #endregion
}
