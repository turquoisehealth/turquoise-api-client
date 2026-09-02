#!/usr/bin/env python3
"""Pull OpenAPI JSON from the specified URL."""

import argparse
import json
import sys
from pathlib import Path
from urllib.request import Request, urlopen


REPOSITORY_ROOT = Path(__file__).resolve().parent.parent
OPENAPI_FILE = REPOSITORY_ROOT / "openapi.json"


def fetch_openapi(url: str) -> None:
    print(f"Fetching OpenAPI spec from {url}...")
    request = Request(url, headers={"User-Agent": "turquoise-api-client-release-tools/1.0"})
    with urlopen(request, timeout=30) as response:
        content = response.read()

    try:
        document = json.loads(content)
    except json.JSONDecodeError as error:
        raise ValueError("response is not valid JSON") from error

    if not isinstance(document, dict) or "openapi" not in document or "info" not in document:
        raise ValueError("response is not an OpenAPI document")

    OPENAPI_FILE.write_bytes(content)
    try:
        display_path = OPENAPI_FILE.relative_to(REPOSITORY_ROOT)
    except ValueError:
        display_path = OPENAPI_FILE
    print(f"Updated {display_path}")


def main() -> None:
    parser = argparse.ArgumentParser(description="Pull an OpenAPI document into openapi.json")
    parser.add_argument(
        "url",
        nargs="?",
        default="https://turquoise.health/api/docs/openapi.json",
        help="OpenAPI document URL (default: Turquoise Health API docs)",
    )
    args = parser.parse_args()

    try:
        fetch_openapi(args.url)
    except (OSError, ValueError) as error:
        print(f"Failed to pull OpenAPI document: {error}", file=sys.stderr)
        return 1
    return 0


if __name__ == "__main__":
    raise SystemExit(main())
