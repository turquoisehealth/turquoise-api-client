#!/usr/bin/env python3
"""Prepend a CHANGELOG.md review draft and save raw oasdiff output.

The --initial-release option writes an initial-release entry instead.
"""

import argparse
import json
import re
import shutil
import subprocess
import sys
from datetime import date
from pathlib import Path


REPOSITORY_ROOT = Path(__file__).resolve().parent.parent
OPENAPI_FILE = REPOSITORY_ROOT / "openapi.json"
CHANGELOG_FILE = REPOSITORY_ROOT / "CHANGELOG.md"
OASDIFF_DIRECTORY = REPOSITORY_ROOT / "oasdiffs"

CHANGELOG_HEADER = """# Changelog

All notable API-level changes to this SDK are documented in this file. Entries are
generated from the OpenAPI diff between releases via [oasdiff](https://github.com/oasdiff/oasdiff)
(see `scripts/generate_changelog.py`) and reviewed/edited as part of each release PR.

Given one client is generated per released API version, the version numbers below match
the `vX.X.XX` tags and package versions published for Python, TypeScript, and C#.
"""


def read_version() -> str:
    version = json.loads(OPENAPI_FILE.read_text())["info"]["version"]
    if not re.fullmatch(r"\d+\.\d+\.\d+", version):
        raise ValueError(f"openapi.json version must use production semver, got {version!r}")
    return version


def previous_tag(allow_initial_release: bool) -> str | None:
    result = subprocess.run(
        ["git", "describe", "--tags", "--abbrev=0", "--match", "v*"],
        cwd=REPOSITORY_ROOT,
        capture_output=True,
        text=True,
    )
    if result.returncode == 0:
        return result.stdout.strip()
    if allow_initial_release:
        return None

    detail = result.stderr.strip() or "no matching release tag was found"
    raise RuntimeError(
        "Could not find the previous release tag. Fetch tags with "
        "'git fetch --tags origin' and retry. For a deliberate first release only, "
        "rerun with '--initial-release'. Git error: "
        f"{detail}"
    )


def build_entry(version: str, allow_initial_release: bool) -> str:
    tag = previous_tag(allow_initial_release)
    if tag is None:
        body = "Initial release."
    else:
        if not shutil.which("oasdiff"):
            raise RuntimeError(
                "oasdiff is not installed. Install it with: curl -fsSL "
                "https://raw.githubusercontent.com/oasdiff/oasdiff/main/install.sh | sh"
            )
        result = subprocess.run(
            ["oasdiff", "changelog", f"{tag}:openapi.json", "openapi.json", "--format", "markdown"],
            cwd=REPOSITORY_ROOT,
            capture_output=True,
            text=True,
        )
        if result.returncode != 0:
            raise RuntimeError(f"oasdiff changelog failed: {result.stderr.strip()}")
        raw_report = result.stdout.strip() or "No consumer-facing API changes."
        OASDIFF_DIRECTORY.mkdir(parents=True, exist_ok=True)
        report_file = OASDIFF_DIRECTORY / f"{version}.md"
        report_file.write_text(raw_report + "\n", encoding="utf-8")
        print(f"Raw oasdiff report written to {report_file}")
        body = (
            "<!-- CHANGELOG REVIEW REQUIRED: replace this placeholder with a "
            f"concise customer-facing summary after reviewing `{report_file.relative_to(REPOSITORY_ROOT)}`. -->\n\n"
            "Summary pending manual review."
        )
    return f"## [{version}] - {date.today().isoformat()}\n\n{body}\n"


def update_changelog(version: str, allow_initial_release: bool) -> None:
    content = CHANGELOG_FILE.read_text() if CHANGELOG_FILE.exists() else CHANGELOG_HEADER
    existing_section = re.search(rf"^## \[{re.escape(version)}\].*?(?=^## \[|\Z)", content, re.MULTILINE | re.DOTALL)
    if existing_section and "CHANGELOG REVIEW REQUIRED" not in existing_section.group(0):
        print(f"CHANGELOG.md already has an entry for {version}, skipping")
        return

    entry = build_entry(version, allow_initial_release)
    if existing_section:
        updated = content[:existing_section.start()] + entry + content[existing_section.end():]
        CHANGELOG_FILE.write_text(updated.strip() + "\n")
        print(f"Refreshed the {version} review entry in CHANGELOG.md")
        return

    marker = re.search(r"^## \[", content, re.MULTILINE)
    insert_at = marker.start() if marker else len(content)
    updated = content[:insert_at].rstrip() + "\n\n" + entry + "\n" + content[insert_at:]
    CHANGELOG_FILE.write_text(updated.strip() + "\n")
    print(f"Updated CHANGELOG.md with the {version} entry")


def main() -> int:
    parser = argparse.ArgumentParser()
    parser.add_argument(
        "--initial-release",
        action="store_true",
        help="Allow an initial-release entry when no previous release tag exists",
    )
    arguments = parser.parse_args()

    try:
        update_changelog(read_version(), arguments.initial_release)
    except (OSError, json.JSONDecodeError, KeyError, RuntimeError, ValueError) as error:
        print(f"Failed to update CHANGELOG.md: {error}", file=sys.stderr)
        return 1
    return 0


if __name__ == "__main__":
    raise SystemExit(main())
