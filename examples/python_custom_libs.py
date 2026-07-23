"""
Example: Using custom libraries with the Turquoise Health Python SDK

This demonstrates how to use manually-coded libraries alongside the generated SDK.

Recommended Approach:
1. Set OAuth credentials as environment variables
2. Create APIAuthHandler once (e.g., at module level or as singleton)
3. Reuse the same handler across your application for automatic token refresh
"""

import os
from turquoise_health import TurquoiseHealth, APIAuthHandler


# RECOMMENDED: Create auth handler once and reuse it
# This enables in-memory token caching and automatic refresh
def example_oauth_client_credentials():
    """
    RECOMMENDED: OAuth client credentials with automatic token refresh.

    This is the recommended authentication method. The auth handler caches tokens
    in memory and automatically refreshes them before expiration.

    Important: Create the auth handler once and reuse it across your application
    to avoid unnecessary token requests.
    """
    # Set OAuth credentials in environment
    os.environ["TURQUOISE_CLIENT_ID"] = "your-client-id"
    os.environ["TURQUOISE_CLIENT_SECRET"] = "your-client-secret"
    os.environ["TURQUOISE_ORGANIZATION_ID"] = "your-org-id"

    # Create auth handler once - reuse this instance!
    auth = APIAuthHandler.from_client_credentials()

    # Use as_callable() to enable automatic token refresh
    client = TurquoiseHealth(token=auth.as_callable())

    # Example: Make API calls
    try:
        ssps = client.consumer_pricing.ssps.list(query="MRI Brain")
        print(f"✓ Found {len(ssps)} packages")

        if ssps:
            prices = client.consumer_pricing.prices.list(
                ssp_id=ssps[0].id,
                zip_code="90210"
            )
            print(f"✓ Found {len(prices)} prices")
    except Exception as e:
        print(f"  (API call skipped: {e})")

    print("✓ Client initialized with OAuth auto-refresh")
    return client, auth


def example_dynamic_token():
    """Alternative: Using a dynamic token provider for custom token sources."""

    def get_token_from_vault():
        # In production, this might fetch from a secrets manager
        return os.getenv("TURQUOISE_API_TOKEN", "default-token")

    # Create auth handler with dynamic token provider
    auth = APIAuthHandler(token_provider=get_token_from_vault)

    # Use as_callable() with the client
    client = TurquoiseHealth(token=auth.as_callable())

    print("✓ Client initialized with dynamic token provider")
    return client


def example_basic_env_token():
    """Alternative: Using a static token from environment variable (no auto-refresh)."""
    # Set your token in environment variable
    os.environ["TURQUOISE_API_TOKEN"] = "your-api-token-here"

    # Use the auth handler
    auth = APIAuthHandler.from_env()
    client = TurquoiseHealth(token=auth.get_token())

    print("✓ Client initialized with static env token")
    return client


def example_import_from_lib():
    """Example: Importing directly from lib module."""
    from turquoise_health.lib import APIAuthHandler

    auth = APIAuthHandler(token="your-static-token")
    client = TurquoiseHealth(
        base_url="https://api.turquoise.health",
        token=auth.get_token()
    )

    print("✓ Imported APIAuthHandler from lib module")
    return client


if __name__ == "__main__":
    print("Turquoise Health SDK - Custom Libraries Examples\n")
    print("Note: Set real credentials in environment variables to run API calls\n")

    print("=" * 60)
    print("RECOMMENDED: OAuth Client Credentials (Auto-Refresh)")
    print("=" * 60)
    try:
        client, auth = example_oauth_client_credentials()
        print("  → Reuse the 'auth' instance across your application!")
    except ValueError as e:
        print(f"  (Skipped: {e})")

    print("\n" + "=" * 60)
    print("Alternative: Dynamic Token Provider")
    print("=" * 60)
    example_dynamic_token()

    print("\n" + "=" * 60)
    print("Alternative: Static Token from Environment")
    print("=" * 60)
    try:
        example_basic_env_token()
    except ValueError as e:
        print(f"  (Skipped: {e})")

    print("\n" + "=" * 60)
    print("Alternative: Import from lib module")
    print("=" * 60)
    example_import_from_lib()

    print("\n✨ All examples completed!")
