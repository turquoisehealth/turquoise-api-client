# Turquoise Health API Client

[![PyPI](https://img.shields.io/pypi/v/turquoise-health)](https://pypi.org/project/turquoise-health/)
[![npm](https://img.shields.io/npm/v/@turquoise-health/api)](https://www.npmjs.com/package/@turquoise-health/api)
[![NuGet](https://img.shields.io/nuget/v/TurquoiseHealth.Api)](https://www.nuget.org/packages/TurquoiseHealth.Api)

Official client libraries for the [Turquoise Health Consumer Pricing API](https://turquoise.health). Surface consumer-friendly healthcare service cost estimates — including cash-pay and negotiated rates — in your application.

---

## Installation

### Python

```bash
pip install turquoise-health
```

### TypeScript / JavaScript

```bash
npm install @turquoise-health/api
# or
yarn add @turquoise-health/api
```

### C\#

```bash
dotnet add package TurquoiseHealth.Api
```

---

## Quickstart

### Python

```python
from turquoise_health import TurquoiseHealth

client = TurquoiseHealth(token="YOUR_API_TOKEN")

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
import { TurquoiseHealth } from "@turquoise-health/api";

const client = new TurquoiseHealth({ token: "YOUR_API_TOKEN" });

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
using TurquoiseHealth.Api;

var client = new TurquoiseHealthClient("YOUR_API_TOKEN");

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

## Authentication

All requests require a Bearer token. Contact [Turquoise Health](https://turquoise.health) to obtain API credentials.

Pass your token at client construction — it is sent as `Authorization: Bearer <token>` on every request.

### Custom Authentication Utilities

Each SDK includes manually-coded utilities in the `lib` module to help manage authentication:

#### Python

```python
from turquoise_health import TurquoiseHealth, APIAuthHandler

# Load token from environment variable
auth = APIAuthHandler.from_env("TURQUOISE_API_TOKEN")
client = TurquoiseHealth(token=auth.get_token())

# Or use a dynamic token provider
auth = APIAuthHandler(token_provider=lambda: get_token_from_vault())
client = TurquoiseHealth(token=auth.as_callable())
```

#### TypeScript

```typescript
import { TurquoiseHealthApiClient, lib } from "@turquoise-health/api";

// Load token from environment variable
const auth = lib.APIAuthHandler.fromEnv("TURQUOISE_API_TOKEN");
const client = new TurquoiseHealthApiClient({ token: auth.asSupplier() });

// Or use a custom token provider
const auth = new lib.APIAuthHandler({
  tokenProvider: () => getTokenFromVault()
});
const client = new TurquoiseHealthApiClient({ token: auth.asSupplier() });
```

#### C#

```csharp
using TurquoiseHealth.Api;
using TurquoiseHealth.Api.Lib;

// Load token from environment variable
var auth = APIAuthHandler.FromEnv("TURQUOISE_API_TOKEN");
var client = new TurquoiseHealthApiClient(token: auth.GetToken());

// Or use a dynamic token provider
var auth = new APIAuthHandler(tokenProvider: () => GetTokenFromVault());
var client = new TurquoiseHealthApiClient(token: auth.GetToken());
```

See [examples/](examples/) for more usage patterns.

---

## API Reference

Full endpoint documentation: **[turquoise.health/docs/api](https://turquoise.health/docs/api)**

---

## Versioning

This library follows [Semantic Versioning](https://semver.org). The SDK version tracks the API version; breaking API changes increment the major version.

---

## Contributing

This repository is auto-generated from the Turquoise Health OpenAPI spec using [Fern](https://buildwithfern.com). To report an API issue or request a change, contact your Turquoise Health integration partner.
