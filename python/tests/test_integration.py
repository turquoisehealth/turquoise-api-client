"""
Integration tests for the Turquoise Health API Python client.

These tests perform smoke testing by calling real API endpoints to validate that
the client can successfully initialize, authenticate, make requests, and parse responses.

Prerequisites:
    - Set TURQUOISE_CLIENT_ID, TURQUOISE_CLIENT_SECRET, and TURQUOISE_ORGANIZATION_ID environment variables
    - Install test dependencies: pip install -r requirements-test.txt

Run tests:
    pytest python/tests/test_integration.py -v -s
"""

import os
import sys
from pathlib import Path

import pytest

# Add the project root to sys.path to import the python package
sys.path.insert(0, str(Path(__file__).parent.parent.parent))

from python.client import TurquoiseHealth
from python.lib import APIAuthHandler

CLIENT_CREDENTIALS = (
    os.getenv("TURQUOISE_CLIENT_ID"),
    os.getenv("TURQUOISE_CLIENT_SECRET"),
    os.getenv("TURQUOISE_ORGANIZATION_ID"),
)
skip_if_no_credentials = pytest.mark.skipif(
    not all(CLIENT_CREDENTIALS),
    reason="OAuth client credentials are not set"
)

BASE_URL = "https://api.turquoise.health"


@pytest.fixture(scope="module")
def client():
    """Create a TurquoiseHealth client instance for testing."""
    if not all(CLIENT_CREDENTIALS):
        pytest.skip("OAuth client credentials are not set")

    auth = APIAuthHandler.from_client_credentials()
    return TurquoiseHealth(
        base_url=BASE_URL,
        token=auth.as_callable(),
    )


class TestV3Endpoints:
    """Test v3 API endpoints."""

    @skip_if_no_credentials
    def test_v3list_packages(self, client):
        """Test listing shoppable service packages."""
        response = client.consumer_pricing.v3list_packages(page_size=5)

        # Verify response structure
        assert response is not None
        assert hasattr(response, 'items')
        assert isinstance(response.items, list)
        assert hasattr(response, 'page')

        # Verify we got results
        assert len(response.items) > 0

        # Verify package structure
        package = response.items[0]
        assert hasattr(package, 'id')
        assert hasattr(package, 'name')

        print(f"✓ Retrieved {len(response.items)} packages")

    @skip_if_no_credentials
    def test_v3list_packages_with_search(self, client):
        """Test searching packages by keyword."""
        response = client.consumer_pricing.v3list_packages(search="MRI", page_size=5)

        assert response is not None
        assert hasattr(response, 'items')
        assert isinstance(response.items, list)

        print(f"✓ Search returned {len(response.items)} packages")

    @skip_if_no_credentials
    def test_v3list_networks(self, client):
        """Test listing insurance networks."""
        response = client.consumer_pricing.v3list_networks(page_size=5)

        # Verify response structure
        assert response is not None
        assert hasattr(response, 'items')
        assert isinstance(response.items, list)
        assert hasattr(response, 'page')

        # Verify we got results
        assert len(response.items) > 0

        # Verify network structure
        network = response.items[0]
        assert hasattr(network, 'id')

        print(f"✓ Retrieved {len(response.items)} insurance networks")

    @skip_if_no_credentials
    def test_v3list_providers(self, client):
        """Test listing healthcare providers."""
        response = client.consumer_pricing.v3list_providers(location_within_state="CA", page_size=5)

        # Verify response structure
        assert response is not None
        assert hasattr(response, 'items')
        assert isinstance(response.items, list)
        assert hasattr(response, 'page')

        # Verify we got results
        assert len(response.items) > 0

        # Verify provider structure
        provider = response.items[0]
        assert hasattr(provider, 'id')

        print(f"✓ Retrieved {len(response.items)} providers")


if __name__ == "__main__":
    # Allow running tests directly
    pytest.main([__file__, "-v", "-s"])
