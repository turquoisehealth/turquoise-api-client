# Turquoise Health API Client

[![PyPI](https://img.shields.io/pypi/v/turquoisehealth_api)](https://pypi.org/project/turquoisehealth-api/)
[![npm](https://img.shields.io/npm/v/@turquoisehealth/api)](https://www.npmjs.com/package/@turquoisehealth/api)
[![NuGet](https://img.shields.io/nuget/v/TurquoiseHealth.Api)](https://www.nuget.org/packages/TurquoiseHealth.Api)

Official client libraries for the [Turquoise Health Consumer Pricing API](https://turquoise.health/api/docs/).

---

## Installation

### Python

```bash
pip install turquoisehealth-api
```

### TypeScript

```bash
npm install @turquoisehealth/api
```

### C\#

```bash
dotnet add package TurquoiseHealth.Api
```

---

## Quickstart

All requests require a Bearer token. Contact [Turquoise Health](https://turquoise.health) to obtain API credentials.

Each SDK includes an `APIAuthHandler` utility for managing OAuth authentication with automatic token refresh. You will need to set your credentials as environment variables:

```bash
export TURQUOISE_CLIENT_ID="your-client-id"
export TURQUOISE_CLIENT_SECRET="your-client-secret"
export TURQUOISE_ORGANIZATION_ID="your-org-id"
```

**Python:**

```python
from turquoise_health import TurquoiseHealth, APIAuthHandler

auth = APIAuthHandler.from_client_credentials()
client = TurquoiseHealth(base_url="https://api.turquoise.health", token=auth.as_callable())
```

**TypeScript:**

```typescript
import { TurquoiseHealthApiClient, lib } from "@turquoisehealth/api";

const auth = lib.APIAuthHandler.fromClientCredentials();
const client = new TurquoiseHealthApiClient({
  environment: "https://api.turquoise.health",
  token: auth.asSupplier(),
});
```

**C#:**

```csharp
using TurquoiseHealth.Api.Lib;

var auth = APIAuthHandler.FromClientCredentials();
var client = new TurquoiseHealthApiClient(auth.GetToken(), new ClientOptions
{
    BaseUrl = "https://api.turquoise.health"
});
```

> **⚠️ Important**: Create the `APIAuthHandler` instance **once** and reuse it across your application (e.g., as a singleton or module-level variable). Each auth handler maintains an in-memory token cache with automatic refresh. Creating new instances for every request will bypass the cache and unnecessarily refetch tokens from the OAuth server, leading to performance degradation and rate limiting errors.

---

## Using the client

Once you have initialized the client with an auth handler, you can begin calling the api endpoints.

### Python

```python
# Search for shoppable service packages (SSPs)
packages = client.consumer_pricing.v3list_packages(search="MRI Brain")
for package in packages.items:
    print(package.name, package.id)

# Get negotiated prices for a package near a zip code / network
prices = client.consumer_pricing.v3query_prices(
    package_id=packages.items[0].id,
    pricing={"type": "negotiated", "network_id": "your-network-id"},
    location={"zip": "90210"},
)
for price in prices.items:
    print(price.provider.name, price.total.amount)
```

### TypeScript / JavaScript

```typescript
// Search for shoppable service packages (SSPs)
const packages = await client.consumerPricing.v3ListPackages({ search: "MRI Brain" });

// Get negotiated prices for a package near a zip code / network
const prices = await client.consumerPricing.v3QueryPrices({
  package_id: packages.items[0].id,
  pricing: { type: "negotiated", network_id: "your-network-id" },
  location: { zip: "90210" },
});

for (const price of prices.items) {
  console.log(price.provider.name, price.total.amount);
}
```

### C\#

```csharp
// Search for shoppable service packages (SSPs)
var packages = await client.ConsumerPricing.V3ListPackagesAsync(new V3ListPackagesRequest { Search = "MRI Brain" });

// Get negotiated prices for a package near a zip code / network
var prices = await client.ConsumerPricing.V3QueryPricesAsync(new V3PricesQueryRequest
{
    PackageId = packages.Items.First().Id,
    Pricing = new V3PricesQueryRequestPricing(
        new V3PricesQueryRequestPricing.Negotiated(new V3PricingNegotiated { NetworkId = "your-network-id" })
    ),
    Location = new V3Location { Zip = "90210" },
});

foreach (var price in prices.Items)
{
    Console.WriteLine($"{price.Provider.Name} {price.Total.Amount}");
}
```

---

## Examples

Runnable, language specific, example projects live under [`examples/`](examples/):

---

## For AI coding assistants

This repository is meant to be read directly by coding agents, not just humans:

- **Copy from `examples/`, not from memory.** The auth-handler and request patterns above are duplicated there in complete, runnable files — prefer reading them over reconstructing a call from partial context.
- **`openapi.json`** at the repository root is the full machine-readable API schema (every endpoint, request/response shape, and field description) if you need more than the SDK's typed methods surface.
- **The hosted docs have an LLM-native mirror.** [turquoise.health/api/docs/llms.txt](https://turquoise.health/api/docs/llms.txt) indexes every docs section, and [llms-full.txt](https://turquoise.health/api/docs/llms-full.txt) inlines all of them as Markdown in one file — useful context to fetch before proposing an integration.
- **Don't invent endpoints or fields.** The Fern-generated code (everything outside `lib/`) is the source of truth for what the API actually accepts; if a method or field isn't in the generated client or `openapi.json`, it doesn't exist yet.

---

## Versioning

This library follows [Semantic Versioning](https://semver.org). The SDK version tracks the API version; breaking API changes increment the major version.

---

## Contributing

The clients in this repository are auto-generated from the Turquoise Health OpenAPI spec using [Fern](https://buildwithfern.com). To report an API issue or request a change, contact your Turquoise Health integration partner.
