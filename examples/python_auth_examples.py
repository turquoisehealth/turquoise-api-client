"""
Example: Using the APIAuthHandler for Authentication

This demonstrates various authentication patterns using the APIAuthHandler class,
including static tokens, dynamic token providers, and OAuth client credentials
with automatic token refresh.

Prerequisites:
    pip install httpx pydantic

For development, this example imports from the local ../python directory.
For production use, install the published package:
    pip install turquoise-health
"""

import os
import sys
from dotenv import load_dotenv
from pathlib import Path

# Add the parent directory to sys.path to import the local module
sys.path.insert(0, str(Path(__file__).parent.parent / "python"))

load_dotenv()

try:
    from client import TurquoiseHealth
    from lib import APIAuthHandler
except ImportError as e:
    print(f"Error: {e}")
    print("\nPlease install dependencies: pip install httpx pydantic")
    sys.exit(1)


def example_basic_auth_handler():
    """Example: Using APIAuthHandler with a non-expiring token from environment."""
    # Set environment variable: TURQUOISE_API_TOKEN
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


def example_oauth_client_credentials():
    """Example: Using OAuth client credentials with automatic token refresh."""
    # Set OAuth credentials in environment
    # TURQUOISE_CLIENT_ID=your-client-id
    # TURQUOISE_CLIENT_SECRET=your-client-secret
    # TURQUOISE_ORGANIZATION_ID=your-org-id

    # Create auth handler with OAuth credentials
    # Token will be automatically refreshed when it expires
    try:
        auth = APIAuthHandler.from_client_credentials()
        client = TurquoiseHealth(
            base_url="https://api.turquoise.health",
            token=auth.as_callable()
        )

        print("✓ Client initialized with OAuth auto-refresh")
        return client
    except ValueError as e:
        print(f"  (Skipped: {e})")
        return None


if __name__ == "__main__":
    print("Turquoise Health SDK - APIAuthHandler Examples\n")

    # Note: These examples won't actually connect without a real API token
    print("Example 1: Basic Auth Handler (Environment Token)")
    try:
        example_basic_auth_handler()
    except ValueError as e:
        print(f"  (Skipped: {e})")

    print("\nExample 2: Dynamic Token Provider")
    example_dynamic_token()

    print("\nExample 3: OAuth Client Credentials (Auto-Refresh)")
    example_oauth_client_credentials()

    print("\n✨ All authentication examples completed!")
