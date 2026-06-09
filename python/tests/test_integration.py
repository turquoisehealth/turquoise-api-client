"""
Integration tests for Turquoise Health API Python client.

These tests validate the client can successfully communicate with the API
and call various endpoints. Set the TURQUOISE_API_TOKEN environment variable
to run these tests.
"""

import os
import sys

# Add the project root to path so we can import the python package
project_root = os.path.abspath(os.path.join(os.path.dirname(__file__), '..', '..'))
sys.path.insert(0, project_root)

import pytest
from python import TurquoiseHealth


@pytest.fixture
def client():
    """Create a Turquoise Health client for testing."""
    token = os.environ.get("TURQUOISE_API_TOKEN")
    if not token:
        pytest.skip("TURQUOISE_API_TOKEN environment variable not set")

    return TurquoiseHealth(
        token=token,
        base_url="https://api.turquoise.health"
    )


class TestConsumerPricingIntegration:
    """Integration tests for consumer pricing endpoints."""

    def test_list_ssps(self, client):
        """Test listing shoppable service packages."""
        response = client.consumer_pricing.get_ssps(page_size=5)

        assert response is not None
        assert hasattr(response, 'results')
        assert isinstance(response.results, list)
        print(f"✓ Listed {len(response.results)} SSPs")

    def test_search_ssps_by_name(self, client):
        """Test searching SSPs by name."""
        response = client.consumer_pricing.get_ssps(search="MRI", page_size=3)

        assert response is not None
        assert hasattr(response, 'results')
        print(f"✓ Found {len(response.results)} SSPs matching 'MRI'")

    def test_list_insurance_networks(self, client):
        """Test listing insurance networks."""
        response = client.consumer_pricing.get_insurance_networks(page_size=5)

        assert response is not None
        assert hasattr(response, 'results')
        assert isinstance(response.results, list)
        print(f"✓ Listed {len(response.results)} insurance networks")

    def test_list_providers(self, client):
        """Test listing healthcare providers."""
        response = client.consumer_pricing.get_providers(page_size=5)

        assert response is not None
        assert hasattr(response, 'results')
        assert isinstance(response.results, list)
        print(f"✓ Listed {len(response.results)} providers")

    def test_v2_list_ssps(self, client):
        """Test v2 endpoint for listing SSPs."""
        response = client.consumer_pricing.v2list_ssps(page_size=5)

        assert response is not None
        assert hasattr(response, 'items')
        assert isinstance(response.items, list)
        print(f"✓ V2: Listed {len(response.items)} SSPs")

    def test_v2_list_networks(self, client):
        """Test v2 endpoint for listing networks."""
        response = client.consumer_pricing.v2list_networks(page_size=5)

        assert response is not None
        assert hasattr(response, 'items')
        assert isinstance(response.items, list)
        print(f"✓ V2: Listed {len(response.items)} networks")

    def test_v2_list_providers(self, client):
        """Test v2 endpoint for listing providers."""
        response = client.consumer_pricing.v2list_providers(page_size=5)

        assert response is not None
        assert hasattr(response, 'items')
        assert isinstance(response.items, list)
        print(f"✓ V2: Listed {len(response.items)} providers")


if __name__ == "__main__":
    # Allow running tests directly with: python -m pytest tests/test_integration.py -v
    pytest.main([__file__, "-v", "-s"])
