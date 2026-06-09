/**
 * Integration tests for Turquoise Health API TypeScript client.
 *
 * These tests validate the client can successfully communicate with the API
 * and call various endpoints. Set the TURQUOISE_API_TOKEN environment variable
 * to run these tests.
 */

import { TurquoiseHealthApiClient } from "../Client";

describe("Consumer Pricing Integration Tests", () => {
    let client: TurquoiseHealthApiClient;

    beforeAll(() => {
        const token = process.env.TURQUOISE_API_TOKEN;
        if (!token) {
            throw new Error("TURQUOISE_API_TOKEN environment variable not set");
        }

        client = new TurquoiseHealthApiClient({
            token: token,
            environment: "https://api.turquoise.health",
        });
    });

    describe("V1 Endpoints", () => {
        it("should list SSPs", async () => {
            const response = await client.consumerPricing.getSsps({
                page_size: 5,
            });

            expect(response).toBeDefined();
            expect(response.results).toBeDefined();
            expect(Array.isArray(response.results)).toBe(true);
            console.log(`✓ Listed ${response.results.length} SSPs`);
        });

        it("should search SSPs by name", async () => {
            const response = await client.consumerPricing.getSsps({
                search: "MRI",
                page_size: 3,
            });

            expect(response).toBeDefined();
            expect(response.results).toBeDefined();
            console.log(`✓ Found ${response.results.length} SSPs matching 'MRI'`);
        });

        it("should list insurance networks", async () => {
            const response = await client.consumerPricing.getInsuranceNetworks({
                page_size: 5,
            });

            expect(response).toBeDefined();
            expect(response.results).toBeDefined();
            expect(Array.isArray(response.results)).toBe(true);
            console.log(`✓ Listed ${response.results.length} insurance networks`);
        });

        it("should list providers", async () => {
            const response = await client.consumerPricing.getProviders({
                page_size: 5,
            });

            expect(response).toBeDefined();
            expect(response.results).toBeDefined();
            expect(Array.isArray(response.results)).toBe(true);
            console.log(`✓ Listed ${response.results.length} providers`);
        });
    });

    describe("V2 Endpoints", () => {
        it("should list SSPs (v2)", async () => {
            const response = await client.consumerPricing.v2ListSsps({
                page_size: 5,
            });

            expect(response).toBeDefined();
            expect(response.items).toBeDefined();
            expect(Array.isArray(response.items)).toBe(true);
            console.log(`✓ V2: Listed ${response.items.length} SSPs`);
        });

        it("should list networks (v2)", async () => {
            const response = await client.consumerPricing.v2ListNetworks({
                page_size: 5,
            });

            expect(response).toBeDefined();
            expect(response.items).toBeDefined();
            expect(Array.isArray(response.items)).toBe(true);
            console.log(`✓ V2: Listed ${response.items.length} networks`);
        });

        it("should list providers (v2)", async () => {
            const response = await client.consumerPricing.v2ListProviders({
                page_size: 5,
            });

            expect(response).toBeDefined();
            expect(response.items).toBeDefined();
            expect(Array.isArray(response.items)).toBe(true);
            console.log(`✓ V2: Listed ${response.items.length} providers`);
        });
    });
});
