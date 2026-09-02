#!/usr/bin/env python3
"""Regenerate SDKs and synchronize their package metadata."""

import json
import os
import re
import shlex
import shutil
import subprocess
from pathlib import Path
from urllib.request import urlopen


REPOSITORY_ROOT = Path(__file__).resolve().parent.parent
OPENAPI_FILE = REPOSITORY_ROOT / "openapi.json"
PYPROJECT_FILE = REPOSITORY_ROOT / "python" / "pyproject.toml"
PACKAGE_FILE = REPOSITORY_ROOT / "typescript" / "package.json"
LOCK_FILE = REPOSITORY_ROOT / "typescript" / "package-lock.json"


def load_environment() -> None:
    env_file = REPOSITORY_ROOT / ".env"
    if not env_file.exists():
        return

    for line in env_file.read_text().splitlines():
        line = line.strip()
        if not line or line.startswith("#"):
            continue
        if line.startswith("export "):
            line = line[7:].lstrip()
        if "=" not in line:
            continue
        name, value = line.split("=", 1)
        name = name.strip()
        if not name or name in os.environ:
            continue
        try:
            parsed = shlex.split(value, comments=True)
            os.environ[name] = parsed[0] if parsed else ""
        except ValueError as error:
            raise ValueError(f"Could not parse {env_file}: {error}") from error


def run_fern() -> None:
    if not shutil.which("fern"):
        raise RuntimeError("Fern CLI is not installed. Install it with: npm install -g fern-api")
    if not os.environ.get("FERN_TOKEN"):
        print("Warning: FERN_TOKEN is not set. Fern generation may fail.")

    print("Running fern generate...")
    subprocess.run(["fern", "generate"], cwd=REPOSITORY_ROOT, check=True)


def update_file(path: Path, pattern: str, replacement: str, description: str) -> None:
    content = path.read_text()
    updated, replacements = re.subn(pattern, replacement, content, count=1, flags=re.MULTILINE)
    if replacements != 1:
        raise ValueError(f"Could not find exactly one expected field in {path}")
    path.write_text(updated)
    print(f"Updated {description}")


def update_library_exports() -> None:
    init_file = REPOSITORY_ROOT / "python" / "__init__.py"
    if init_file.exists():
        content = init_file.read_text()
        if '"APIAuthHandler": ".lib"' not in content:
            content = re.sub(r'(_dynamic_imports: typing\.Dict\[str, str\] = \{)', r'\1\n    "APIAuthHandler": ".lib",', content, count=1)
        if '"lib": ".lib"' not in content:
            content = re.sub(r'("consumer_pricing": "\.consumer_pricing",)', r'\1\n    "lib": ".lib",', content, count=1)
        all_match = re.search(r'__all__ = \[(.*?)\]', content, re.DOTALL)
        if all_match and '"APIAuthHandler"' not in all_match.group(1):
            content = re.sub(r'(__all__ = \[)', r'\1\n    "APIAuthHandler",', content, count=1)
        all_match = re.search(r'__all__ = \[(.*?)\]', content, re.DOTALL)
        if all_match and '"lib"' not in all_match.group(1):
            content = re.sub(r'("consumer_pricing",\n\])', r'"consumer_pricing",\n    "lib",\n]', content, count=1)
        init_file.write_text(content)
        print("Updated python/__init__.py exports")

    index_file = REPOSITORY_ROOT / "typescript" / "index.ts"
    if index_file.exists() and 'from "./lib/index.js"' not in index_file.read_text():
        index_file.write_text(index_file.read_text().rstrip() + '\nexport * as lib from "./lib/index.js";\n')
        print("Updated typescript/index.ts exports")

    jsonable_encoder_file = REPOSITORY_ROOT / "python" / "core" / "jsonable_encoder.py"
    if jsonable_encoder_file.exists():
        content = jsonable_encoder_file.read_text()
        if "from types import GeneratorType" in content:
            content = content.replace("from types import GeneratorType", "import types as stdlib_types")
            content = content.replace("if isinstance(obj, (list, set, frozenset, GeneratorType, tuple)):", "if isinstance(obj, (list, set, frozenset, stdlib_types.GeneratorType, tuple)):")
            jsonable_encoder_file.write_text(content)
            print("Fixed python/types import conflict")


def update_package_versions() -> None:
    with OPENAPI_FILE.open() as file:
        version = json.load(file)["info"]["version"]
    if not re.fullmatch(r"\d+\.\d+\.\d+", version):
        raise ValueError(f"OpenAPI version must use production semver, got {version!r}")

    update_file(PYPROJECT_FILE, r'^(version = ")[^"]+(")$', rf'\g<1>{version}\g<2>', "python/pyproject.toml version")
    update_file(PACKAGE_FILE, r'^(  "version": ")[^"]+(",)$', rf'\g<1>{version}\g<2>', "typescript/package.json version")
    update_file(LOCK_FILE, r'^(  "version": ")[^"]+(",)$', rf'\g<1>{version}\g<2>', "typescript/package-lock.json version")
    update_file(LOCK_FILE, r'^(      "version": ")[^"]+(",)$', rf'\g<1>{version}\g<2>', "typescript lock package version")


def main() -> None:
    load_environment()
    run_fern()
    update_library_exports()
    update_package_versions()
    print("SDK regeneration complete!")


if __name__ == "__main__":
    main()
