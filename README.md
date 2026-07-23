# Turquoise Health API Client

[![PyPI](https://img.shields.io/pypi/v/turquoise-health)](https://pypi.org/project/turquoise-health/)
[![npm](https://img.shields.io/npm/v/@turquoise-health/api)](https://www.npmjs.com/package/@turquoise-health/api)
[![NuGet](https://img.shields.io/nuget/v/TurquoiseHealth.Api)](https://www.nuget.org/packages/TurquoiseHealth.Api)

Official client libraries for the [Turquoise Health Consumer Pricing API](https://turquoise.health/api/docs/).

---

## Installation

### Python

```bash
pip install turquoise-health
```

### TypeScript / JavaScript

```bash
npm install @turquoise-health/api
```

### C\#

```bash
dotnet add package TurquoiseHealth.Api
```

---

## Quickstart

All requests require a Bearer token. Contact [Turquoise Health](https://turquoise.health) to obtain API credentials.

### OAuth Client Credentials with Auto-Refresh

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
client = TurquoiseHealth(token=auth.as_callable())
```

**TypeScript:**

```typescript
import { TurquoiseHealthApiClient, lib } from "@turquoise-health/api";

const auth = lib.APIAuthHandler.fromClientCredentials();
const client = new TurquoiseHealthApiClient({ token: auth.asSupplier() });
```

**C#:**

```csharp
using TurquoiseHealth.Api.Lib;

var auth = APIAuthHandler.FromClientCredentials();
var client = new TurquoiseHealthApiClient(token: auth.GetToken());
```

> **⚠️ Important**: Create the `APIAuthHandler` instance **once** and reuse it across your application (e.g., as a singleton or module-level variable). Each auth handler maintains an in-memory token cache with automatic refresh. Creating new instances for every request will bypass the cache and unnecessarily refetch tokens from the OAuth server, leading to performance degradation and rate limiting errors.

---

## Using the client

Once you have initialized the client with an auth handler, you can begin calling the api endpoints.

### Python

```python
# Search for shoppable service packages
ssps = client.consumer_pricing.ssps.list(query="MRI Brain")
for ssp in ssps:
    print(ssp.name, ssp.id)

# Get prices for a service at a location
prices = client.consumer_pricing.prices.list(
    ssp_id=ssps[0].id,
    zip_code="90210",
    network_id="your-network-id",
)
for price in prices:
    print(price.provider_name, price.cash_pay_price, price.negotiated_price)
```

### TypeScript / JavaScript

```typescript
// Search for shoppable service packages
const ssps = await client.consumerPricing.ssps.list({ query: "MRI Brain" });

// Get prices for a service at a location
const prices = await client.consumerPricing.prices.list({
  sspId: ssps.items[0].id,
  zipCode: "90210",
  networkId: "your-network-id",
});
```

### C\#

```csharp
// Search for shoppable service packages
var ssps = await client.ConsumerPricing.Ssps.ListAsync(new SspsListRequest { Query = "MRI Brain" });

// Get prices
var prices = await client.ConsumerPricing.Prices.ListAsync(new PricesListRequest
{
    SspId = ssps.Items[0].Id,
    ZipCode = "90210",
    NetworkId = "your-network-id",
});
```

---

## API Reference

Full endpoint documentation: **[turquoise.health/api/docs/getting-started/](https://turquoise.health/api/docs/getting-started/)**

---

## Versioning

This library follows [Semantic Versioning](https://semver.org). The SDK version tracks the API version; breaking API changes increment the major version.

---

## Contributing

This repository is auto-generated from the Turquoise Health OpenAPI spec using [Fern](https://buildwithfern.com). To report an API issue or request a change, contact your Turquoise Health integration partner.
