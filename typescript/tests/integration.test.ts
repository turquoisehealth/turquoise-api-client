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

import { TurquoiseHealthApiClient } from "../../Client";

// Check if API token is available
const API_TOKEN = process.env.TURQUOISE_API_TOKEN;
const BASE_URL = "https://api.turquoise.health";

// Helper to skip tests if no token
const describeIfToken = API_TOKEN ? describe : describe.skip;

describe("Turquoise Health API TypeScript Client", () => {
  let client: TurquoiseHealthApiClient;

  beforeAll(() => {
    if (!API_TOKEN) {
      console.warn("⚠ TURQUOISE_API_TOKEN not set - tests will be skipped");
      return;
    }

    client = new TurquoiseHealthApiClient({
      environment: BASE_URL,
      token: API_TOKEN
    });
  });

  describeIfToken("V1 Endpoints", () => {
    test("getSSPs - list shoppable service packages", async () => {
      const response = await client.consumerPricing.getSSPs({
        pageSize: 5
      });

      // Verify response structure
      expect(response).toBeDefined();
      expect(response.data).toBeInstanceOf(Array);
      expect(response.meta).toBeDefined();

      // Verify we got results
      expect(response.data.length).toBeGreaterThan(0);

      // Verify SSP structure
      const ssp = response.data[0];
      expect(ssp.id).toBeDefined();
      expect(ssp.name).toBeDefined();

      console.log(`✓ Retrieved ${response.data.length} SSPs`);
    }, 30000);

    test("getSSPs - search by keyword", async () => {
      const response = await client.consumerPricing.getSSPs({
        search: "MRI",
        pageSize: 5
      });

      expect(response).toBeDefined();
      expect(response.data).toBeInstanceOf(Array);

      console.log(`✓ Search returned ${response.data.length} SSPs`);
    }, 30000);

    test("getInsuranceNetworks - list insurance networks", async () => {
      const response = await client.consumerPricing.getInsuranceNetworks({
        pageSize: 5
      });

      // Verify response structure
      expect(response).toBeDefined();
      expect(response.data).toBeInstanceOf(Array);
      expect(response.meta).toBeDefined();

      // Verify we got results
      expect(response.data.length).toBeGreaterThan(0);

      // Verify network structure
      const network = response.data[0];
      expect(network.id).toBeDefined();

      console.log(`✓ Retrieved ${response.data.length} insurance networks`);
    }, 30000);

    test("getProviders - list healthcare providers", async () => {
      const response = await client.consumerPricing.getProviders({
        state: "CA",
        pageSize: 5
      });

      // Verify response structure
      expect(response).toBeDefined();
      expect(response.data).toBeInstanceOf(Array);
      expect(response.meta).toBeDefined();

      // Verify we got results
      expect(response.data.length).toBeGreaterThan(0);

      // Verify provider structure
      const provider = response.data[0];
      expect(provider.id).toBeDefined();

      console.log(`✓ Retrieved ${response.data.length} providers`);
    }, 30000);
  });

  describeIfToken("V2 Endpoints", () => {
    test("v2ListSsps - list SSPs (v2)", async () => {
      const response = await client.consumerPricing.v2ListSsps({
        pageSize: 5
      });

      // Verify response structure
      expect(response).toBeDefined();
      expect(response.data).toBeInstanceOf(Array);

      // Verify we got results
      expect(response.data.length).toBeGreaterThan(0);

      // Verify SSP structure
      const ssp = response.data[0];
      expect(ssp.id).toBeDefined();
      expect(ssp.name).toBeDefined();

      console.log(`✓ V2: Retrieved ${response.data.length} SSPs`);
    }, 30000);

    test("v2ListNetworks - list networks (v2)", async () => {
      const response = await client.consumerPricing.v2ListNetworks({
        pageSize: 5
      });

      // Verify response structure
      expect(response).toBeDefined();
      expect(response.data).toBeInstanceOf(Array);

      // Verify we got results
      expect(response.data.length).toBeGreaterThan(0);

      // Verify network structure
      const network = response.data[0];
      expect(network.id).toBeDefined();

      console.log(`✓ V2: Retrieved ${response.data.length} networks`);
    }, 30000);

    test("v2ListProviders - list providers (v2)", async () => {
      const response = await client.consumerPricing.v2ListProviders({
        state: "CA",
        pageSize: 5
      });

      // Verify response structure
      expect(response).toBeDefined();
      expect(response.data).toBeInstanceOf(Array);

      // Verify we got results
      expect(response.data.length).toBeGreaterThan(0);

      // Verify provider structure
      const provider = response.data[0];
      expect(provider.id).toBeDefined();

      console.log(`✓ V2: Retrieved ${response.data.length} providers`);
    }, 30000);
  });
});
