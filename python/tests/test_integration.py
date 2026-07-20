"""
Integration tests for the Turquoise Health API Python client.

These tests perform smoke testing by calling real API endpoints to validate that
the client can successfully initialize, authenticate, make requests, and parse responses.

Prerequisites:
    - Set TURQUOISE_API_TOKEN environment variable
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

# Check if API token is available
API_TOKEN = os.getenv("TURQUOISE_API_TOKEN")
skip_if_no_token = pytest.mark.skipif(
    not API_TOKEN,
    reason="TURQUOISE_API_TOKEN environment variable not set"
)

BASE_URL = "https://api.turquoise.health"


@pytest.fixture(scope="module")
def client():
    """Create a TurquoiseHealth client instance for testing."""
    if not API_TOKEN:
        pytest.skip("TURQUOISE_API_TOKEN not set")

    return TurquoiseHealth(
        base_url=BASE_URL,
        token=API_TOKEN
    )


class TestV1Endpoints:
    """Test v1 API endpoints."""

    @skip_if_no_token
    def test_get_ssps(self, client):
        """Test listing shoppable service packages."""
        response = client.consumer_pricing.get_ssps(page_size=5)

        # Verify response structure
        assert response is not None
        assert hasattr(response, 'data')
        assert isinstance(response.data, list)
        assert hasattr(response, 'meta')

        # Verify we got results
        assert len(response.data) > 0

        # Verify SSP structure
        ssp = response.data[0]
        assert hasattr(ssp, 'id')
        assert hasattr(ssp, 'name')

        print(f"✓ Retrieved {len(response.data)} SSPs")

    @skip_if_no_token
    def test_get_ssps_with_search(self, client):
        """Test searching SSPs by keyword."""
        response = client.consumer_pricing.get_ssps(search="MRI", page_size=5)

        assert response is not None
        assert hasattr(response, 'data')
        assert isinstance(response.data, list)

        print(f"✓ Search returned {len(response.data)} SSPs")

    @skip_if_no_token
    def test_get_insurance_networks(self, client):
        """Test listing insurance networks."""
        response = client.consumer_pricing.get_insurance_networks(page_size=5)

        # Verify response structure
        assert response is not None
        assert hasattr(response, 'data')
        assert isinstance(response.data, list)
        assert hasattr(response, 'meta')

        # Verify we got results
        assert len(response.data) > 0

        # Verify network structure
        network = response.data[0]
        assert hasattr(network, 'id')

        print(f"✓ Retrieved {len(response.data)} insurance networks")

    @skip_if_no_token
    def test_get_providers(self, client):
        """Test listing healthcare providers."""
        response = client.consumer_pricing.get_providers(state="CA", page_size=5)

        # Verify response structure
        assert response is not None
        assert hasattr(response, 'data')
        assert isinstance(response.data, list)
        assert hasattr(response, 'meta')

        # Verify we got results
        assert len(response.data) > 0

        # Verify provider structure
        provider = response.data[0]
        assert hasattr(provider, 'id')

        print(f"✓ Retrieved {len(response.data)} providers")


class TestV2Endpoints:
    """Test v2 API endpoints."""

    @skip_if_no_token
    def test_v2list_ssps(self, client):
        """Test v2 endpoint for listing SSPs."""
        response = client.consumer_pricing.v2list_ssps(page_size=5)

        # Verify response structure
        assert response is not None
        assert hasattr(response, 'data')
        assert isinstance(response.data, list)

        # Verify we got results
        assert len(response.data) > 0

        # Verify SSP structure
        ssp = response.data[0]
        assert hasattr(ssp, 'id')
        assert hasattr(ssp, 'name')

        print(f"✓ V2: Retrieved {len(response.data)} SSPs")

    @skip_if_no_token
    def test_v2list_networks(self, client):
        """Test v2 endpoint for listing networks."""
        response = client.consumer_pricing.v2list_networks(page_size=5)

        # Verify response structure
        assert response is not None
        assert hasattr(response, 'data')
        assert isinstance(response.data, list)

        # Verify we got results
        assert len(response.data) > 0

        # Verify network structure
        network = response.data[0]
        assert hasattr(network, 'id')

        print(f"✓ V2: Retrieved {len(response.data)} networks")

    @skip_if_no_token
    def test_v2list_providers(self, client):
        """Test v2 endpoint for listing providers."""
        response = client.consumer_pricing.v2list_providers(state="CA", page_size=5)

        # Verify response structure
        assert response is not None
        assert hasattr(response, 'data')
        assert isinstance(response.data, list)

        # Verify we got results
        assert len(response.data) > 0

        # Verify provider structure
        provider = response.data[0]
        assert hasattr(provider, 'id')

        print(f"✓ V2: Retrieved {len(response.data)} providers")


if __name__ == "__main__":
    # Allow running tests directly
    pytest.main([__file__, "-v", "-s"])
