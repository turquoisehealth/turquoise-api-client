# Reference
## ConsumerPricing
<details><summary><code>client.ConsumerPricing.<a href="/src/TurquoiseHealth.Api/ConsumerPricing/ConsumerPricingClient.cs">GetSspsAsync</a>(GetSspsRequest { ... }) -> ServicePackagePage</code></summary>
<dl>
<dd>

#### 📝 Description

<dl>
<dd>

<dl>
<dd>

Discover available service/surgery packages (SSPs). Supports optional name filtering and pagination.
</dd>
</dl>
</dd>
</dl>

#### 🔌 Usage

<dl>
<dd>

<dl>
<dd>

```csharp
await client.ConsumerPricing.GetSspsAsync(new GetSspsRequest());
```
</dd>
</dl>
</dd>
</dl>

#### ⚙️ Parameters

<dl>
<dd>

<dl>
<dd>

**request:** `GetSspsRequest` 
    
</dd>
</dl>
</dd>
</dl>


</dd>
</dl>
</details>

<details><summary><code>client.ConsumerPricing.<a href="/src/TurquoiseHealth.Api/ConsumerPricing/ConsumerPricingClient.cs">GetInsuranceNetworksAsync</a>(GetInsuranceNetworksRequest { ... }) -> InsuranceNetworkPage</code></summary>
<dl>
<dd>

#### 📝 Description

<dl>
<dd>

<dl>
<dd>

Discover insurance networks, optionally filtered by SSP or payer name.
</dd>
</dl>
</dd>
</dl>

#### 🔌 Usage

<dl>
<dd>

<dl>
<dd>

```csharp
await client.ConsumerPricing.GetInsuranceNetworksAsync(new GetInsuranceNetworksRequest());
```
</dd>
</dl>
</dd>
</dl>

#### ⚙️ Parameters

<dl>
<dd>

<dl>
<dd>

**request:** `GetInsuranceNetworksRequest` 
    
</dd>
</dl>
</dd>
</dl>


</dd>
</dl>
</details>

<details><summary><code>client.ConsumerPricing.<a href="/src/TurquoiseHealth.Api/ConsumerPricing/ConsumerPricingClient.cs">GetProvidersAsync</a>(GetProvidersRequest { ... }) -> ProviderPage</code></summary>
<dl>
<dd>

#### 📝 Description

<dl>
<dd>

<dl>
<dd>

Search for providers by name, NPI, or location.
</dd>
</dl>
</dd>
</dl>

#### 🔌 Usage

<dl>
<dd>

<dl>
<dd>

```csharp
await client.ConsumerPricing.GetProvidersAsync(new GetProvidersRequest());
```
</dd>
</dl>
</dd>
</dl>

#### ⚙️ Parameters

<dl>
<dd>

<dl>
<dd>

**request:** `GetProvidersRequest` 
    
</dd>
</dl>
</dd>
</dl>


</dd>
</dl>
</details>

<details><summary><code>client.ConsumerPricing.<a href="/src/TurquoiseHealth.Api/ConsumerPricing/ConsumerPricingClient.cs">GetSspPricesAsync</a>(PackagePricesRequest { ... }) -> SspPricePage</code></summary>
<dl>
<dd>

#### 📝 Description

<dl>
<dd>

<dl>
<dd>

List of providers that can satisfy a given SSP, with the total expected price, in a given geographic area. Requests support location filtering via ZIP code, CBSA, state, or coordinates. When no Network ID is provided, cash prices are shown.
</dd>
</dl>
</dd>
</dl>

#### 🔌 Usage

<dl>
<dd>

<dl>
<dd>

```csharp
await client.ConsumerPricing.GetSspPricesAsync(
    new PackagePricesRequest
    {
        SspId = "DE000",
        Location = new LocationInput
        {
            GeoSpace = new GeoSpace { ZipCodes = new List<string>() { "80129" } },
        },
        NetworkId = "-7695283351826393948",
        SortBy = CareNavSortType.PriceAsc,
    }
);
```
</dd>
</dl>
</dd>
</dl>

#### ⚙️ Parameters

<dl>
<dd>

<dl>
<dd>

**request:** `PackagePricesRequest` 
    
</dd>
</dl>
</dd>
</dl>


</dd>
</dl>
</details>

<details><summary><code>client.ConsumerPricing.<a href="/src/TurquoiseHealth.Api/ConsumerPricing/ConsumerPricingClient.cs">GetProviderSspPricesAsync</a>(ProviderPackageBreakdownRequest { ... }) -> OneOf&lt;ProviderBreakdown, NoDataResponse&gt;</code></summary>
<dl>
<dd>

#### 📝 Description

<dl>
<dd>

<dl>
<dd>

Get SSP breakdown and fee information about a selected SSP from a single provider. When no Network ID is provided, cash prices are shown.
</dd>
</dl>
</dd>
</dl>

#### 🔌 Usage

<dl>
<dd>

<dl>
<dd>

```csharp
await client.ConsumerPricing.GetProviderSspPricesAsync(
    new ProviderPackageBreakdownRequest { SspId = "DE000", ProviderId = "provider_id" }
);
```
</dd>
</dl>
</dd>
</dl>

#### ⚙️ Parameters

<dl>
<dd>

<dl>
<dd>

**request:** `ProviderPackageBreakdownRequest` 
    
</dd>
</dl>
</dd>
</dl>


</dd>
</dl>
</details>

<details><summary><code>client.ConsumerPricing.<a href="/src/TurquoiseHealth.Api/ConsumerPricing/ConsumerPricingClient.cs">CompareSspPricesAsync</a>(PriceComparisonRequest { ... }) -> OneOf&lt;PricesComparison, NoDataResponse&gt;</code></summary>
<dl>
<dd>

#### 📝 Description

<dl>
<dd>

<dl>
<dd>

Get summary statistics (min, max, average, quartiles) for prices matching the selected filters. When no Network ID is provided, cash prices are shown.
</dd>
</dl>
</dd>
</dl>

#### 🔌 Usage

<dl>
<dd>

<dl>
<dd>

```csharp
await client.ConsumerPricing.CompareSspPricesAsync(
    new PriceComparisonRequest
    {
        SspId = "DE000",
        Location = new LocationInput { GeoSpace = new GeoSpace { State = "CO" } },
        NetworkId = "-7695283351826393948",
    }
);
```
</dd>
</dl>
</dd>
</dl>

#### ⚙️ Parameters

<dl>
<dd>

<dl>
<dd>

**request:** `PriceComparisonRequest` 
    
</dd>
</dl>
</dd>
</dl>


</dd>
</dl>
</details>

<details><summary><code>client.ConsumerPricing.<a href="/src/TurquoiseHealth.Api/ConsumerPricing/ConsumerPricingClient.cs">V2ListSspsAsync</a>(V2ListSspsRequest { ... }) -> EnvelopeSsp</code></summary>
<dl>
<dd>

#### 📝 Description

<dl>
<dd>

<dl>
<dd>

Discover available service/surgery packages (SSPs). Supports optional name filtering and pagination.
</dd>
</dl>
</dd>
</dl>

#### 🔌 Usage

<dl>
<dd>

<dl>
<dd>

```csharp
await client.ConsumerPricing.V2ListSspsAsync(new V2ListSspsRequest());
```
</dd>
</dl>
</dd>
</dl>

#### ⚙️ Parameters

<dl>
<dd>

<dl>
<dd>

**request:** `V2ListSspsRequest` 
    
</dd>
</dl>
</dd>
</dl>


</dd>
</dl>
</details>

<details><summary><code>client.ConsumerPricing.<a href="/src/TurquoiseHealth.Api/ConsumerPricing/ConsumerPricingClient.cs">V2GetSspAsync</a>(V2GetSspRequest { ... }) -> SingleResourceEnvelopeSsp</code></summary>
<dl>
<dd>

#### 📝 Description

<dl>
<dd>

<dl>
<dd>

Retrieve an SSP name and patient-ready description from the relevant SSP ID.
</dd>
</dl>
</dd>
</dl>

#### 🔌 Usage

<dl>
<dd>

<dl>
<dd>

```csharp
await client.ConsumerPricing.V2GetSspAsync(new V2GetSspRequest { SspId = "GA002" });
```
</dd>
</dl>
</dd>
</dl>

#### ⚙️ Parameters

<dl>
<dd>

<dl>
<dd>

**request:** `V2GetSspRequest` 
    
</dd>
</dl>
</dd>
</dl>


</dd>
</dl>
</details>

<details><summary><code>client.ConsumerPricing.<a href="/src/TurquoiseHealth.Api/ConsumerPricing/ConsumerPricingClient.cs">V2ListProvidersAsync</a>(V2ListProvidersRequest { ... }) -> EnvelopeProvider</code></summary>
<dl>
<dd>

#### 📝 Description

<dl>
<dd>

<dl>
<dd>

Search for providers by name, NPI, or location.
</dd>
</dl>
</dd>
</dl>

#### 🔌 Usage

<dl>
<dd>

<dl>
<dd>

```csharp
await client.ConsumerPricing.V2ListProvidersAsync(new V2ListProvidersRequest());
```
</dd>
</dl>
</dd>
</dl>

#### ⚙️ Parameters

<dl>
<dd>

<dl>
<dd>

**request:** `V2ListProvidersRequest` 
    
</dd>
</dl>
</dd>
</dl>


</dd>
</dl>
</details>

<details><summary><code>client.ConsumerPricing.<a href="/src/TurquoiseHealth.Api/ConsumerPricing/ConsumerPricingClient.cs">V2GetProviderAsync</a>(V2GetProviderRequest { ... }) -> SingleResourceEnvelopeProvider</code></summary>
<dl>
<dd>

#### 📝 Description

<dl>
<dd>

<dl>
<dd>

Return relevant information about a specific provider from the provider ID, including name, NPI, and location.
</dd>
</dl>
</dd>
</dl>

#### 🔌 Usage

<dl>
<dd>

<dl>
<dd>

```csharp
await client.ConsumerPricing.V2GetProviderAsync(new V2GetProviderRequest { ProviderId = "21929" });
```
</dd>
</dl>
</dd>
</dl>

#### ⚙️ Parameters

<dl>
<dd>

<dl>
<dd>

**request:** `V2GetProviderRequest` 
    
</dd>
</dl>
</dd>
</dl>


</dd>
</dl>
</details>

<details><summary><code>client.ConsumerPricing.<a href="/src/TurquoiseHealth.Api/ConsumerPricing/ConsumerPricingClient.cs">V2ListNetworksAsync</a>(V2ListNetworksRequest { ... }) -> EnvelopeNetwork</code></summary>
<dl>
<dd>

#### 📝 Description

<dl>
<dd>

<dl>
<dd>

Discover insurance networks, optionally filtered by network name, payer name, or payer ID.
</dd>
</dl>
</dd>
</dl>

#### 🔌 Usage

<dl>
<dd>

<dl>
<dd>

```csharp
await client.ConsumerPricing.V2ListNetworksAsync(new V2ListNetworksRequest());
```
</dd>
</dl>
</dd>
</dl>

#### ⚙️ Parameters

<dl>
<dd>

<dl>
<dd>

**request:** `V2ListNetworksRequest` 
    
</dd>
</dl>
</dd>
</dl>


</dd>
</dl>
</details>

<details><summary><code>client.ConsumerPricing.<a href="/src/TurquoiseHealth.Api/ConsumerPricing/ConsumerPricingClient.cs">V2GetNetworkAsync</a>(V2GetNetworkRequest { ... }) -> SingleResourceEnvelopeNetwork</code></summary>
<dl>
<dd>

#### 📝 Description

<dl>
<dd>

<dl>
<dd>

Return relevant information about a specific insurance network from the network ID.
</dd>
</dl>
</dd>
</dl>

#### 🔌 Usage

<dl>
<dd>

<dl>
<dd>

```csharp
await client.ConsumerPricing.V2GetNetworkAsync(
    new V2GetNetworkRequest { NetworkId = "8361580493441765265" }
);
```
</dd>
</dl>
</dd>
</dl>

#### ⚙️ Parameters

<dl>
<dd>

<dl>
<dd>

**request:** `V2GetNetworkRequest` 
    
</dd>
</dl>
</dd>
</dl>


</dd>
</dl>
</details>

<details><summary><code>client.ConsumerPricing.<a href="/src/TurquoiseHealth.Api/ConsumerPricing/ConsumerPricingClient.cs">V2ListPricesAsync</a>(PricesRequest { ... }) -> EnvelopeRate</code></summary>
<dl>
<dd>

#### 📝 Description

<dl>
<dd>

<dl>
<dd>

List of providers that can satisfy a given SSP, with the total expected price, in a given geographic area. Requests support location filtering via ZIP code, CBSA name, state, or coordinates. When no Network ID is provided, cash prices are shown.
</dd>
</dl>
</dd>
</dl>

#### 🔌 Usage

<dl>
<dd>

<dl>
<dd>

```csharp
await client.ConsumerPricing.V2ListPricesAsync(
    new PricesRequest
    {
        SspId = "GA002",
        NetworkId = "8361580493441765265",
        RateType = PricesRequestRateType.Negotiated,
        Location = new RateCompareLocation { ZipAnchor = "80202" },
    }
);
```
</dd>
</dl>
</dd>
</dl>

#### ⚙️ Parameters

<dl>
<dd>

<dl>
<dd>

**request:** `PricesRequest` 
    
</dd>
</dl>
</dd>
</dl>


</dd>
</dl>
</details>

<details><summary><code>client.ConsumerPricing.<a href="/src/TurquoiseHealth.Api/ConsumerPricing/ConsumerPricingClient.cs">V2GetPriceBreakdownAsync</a>(ProviderBreakdownRequest { ... }) -> SingleResourceEnvelopeRateBreakdown</code></summary>
<dl>
<dd>

#### 📝 Description

<dl>
<dd>

<dl>
<dd>

Get SSP breakdown and fee information about a selected SSP from a single provider. When no Network ID is provided, cash prices are shown.
</dd>
</dl>
</dd>
</dl>

#### 🔌 Usage

<dl>
<dd>

<dl>
<dd>

```csharp
await client.ConsumerPricing.V2GetPriceBreakdownAsync(
    new ProviderBreakdownRequest { ProviderId = "21929", SspId = "RA011" }
);
```
</dd>
</dl>
</dd>
</dl>

#### ⚙️ Parameters

<dl>
<dd>

<dl>
<dd>

**request:** `ProviderBreakdownRequest` 
    
</dd>
</dl>
</dd>
</dl>


</dd>
</dl>
</details>

<details><summary><code>client.ConsumerPricing.<a href="/src/TurquoiseHealth.Api/ConsumerPricing/ConsumerPricingClient.cs">V2ComparePricesAsync</a>(RateCompareRequest { ... }) -> RateComparison</code></summary>
<dl>
<dd>

#### 📝 Description

<dl>
<dd>

<dl>
<dd>

Get summary statistics (min, max, average, quartiles) for prices matching the selected filters. When no Network ID is provided, cash prices are shown.
</dd>
</dl>
</dd>
</dl>

#### 🔌 Usage

<dl>
<dd>

<dl>
<dd>

```csharp
await client.ConsumerPricing.V2ComparePricesAsync(
    new RateCompareRequest
    {
        SspId = "RA011",
        NetworkId = "8361580493441765265",
        Location = new RateCompareLocation { WithinState = "CO" },
    }
);
```
</dd>
</dl>
</dd>
</dl>

#### ⚙️ Parameters

<dl>
<dd>

<dl>
<dd>

**request:** `RateCompareRequest` 
    
</dd>
</dl>
</dd>
</dl>


</dd>
</dl>
</details>
