#!/usr/bin/env python3
"""Create a GitHub Release for a pushed release tag.

Combines the CHANGELOG.md entry for the release (the OpenAPI-level summary) with
GitHub's auto-generated release notes (the merged PR list) into one release body.
"""

import json
import re
import shutil
import subprocess
import sys
from pathlib import Path


REPOSITORY_ROOT = Path(__file__).resolve().parent.parent
OPENAPI_FILE = REPOSITORY_ROOT / "openapi.json"
CHANGELOG_FILE = REPOSITORY_ROOT / "CHANGELOG.md"


def run(command: list[str]) -> str:
    result = subprocess.run(
        command,
        cwd=REPOSITORY_ROOT,
        check=True,
        capture_output=True,
        text=True,
    )
    return result.stdout.strip()


def read_version() -> str:
    version = json.loads(OPENAPI_FILE.read_text())["info"]["version"]
    if not re.fullmatch(r"\d+\.\d+\.\d+", version):
        raise ValueError(f"openapi.json version must use production semver, got {version!r}")
    return version


def extract_changelog_section(version: str) -> str:
    text = CHANGELOG_FILE.read_text()
    match = re.search(rf"^## \[{re.escape(version)}\].*?(?=^## \[|\Z)", text, re.MULTILINE | re.DOTALL)
    if not match:
        raise ValueError(f"No CHANGELOG.md section found for {version}. Run scripts/generate_changelog.py first.")
    section = match.group(0).strip()
    if "CHANGELOG REVIEW REQUIRED" in section or "Summary pending manual review." in section:
        raise ValueError(f"CHANGELOG.md section for {version} still needs a customer-facing summary")
    return section


def validate(version: str, tag: str) -> None:
    if run(["git", "status", "--porcelain"]):
        raise ValueError("Working tree is not clean; commit or remove local changes first")

    remote_tag = subprocess.run(
        ["git", "ls-remote", "--exit-code", "origin", f"refs/tags/{tag}^{{}}"],
        cwd=REPOSITORY_ROOT,
        capture_output=True,
        text=True,
    )
    if remote_tag.returncode != 0:
        raise ValueError(f"Annotated tag {tag} is not available on origin")

    remote_commit = remote_tag.stdout.split(maxsplit=1)[0]
    current_commit = run(["git", "rev-parse", "HEAD"])
    if remote_commit != current_commit:
        raise ValueError(f"Remote tag {tag} does not point to the current commit")

    if shutil.which("gh") is None:
        raise ValueError("GitHub CLI (gh) is not installed")
    subprocess.run(["gh", "auth", "status"], cwd=REPOSITORY_ROOT, check=True)


def main() -> int:
    try:
        version = read_version()
        tag = f"v{version}"
        validate(version, tag)
        changelog_section = extract_changelog_section(version)
    except (OSError, json.JSONDecodeError, KeyError, subprocess.CalledProcessError, ValueError) as error:
        print(f"Release validation failed: {error}", file=sys.stderr)
        return 1

    repo = run(["gh", "repo", "view", "--json", "nameWithOwner", "-q", ".nameWithOwner"])

    generated_notes = ""
    generate_notes = subprocess.run(
        ["gh", "api", f"repos/{repo}/releases/generate-notes", "-f", f"tag_name={tag}", "-q", ".body"],
        cwd=REPOSITORY_ROOT,
        capture_output=True,
        text=True,
    )
    if generate_notes.returncode != 0:
        print(f"Failed to generate PR notes: {generate_notes.stderr.strip()}", file=sys.stderr)
        return 1
    generated_notes = generate_notes.stdout.strip()

    body = changelog_section
    if generated_notes:
        body += f"\n\n---\n\n{generated_notes}"

    notes_file = REPOSITORY_ROOT / f".release-notes-{version}.md"
    notes_file.write_text(body + "\n")
    try:
        answer = input(f"Create GitHub Release {tag}? [y/N] ")
        if answer.lower() != "y":
            print("Release not created.")
            return 0
        subprocess.run(
            ["gh", "release", "create", tag, "--title", tag, "--notes-file", str(notes_file)],
            cwd=REPOSITORY_ROOT,
            check=True,
        )
    finally:
        notes_file.unlink(missing_ok=True)

    print(f"Created GitHub Release {tag}")
    return 0


if __name__ == "__main__":
    raise SystemExit(main())
