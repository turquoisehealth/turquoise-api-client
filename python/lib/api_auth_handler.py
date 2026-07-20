"""
APIAuthHandler - Custom authentication utilities for Turquoise Health API

Manages API authentication tokens with automatic refresh and caching for expiring tokens.
"""

import json
import os
import time
from http.client import HTTPSConnection, HTTPConnection
from typing import Optional, Callable, Dict, Any
from urllib.parse import urlparse


class APIAuthHandler:
    """
    Helper class for managing API authentication tokens with automatic refresh.

    Supports multiple authentication methods:
    1. Static token (never expires)
    2. Environment variable token
    3. OAuth client credentials flow with automatic token refresh

    Example usage:
        from turquoise_health import TurquoiseHealth
        from turquoise_health.lib import APIAuthHandler

        # Static token from environment
        auth = APIAuthHandler.from_env()
        client = TurquoiseHealth(token=auth.as_callable())

        # OAuth with client credentials (auto-refreshes when expired)
        auth = APIAuthHandler.from_client_credentials()
        client = TurquoiseHealth(token=auth.as_callable())

        # Custom token provider
        auth = APIAuthHandler(token_provider=lambda: get_token_from_vault())
        client = TurquoiseHealth(token=auth.as_callable())
    """

    def __init__(
        self,
        token: Optional[str] = None,
        token_provider: Optional[Callable[[], str]] = None,
        client_id: Optional[str] = None,
        client_secret: Optional[str] = None,
        organization_id: Optional[str] = None,
        auth_url: Optional[str] = None,
        ttl_buffer: int = 60,
    ):
        """
        Initialize the auth handler.

        Args:
            token: Static API token (takes precedence if provided)
            token_provider: Function that returns a token (for dynamic tokens)
            client_id: OAuth client ID for client credentials flow
            client_secret: OAuth client secret
            organization_id: Organization ID for OAuth
            auth_url: Authentication server URL (defaults to https://api.turquoise.health)
            ttl_buffer: Seconds before expiration to refresh token (default: 60)
        """
        self._token = token
        self._token_provider = token_provider
        self._client_id = client_id
        self._client_secret = client_secret
        self._organization_id = organization_id
        self._auth_url = auth_url or "https://api.turquoise.health"
        self._ttl_buffer = ttl_buffer
        
        # In-memory token cache
        self._cached_token: Optional[str] = None
        self._token_expiration: Optional[float] = None

    @classmethod
    def from_env(cls, env_var: str = "TURQUOISE_API_TOKEN") -> "APIAuthHandler":
        """
        Create an auth handler using a static token from environment variables.

        Args:
            env_var: Name of the environment variable containing the token

        Returns:
            APIAuthHandler instance

        Raises:
            ValueError: If the environment variable is not set
        """
        token = os.getenv(env_var)
        if not token:
            raise ValueError(f"Environment variable '{env_var}' is not set")
        return cls(token=token)

    @classmethod
    def from_client_credentials(
        cls,
        client_id: Optional[str] = None,
        client_secret: Optional[str] = None,
        organization_id: Optional[str] = None,
        auth_url: Optional[str] = None,
        ttl_buffer: int = 60,
    ) -> "APIAuthHandler":
        """
        Create an auth handler using OAuth client credentials flow.

        Credentials are read from parameters or environment variables:
        - TURQUOISE_CLIENT_ID
        - TURQUOISE_CLIENT_SECRET
        - TURQUOISE_ORGANIZATION_ID
        - TURQUOISE_AUTH_URL (optional, defaults to https://api.turquoise.health)

        Args:
            client_id: OAuth client ID (or from TURQUOISE_CLIENT_ID)
            client_secret: OAuth client secret (or from TURQUOISE_CLIENT_SECRET)
            organization_id: Organization ID (or from TURQUOISE_ORGANIZATION_ID)
            auth_url: Auth server URL (or from TURQUOISE_AUTH_URL)
            ttl_buffer: Seconds before expiration to refresh token

        Returns:
            APIAuthHandler instance

        Raises:
            ValueError: If required credentials are not provided
        """
        client_id = client_id or os.getenv("TURQUOISE_CLIENT_ID")
        client_secret = client_secret or os.getenv("TURQUOISE_CLIENT_SECRET")
        organization_id = organization_id or os.getenv("TURQUOISE_ORGANIZATION_ID")
        auth_url = auth_url or os.getenv("TURQUOISE_AUTH_URL")

        if not client_id or not client_secret or not organization_id:
            raise ValueError(
                "Client credentials required. Provide client_id, client_secret, and organization_id "
                "or set TURQUOISE_CLIENT_ID, TURQUOISE_CLIENT_SECRET, and TURQUOISE_ORGANIZATION_ID "
                "environment variables."
            )

        return cls(
            client_id=client_id,
            client_secret=client_secret,
            organization_id=organization_id,
            auth_url=auth_url,
            ttl_buffer=ttl_buffer,
        )

    def get_token(self) -> str:
        """
        Get the current authentication token.

        Resolution order:
        1. Static token if provided
        2. Custom token provider if provided
        3. Cached OAuth token if valid
        4. New OAuth token via client credentials

        Returns:
            The API token string

        Raises:
            ValueError: If no token source is configured
        """
        # Static token takes precedence
        if self._token:
            return self._token

        # Custom provider
        if self._token_provider:
            return self._token_provider()

        # OAuth client credentials with caching
        if self._client_id and self._client_secret and self._organization_id:
            return self._get_oauth_token()

        raise ValueError("No token source configured")

    def _get_oauth_token(self) -> str:
        """
        Get OAuth token using client credentials, with caching.

        Returns:
            Access token

        Raises:
            RuntimeError: If token request fails
        """
        # Check cache first
        if self._cached_token and self._token_expiration:
            if time.time() < self._token_expiration - self._ttl_buffer:
                return self._cached_token

        # Request new token
        parsed_url = urlparse(self._auth_url)
        host = parsed_url.hostname
        port = parsed_url.port

        if not host:
            raise ValueError(f"Invalid auth URL: {self._auth_url}")

        body = {
            "client_id": self._client_id,
            "client_secret": self._client_secret,
            "organization_id": self._organization_id,
            "audience": "https://api.turquoise.health",
            "grant_type": "client_credentials",
        }

        headers = {
            "Content-Type": "application/json",
            "Accept": "application/json",
        }

        # Use HTTPS by default
        if parsed_url.scheme == "http":
            conn = HTTPConnection(host, port or 80)
        else:
            conn = HTTPSConnection(host, port or 443)

        try:
            request_time = time.time()
            conn.request("POST", "/oauth/token", json.dumps(body), headers)
            response = conn.getresponse()

            if response.status != 200:
                response_body = response.read().decode("utf-8")
                raise RuntimeError(
                    f"Failed to retrieve access token: {response.status} {response.reason}. "
                    f"Response: {response_body}"
                )

            response_data = json.loads(response.read().decode("utf-8"))
            access_token = response_data.get("access_token")

            if not access_token:
                raise RuntimeError("No access_token in OAuth response")

            # Cache the token
            expires_in = response_data.get("expires_in", 3600)
            self._cached_token = access_token
            self._token_expiration = request_time + expires_in

            return access_token

        finally:
            conn.close()

    def as_callable(self) -> Callable[[], str]:
        """
        Return a callable that can be passed to the TurquoiseHealth client.

        The returned function will automatically handle token refresh when using
        OAuth client credentials.

        Returns:
            Callable that returns the current token
        """
        return self.get_token
