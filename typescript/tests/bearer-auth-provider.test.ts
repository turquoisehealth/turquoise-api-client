import { BearerAuthProvider } from "../auth/BearerAuthProvider";

describe("BearerAuthProvider", () => {
    it("resolves a fresh token for each request", async () => {
        const tokens = ["first-token", "second-token"];
        const provider = new BearerAuthProvider({
            token: () => tokens.shift(),
        });

        await expect(provider.getAuthRequest()).resolves.toEqual({
            headers: { Authorization: "Bearer first-token" },
        });
        await expect(provider.getAuthRequest()).resolves.toEqual({
            headers: { Authorization: "Bearer second-token" },
        });
    });

    it("rejects a static token at runtime", () => {
        const options = { token: "static-token" } as unknown as BearerAuthProvider.Options;

        expect(() => new BearerAuthProvider(options)).toThrow(
            "token must be a token supplier function",
        );
    });
});
