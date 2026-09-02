#!/usr/bin/env python3
"""Validate a release checkout and trigger the GitHub publish workflow."""

import argparse
import json
import re
import shutil
import subprocess
import sys
from pathlib import Path


REPOSITORY_ROOT = Path(__file__).resolve().parent.parent
OPENAPI_FILE = REPOSITORY_ROOT / "openapi.json"
PYPROJECT_FILE = REPOSITORY_ROOT / "python" / "pyproject.toml"
PACKAGE_FILE = REPOSITORY_ROOT / "typescript" / "package.json"
LOCK_FILE = REPOSITORY_ROOT / "typescript" / "package-lock.json"
CSHARP_VERSION_FILE = REPOSITORY_ROOT / "csharp" / "src" / "TurquoiseHealth.Api" / "Core" / "Public" / "Version.cs"


def run(command: list[str]) -> str:
    result = subprocess.run(
        command,
        cwd=REPOSITORY_ROOT,
        check=True,
        capture_output=True,
        text=True,
    )
    return result.stdout.strip()


def current_branch() -> str:
    result = subprocess.run(
        ["git", "symbolic-ref", "--quiet", "--short", "HEAD"],
        cwd=REPOSITORY_ROOT,
        capture_output=True,
        text=True,
    )
    return result.stdout.strip() if result.returncode == 0 else ""


def read_versions() -> dict[str, str]:
    openapi_version = json.loads(OPENAPI_FILE.read_text())["info"]["version"]
    pyproject_version = re.search(
        r'^version = "([^"]+)"$', PYPROJECT_FILE.read_text(), re.MULTILINE
    )
    if pyproject_version is None:
        raise ValueError(f"Could not find the version in {PYPROJECT_FILE}")

    package_version = json.loads(PACKAGE_FILE.read_text())["version"]
    lock_version = json.loads(LOCK_FILE.read_text())["packages"][""]["version"]
    csharp_version = re.search(
        r'^    public const string Current = "([^"]+)";$',
        CSHARP_VERSION_FILE.read_text(),
        re.MULTILINE,
    )
    if csharp_version is None:
        raise ValueError(f"Could not find the version in {CSHARP_VERSION_FILE}")

    return {
        "openapi.json": openapi_version,
        "python/pyproject.toml": pyproject_version.group(1),
        "typescript/package.json": package_version,
        "typescript/package-lock.json": lock_version,
        "csharp/src/TurquoiseHealth.Api/Core/Public/Version.cs": csharp_version.group(1),
    }


def parse_args() -> argparse.Namespace:
    parser = argparse.ArgumentParser(
        description="Validate a release and trigger publish.yml on GitHub Actions."
    )
    parser.add_argument(
        "--sdk",
        choices=("all", "python", "typescript", "csharp"),
        default="all",
        help="SDK to publish (default: all)",
    )
    parser.add_argument(
        "--dry",
        action="store_true",
        help="Validate readiness without triggering the workflow",
    )
    return parser.parse_args()


def validate() -> tuple[str, str]:
    versions = read_versions()
    version = versions["openapi.json"]
    if not re.fullmatch(r"\d+\.\d+\.\d+", version):
        raise ValueError("Version must use production semver, for example 3.2.68")

    tag = f"v{version}"
    current_ref = current_branch()
    if current_ref:
        raise ValueError(f"Checkout is on branch {current_ref}; check out {tag} first")

    current_commit = run(["git", "rev-parse", "HEAD"])
    tag_commit = run(["git", "rev-list", "-n", "1", tag])
    if tag_commit != current_commit:
        raise ValueError(f"Tag {tag} does not point to the current commit")

    remote_tag = subprocess.run(
        ["git", "ls-remote", "--exit-code", "origin", f"refs/tags/{tag}^{{}}"],
        cwd=REPOSITORY_ROOT,
        capture_output=True,
        text=True,
    )
    if remote_tag.returncode != 0:
        raise ValueError(f"Annotated tag {tag} is not available on origin")
    remote_commit = remote_tag.stdout.split(maxsplit=1)[0]
    if remote_commit != current_commit:
        raise ValueError(f"Remote tag {tag} does not point to the current commit")

    if run(["git", "status", "--porcelain"]):
        raise ValueError("Working tree is not clean; commit or remove local changes first")

    mismatches = {path: value for path, value in versions.items() if value != version}
    if mismatches:
        details = ", ".join(f"{path}={value}" for path, value in mismatches.items())
        raise ValueError(f"Package metadata does not match {version}: {details}")

    if shutil.which("gh") is None:
        raise ValueError("GitHub CLI (gh) is not installed")
    subprocess.run(["gh", "auth", "status"], cwd=REPOSITORY_ROOT, check=True)
    return tag, version


def main() -> int:
    args = parse_args()
    try:
        tag, version = validate()
    except (OSError, json.JSONDecodeError, KeyError, subprocess.CalledProcessError, ValueError) as error:
        print(f"Release validation failed: {error}", file=sys.stderr)
        return 1

    print(f"Release validation passed for {tag}")
    print(f"SDK selection: {args.sdk}")
    if args.dry:
        return 0

    answer = input(f"Trigger publish.yml for {tag} ({args.sdk})? [y/N] ")
    if answer.lower() != "y":
        print("Publish workflow not triggered.")
        return 0

    command = [
        "gh",
        "workflow",
        "run",
        "publish.yml",
        "--ref",
        tag,
        "-f",
        f"version={version}",
        "-f",
        f"sdk={args.sdk}",
    ]
    subprocess.run(command, cwd=REPOSITORY_ROOT, check=True)
    print(f"Publish workflow triggered for {tag}")
    return 0


if __name__ == "__main__":
    raise SystemExit(main())
