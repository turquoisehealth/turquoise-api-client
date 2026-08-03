using TurquoiseHealth.Api;
using TurquoiseHealth.Api.Lib;

// Load environment variables from .env file
DotNetEnv.Env.Load();

// Create the auth handler once and reuse it — it caches the OAuth token in
// memory and refreshes it automatically. Building a new one per request
// bypasses that cache and can trip rate limits.
var auth = APIAuthHandler.FromClientCredentials();

var client = new TurquoiseHealthApiClient(auth.GetToken(), new ClientOptions
{
    BaseUrl = "https://api.turquoise.health"
});

try
{
    // Search for shoppable service packages by name.
    var packages = await client.ConsumerPricing.V3ListPackagesAsync(
        new V3ListPackagesRequest { Search = "MRI Brain" }
    );

    if (!packages.Items.Any())
    {
        Console.WriteLine("No matching service packages found.");
        return;
    }

    var ssp = packages.Items.First();
    Console.WriteLine($"Found package: {ssp.Name} ({ssp.Id})");

    // Get negotiated prices for that package near a given zip code / network.
    var prices = await client.ConsumerPricing.V3QueryPricesAsync(
        new V3PricesQueryRequest
        {
            PackageId = ssp.Id,
            Pricing = new V3PricesQueryRequestPricing(
                new V3PricesQueryRequestPricing.Negotiated(
                    new V3PricingNegotiated { NetworkId = "-3776001016975145508" }
                )
            ),
            Location = new V3Location { Zip = "90210" }
        }
    );

    foreach (var price in prices.Items)
    {
        Console.WriteLine($"{price.Provider.Name} {price.Total.Amount}");
    }
}
catch (Exception ex)
{
    Console.WriteLine($"Error: {ex.Message}");
    Environment.Exit(1);
}
