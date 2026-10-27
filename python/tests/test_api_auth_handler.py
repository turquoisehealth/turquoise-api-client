import json
from unittest.mock import Mock

import pytest

from python.lib import APIAuthHandler
from python.lib import api_auth_handler as auth_module


CREDENTIALS = {
    "client_id": "test-client",
    "client_secret": "test-secret",
    "organization_id": "test-org",
}


@pytest.fixture(autouse=True)
def isolated_auth(monkeypatch):
    for name in (
        "TURQUOISE_API_TOKEN",
        "TURQUOISE_CLIENT_ID",
        "TURQUOISE_CLIENT_SECRET",
        "TURQUOISE_ORGANIZATION_ID",
        "TURQUOISE_AUTH_URL",
        "TEST_API_TOKEN",
    ):
        monkeypatch.delenv(name, raising=False)

    connection = Mock()
    connection.getresponse.return_value.status = 200
    connection.getresponse.return_value.read.return_value = json.dumps(
        {"access_token": "oauth-token", "expires_in": 100}
    ).encode()
    https = Mock(return_value=connection)
    http = Mock(return_value=connection)
    clock = Mock(return_value=1000)
    monkeypatch.setattr(auth_module, "HTTPSConnection", https)
    monkeypatch.setattr(auth_module, "HTTPConnection", http)
    monkeypatch.setattr(auth_module.time, "time", clock)
    return connection, https, http, clock


def test_static_token_takes_precedence(isolated_auth):
    connection, _, _, _ = isolated_auth
    provider = Mock(return_value="provider-token")
    auth = APIAuthHandler(token="static-token", token_provider=provider, **CREDENTIALS)

    assert auth.get_token() == "static-token"
    assert auth.as_callable()() == "static-token"
    provider.assert_not_called()
    connection.request.assert_not_called()


def test_provider_is_called_for_each_token_and_takes_precedence(isolated_auth):
    connection, _, _, _ = isolated_auth
    provider = Mock(side_effect=["first", "second"])
    auth = APIAuthHandler(token_provider=provider, **CREDENTIALS)

    supplier = auth.as_callable()
    assert supplier() == "first"
    assert supplier() == "second"
    assert provider.call_count == 2
    connection.request.assert_not_called()


def test_provider_errors_propagate():
    auth = APIAuthHandler(token_provider=Mock(side_effect=RuntimeError("vault unavailable")))
    with pytest.raises(RuntimeError, match="vault unavailable"):
        auth.get_token()


@pytest.mark.parametrize("env_var", ["TURQUOISE_API_TOKEN", "TEST_API_TOKEN"])
def test_token_from_environment(monkeypatch, env_var):
    monkeypatch.setenv(env_var, "environment-token")
    auth = APIAuthHandler.from_env() if env_var == "TURQUOISE_API_TOKEN" else APIAuthHandler.from_env(env_var)
    assert auth.get_token() == "environment-token"


@pytest.mark.parametrize("value", [None, ""])
def test_missing_environment_token_fails(monkeypatch, value):
    if value is not None:
        monkeypatch.setenv("TURQUOISE_API_TOKEN", value)
    with pytest.raises(ValueError, match="TURQUOISE_API_TOKEN.*not set"):
        APIAuthHandler.from_env()


@pytest.mark.parametrize("missing", list(CREDENTIALS))
def test_each_missing_credential_fails(missing):
    credentials = {**CREDENTIALS, missing: None}
    with pytest.raises(ValueError, match="Client credentials required"):
        APIAuthHandler.from_client_credentials(**credentials)


@pytest.mark.parametrize("credentials", [{}, {"client_id": "only-client"}])
def test_no_complete_token_source_fails(credentials):
    with pytest.raises(ValueError, match="No token source configured"):
        APIAuthHandler(**credentials).get_token()


def test_credentials_and_auth_url_from_environment(monkeypatch, isolated_auth):
    connection, _, http, _ = isolated_auth
    for key, value in CREDENTIALS.items():
        monkeypatch.setenv(f"TURQUOISE_{key.upper()}", value)
    monkeypatch.setenv("TURQUOISE_AUTH_URL", "http://auth.example:8080")

    assert APIAuthHandler.from_client_credentials().get_token() == "oauth-token"
    http.assert_called_once_with("auth.example", 8080)
    body = json.loads(connection.request.call_args.args[2])
    assert {key: body[key] for key in CREDENTIALS} == CREDENTIALS


def test_explicit_credentials_override_environment(monkeypatch, isolated_auth):
    connection, https, _, _ = isolated_auth
    for key in CREDENTIALS:
        monkeypatch.setenv(f"TURQUOISE_{key.upper()}", "environment-value")
    monkeypatch.setenv("TURQUOISE_AUTH_URL", "http://ignored.example")

    auth = APIAuthHandler.from_client_credentials(
        **CREDENTIALS, auth_url="https://auth.example:8443"
    )
    assert auth.get_token() == "oauth-token"
    https.assert_called_once_with("auth.example", 8443)
    body = json.loads(connection.request.call_args.args[2])
    assert body == {
        **CREDENTIALS,
        "audience": "https://api.turquoise.health",
        "grant_type": "client_credentials",
    }


@pytest.mark.parametrize(
    "auth_url, transport, host, port",
    [
        (None, "https", "api.turquoise.health", 443),
        ("https://auth.example", "https", "auth.example", 443),
        ("http://auth.example", "http", "auth.example", 80),
    ],
)
def test_oauth_transport_and_request(isolated_auth, auth_url, transport, host, port):
    connection, https, http, _ = isolated_auth
    auth = APIAuthHandler.from_client_credentials(**CREDENTIALS, auth_url=auth_url)
    assert auth.get_token() == "oauth-token"

    (https if transport == "https" else http).assert_called_once_with(host, port)
    method, path, body, headers = connection.request.call_args.args
    assert (method, path) == ("POST", "/oauth/token")
    assert json.loads(body)["grant_type"] == "client_credentials"
    assert headers == {"Content-Type": "application/json", "Accept": "application/json"}
    connection.close.assert_called_once()


def test_oauth_cache_refreshes_at_buffer_boundary(isolated_auth):
    connection, _, _, clock = isolated_auth
    auth = APIAuthHandler.from_client_credentials(**CREDENTIALS)
    supplier = auth.as_callable()
    assert supplier() == "oauth-token"

    clock.return_value = 1039
    assert supplier() == "oauth-token"
    assert connection.request.call_count == 1

    clock.return_value = 1040
    connection.getresponse.return_value.read.return_value = b'{"access_token":"refreshed"}'
    assert supplier() == "refreshed"
    assert connection.request.call_count == 2


def test_zero_buffer_keeps_token_until_actual_expiration(isolated_auth):
    connection, _, _, clock = isolated_auth
    auth = APIAuthHandler.from_client_credentials(**CREDENTIALS, ttl_buffer=0)
    assert auth.get_token() == "oauth-token"
    clock.return_value = 1099
    assert auth.get_token() == "oauth-token"
    assert connection.request.call_count == 1
    clock.return_value = 1100
    auth.get_token()
    assert connection.request.call_count == 2


def test_missing_expiry_defaults_to_one_hour(isolated_auth):
    connection, _, _, clock = isolated_auth
    connection.getresponse.return_value.read.return_value = b'{"access_token":"token"}'
    auth = APIAuthHandler.from_client_credentials(**CREDENTIALS)
    assert auth.get_token() == "token"
    clock.return_value = 4539
    assert auth.get_token() == "token"
    assert connection.request.call_count == 1
    clock.return_value = 4540
    auth.get_token()
    assert connection.request.call_count == 2


def test_zero_expiry_is_not_cached(isolated_auth):
    connection, _, _, _ = isolated_auth
    connection.getresponse.return_value.read.return_value = b'{"access_token":"token","expires_in":0}'
    auth = APIAuthHandler.from_client_credentials(**CREDENTIALS)
    auth.get_token()
    auth.get_token()
    assert connection.request.call_count == 2


def test_expiration_is_measured_before_network_delay(isolated_auth):
    connection, _, _, clock = isolated_auth

    def delayed_response():
        clock.return_value = 1050
        response = Mock(status=200)
        response.read.return_value = b'{"access_token":"token","expires_in":100}'
        return response

    connection.getresponse.side_effect = delayed_response
    auth = APIAuthHandler.from_client_credentials(**CREDENTIALS)
    auth.get_token()
    auth.get_token()
    assert connection.request.call_count == 2


def test_invalid_auth_url_fails_before_network(isolated_auth):
    connection, _, _, _ = isolated_auth
    auth = APIAuthHandler.from_client_credentials(**CREDENTIALS, auth_url="not-a-url")
    with pytest.raises(ValueError, match="Invalid auth URL"):
        auth.get_token()
    connection.request.assert_not_called()


def test_http_error_closes_connection_and_does_not_cache_failure(isolated_auth):
    connection, _, _, _ = isolated_auth
    response = connection.getresponse.return_value
    response.status, response.reason = 401, "Unauthorized"
    response.read.return_value = b'{"error":"invalid_client"}'
    auth = APIAuthHandler.from_client_credentials(**CREDENTIALS)
    with pytest.raises(RuntimeError, match="401 Unauthorized.*invalid_client"):
        auth.get_token()
    connection.close.assert_called_once()

    response.status = 200
    response.read.return_value = b'{"access_token":"recovered"}'
    assert auth.get_token() == "recovered"
    assert connection.request.call_count == 2


@pytest.mark.parametrize("body", [b"{}", b'{"access_token":""}', b'{"access_token":null}'])
def test_missing_access_token_closes_connection(isolated_auth, body):
    connection, _, _, _ = isolated_auth
    connection.getresponse.return_value.read.return_value = body
    with pytest.raises(RuntimeError, match="No access_token"):
        APIAuthHandler.from_client_credentials(**CREDENTIALS).get_token()
    connection.close.assert_called_once()


def test_invalid_json_closes_connection(isolated_auth):
    connection, _, _, _ = isolated_auth
    connection.getresponse.return_value.read.return_value = b"not-json"
    with pytest.raises(json.JSONDecodeError):
        APIAuthHandler.from_client_credentials(**CREDENTIALS).get_token()
    connection.close.assert_called_once()


def test_network_failure_closes_connection_and_propagates(isolated_auth):
    connection, _, _, _ = isolated_auth
    connection.request.side_effect = OSError("connection refused")
    with pytest.raises(OSError, match="connection refused"):
        APIAuthHandler.from_client_credentials(**CREDENTIALS).get_token()
    connection.close.assert_called_once()
