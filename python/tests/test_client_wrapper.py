import pytest

from python.core.client_wrapper import BaseClientWrapper


def test_token_supplier_is_called_for_each_header_set():
    tokens = iter(["first-token", "second-token"])
    client = BaseClientWrapper(
        token=lambda: next(tokens),
        base_url="https://api.turquoise.health",
    )

    assert client.get_headers()["Authorization"] == "Bearer first-token"
    assert client.get_headers()["Authorization"] == "Bearer second-token"


def test_static_token_is_rejected():
    with pytest.raises(TypeError, match="callable"):
        BaseClientWrapper(token="static-token", base_url="https://api.turquoise.health")
