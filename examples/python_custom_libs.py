"""
Example: Using custom libraries with the Turquoise Health Python SDK

This demonstrates how to use manually-coded libraries alongside the generated SDK.
"""

import os
from turquoise_health import TurquoiseHealth, APIAuthHandler


def example_basic_auth_handler():
    """Example: Using APIAuthHandler for basic authentication."""
    # Set your token in environment variable
    os.environ["TURQUOISE_API_TOKEN"] = "your-api-token-here"

    # Use the auth handler
    auth = APIAuthHandler.from_env()
    client = TurquoiseHealth(
        base_url="https://api.turquoise.health",
        token=auth.get_token()
    )

    print("✓ Client initialized with APIAuthHandler")
    return client


def example_dynamic_token():
    """Example: Using a dynamic token provider."""

    def get_token_from_vault():
        # In production, this might fetch from a secrets manager
        return os.getenv("TURQUOISE_API_TOKEN", "default-token")

    # Create auth handler with dynamic token provider
    auth = APIAuthHandler(token_provider=get_token_from_vault)

    # Use as a callable with the client
    client = TurquoiseHealth(
        base_url="https://api.turquoise.health",
        token=auth.as_callable()
    )

    print("✓ Client initialized with dynamic token provider")
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

    # Note: These examples won't actually connect without a real API token
    print("Example 1: Basic Auth Handler")
    try:
        example_basic_auth_handler()
    except ValueError as e:
        print(f"  (Skipped: {e})")

    print("\nExample 2: Dynamic Token Provider")
    example_dynamic_token()

    print("\nExample 3: Import from lib module")
    example_import_from_lib()

    print("\n✨ All examples completed!")
