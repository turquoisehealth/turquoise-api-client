"""
APIAuthHandler - Custom authentication utilities for Turquoise Health API

This is an example of a manually-coded library that extends the generated SDK.
"""

import os
from typing import Optional, Callable


class APIAuthHandler:
    """
    Helper class for managing API authentication tokens.

    Example usage:
        from turquoise_health import TurquoiseHealth
        from turquoise_health.lib import APIAuthHandler

        # Get token from environment
        auth = APIAuthHandler.from_env()
        client = TurquoiseHealth(token=auth.get_token())

        # Or use a custom token provider
        auth = APIAuthHandler(token_provider=lambda: get_token_from_vault())
        client = TurquoiseHealth(token=auth.get_token())
    """

    def __init__(
        self,
        token: Optional[str] = None,
        token_provider: Optional[Callable[[], str]] = None,
    ):
        """
        Initialize the auth handler.

        Args:
            token: Static API token
            token_provider: Function that returns a token (for dynamic tokens)
        """
        self._token = token
        self._token_provider = token_provider

    @classmethod
    def from_env(cls, env_var: str = "TURQUOISE_API_TOKEN") -> "APIAuthHandler":
        """
        Create an auth handler using a token from environment variables.

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

    def get_token(self) -> str:
        """
        Get the current authentication token.

        Returns:
            The API token string

        Raises:
            ValueError: If no token is available
        """
        if self._token_provider:
            return self._token_provider()
        if self._token:
            return self._token
        raise ValueError("No token or token provider configured")

    def as_callable(self) -> Callable[[], str]:
        """
        Return a callable that can be passed to the TurquoiseHealth client.

        Returns:
            Callable that returns the token
        """
        return self.get_token
