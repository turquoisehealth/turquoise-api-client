/**
 * Integration tests for the Turquoise Health API TypeScript client.
 *
 * These tests perform smoke testing by calling real API endpoints to validate that
 * the client can successfully initialize, authenticate, make requests, and parse responses.
 *
 * Prerequisites:
 *   - Set TURQUOISE_CLIENT_ID, TURQUOISE_CLIENT_SECRET, and TURQUOISE_ORGANIZATION_ID environment variables
 *   - Install test dependencies: npm install
 *
 * Run tests:
 *   npm test
 */

import { TurquoiseHealthApiClient, lib } from "../index";

const HAS_CLIENT_CREDENTIALS = [
  process.env.TURQUOISE_CLIENT_ID,
  process.env.TURQUOISE_CLIENT_SECRET,
  process.env.TURQUOISE_ORGANIZATION_ID,
].every(Boolean);
const BASE_URL = "https://api.turquoise.health";

const describeIfCredentials = HAS_CLIENT_CREDENTIALS ? describe : describe.skip;

describe("Turquoise Health API TypeScript Client", () => {
  let client: TurquoiseHealthApiClient;

  beforeAll(() => {
    if (!HAS_CLIENT_CREDENTIALS) {
      console.warn("⚠ OAuth client credentials not set - tests will be skipped");
      return;
    }

    const auth = lib.APIAuthHandler.fromClientCredentials();
    client = new TurquoiseHealthApiClient({
      environment: BASE_URL,
      token: auth.asSupplier()
    });
  });

  describeIfCredentials("V3 Endpoints", () => {
    test("v3ListPackages - list shoppable service packages", async () => {
      const response = await client.consumerPricing.v3ListPackages({
        page_size: 5
      });

      // Verify response structure
      expect(response).toBeDefined();
      expect(response.items).toBeInstanceOf(Array);
      expect(response.page).toBeDefined();

      // Verify we got results
      expect(response.items.length).toBeGreaterThan(0);

      // Verify package structure
      const pkg = response.items[0];
      expect(pkg.id).toBeDefined();
      expect(pkg.name).toBeDefined();

      console.log(`✓ Retrieved ${response.items.length} packages`);
    }, 30000);

    test("v3ListPackages - search by keyword", async () => {
      const response = await client.consumerPricing.v3ListPackages({
        search: "MRI",
        page_size: 5
      });

      expect(response).toBeDefined();
      expect(response.items).toBeInstanceOf(Array);

      console.log(`✓ Search returned ${response.items.length} packages`);
    }, 30000);

    test("v3ListNetworks - list insurance networks", async () => {
      const response = await client.consumerPricing.v3ListNetworks({
        page_size: 5
      });

      // Verify response structure
      expect(response).toBeDefined();
      expect(response.items).toBeInstanceOf(Array);
      expect(response.page).toBeDefined();

      // Verify we got results
      expect(response.items.length).toBeGreaterThan(0);

      // Verify network structure
      const network = response.items[0];
      expect(network.id).toBeDefined();

      console.log(`✓ Retrieved ${response.items.length} insurance networks`);
    }, 30000);

    test("v3ListProviders - list healthcare providers", async () => {
      const response = await client.consumerPricing.v3ListProviders({
        "location.within.state": "CA",
        page_size: 5
      });

      // Verify response structure
      expect(response).toBeDefined();
      expect(response.items).toBeInstanceOf(Array);
      expect(response.page).toBeDefined();

      // Verify we got results
      expect(response.items.length).toBeGreaterThan(0);

      // Verify provider structure
      const provider = response.items[0];
      expect(provider.id).toBeDefined();

      console.log(`✓ Retrieved ${response.items.length} providers`);
    }, 30000);
  });
});
