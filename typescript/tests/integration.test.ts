/**
 * Integration tests for the Turquoise Health API TypeScript client.
 *
 * These tests perform smoke testing by calling real API endpoints to validate that
 * the client can successfully initialize, authenticate, make requests, and parse responses.
 *
 * Prerequisites:
 *   - Set TURQUOISE_API_TOKEN environment variable
 *   - Install test dependencies: npm install
 *
 * Run tests:
 *   npm test
 */

import { TurquoisehealthApiClient } from "../index";

// Check if API token is available
const API_TOKEN = process.env.TURQUOISE_API_TOKEN;
const BASE_URL = "https://api.turquoise.health";

// Helper to skip tests if no token
const describeIfToken = API_TOKEN ? describe : describe.skip;

describe("Turquoise Health API TypeScript Client", () => {
  let client: TurquoisehealthApiClient;

  beforeAll(() => {
    if (!API_TOKEN) {
      console.warn("⚠ TURQUOISE_API_TOKEN not set - tests will be skipped");
      return;
    }

    client = new TurquoisehealthApiClient({
      environment: BASE_URL,
      token: API_TOKEN
    });
  });

  describeIfToken("V2 Endpoints", () => {
    test("v2ListSsps - list shoppable service packages", async () => {
      const response = await client.consumerPricing.v2ListSsps({
        page_size: 5
      });

      // Verify response structure
      expect(response).toBeDefined();
      expect(response.items).toBeInstanceOf(Array);
      expect(response.page).toBeDefined();

      // Verify we got results
      expect(response.items.length).toBeGreaterThan(0);

      // Verify SSP structure
      const ssp = response.items[0];
      expect(ssp.id).toBeDefined();
      expect(ssp.name).toBeDefined();

      console.log(`✓ Retrieved ${response.items.length} SSPs`);
    }, 30000);

    test("v2ListSsps - search by keyword", async () => {
      const response = await client.consumerPricing.v2ListSsps({
        search: "MRI",
        page_size: 5
      });

      expect(response).toBeDefined();
      expect(response.items).toBeInstanceOf(Array);

      console.log(`✓ Search returned ${response.items.length} SSPs`);
    }, 30000);

    test("v2ListNetworks - list insurance networks", async () => {
      const response = await client.consumerPricing.v2ListNetworks({
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

    test("v2ListProviders - list healthcare providers", async () => {
      const response = await client.consumerPricing.v2ListProviders({
        "within.state": "CA",
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
