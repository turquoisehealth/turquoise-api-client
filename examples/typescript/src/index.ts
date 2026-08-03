import { TurquoiseHealthApiClient, lib } from "@turquoisehealth/api";

// Create the auth handler once and reuse it — it caches the OAuth token in
// memory and refreshes it automatically. Building a new one per request
// bypasses that cache and can trip rate limits.
const auth = lib.APIAuthHandler.fromClientCredentials();

const client = new TurquoiseHealthApiClient({
  environment: "https://api.turquoise.health",
  token: auth.asSupplier(),
});

async function main() {
  // Search for shoppable service packages by name.
  const packages = await client.consumerPricing.v3ListPackages({
    search: "MRI Brain",
  });

  const ssp = packages.items[0];
  if (!ssp) {
    console.log("No matching service packages found.");
    return;
  }
  console.log(`Found package: ${ssp.name} (${ssp.id})`);

  // Get negotiated prices for that package near a given zip code / network.
  const prices = await client.consumerPricing.v3QueryPrices({
    package_id: ssp.id,
    pricing: {
      type: "negotiated",
      network_id: "-3776001016975145508",
    },
    location: {
      zip: "90210",
    },
  });

  for (const price of prices.items) {
    console.log(price.provider.name, price.total.amount);
  }
}

main().catch((err) => {
  console.error(err);
  process.exitCode = 1;
});
