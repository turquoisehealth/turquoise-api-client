#!/bin/bash
set -e

# Script to trigger SDK publishing via GitHub workflow dispatch
# Usage: ./scripts/publish-sdk.sh <sdk> [version] [type]
# sdk: python, typescript, csharp, or all
# version: version to publish (optional, reads from openapi.json if not provided)
# type: production or dev (default: production)

SDK=$1
VERSION=$2
TYPE=${3:-production}

if [ -z "$SDK" ]; then
  echo "Error: SDK type required"
  echo "Usage: $0 <sdk> [version] [type]"
  echo "  sdk:     python, typescript, csharp, or all"
  echo "  version: version to publish (optional, reads from openapi.json)"
  echo "  type:    production or dev (default: production)"
  exit 1
fi

# Validate SDK type
if [[ ! "$SDK" =~ ^(python|typescript|csharp|all)$ ]]; then
  echo "Error: SDK must be one of: python, typescript, csharp, all"
  exit 1
fi

# Validate release type
if [[ ! "$TYPE" =~ ^(production|dev)$ ]]; then
  echo "Error: Type must be either 'production' or 'dev'"
  exit 1
fi

# Determine version if not provided
if [ -z "$VERSION" ]; then
  echo "No version provided, reading from openapi.json..."

  if [ ! -f "openapi.json" ]; then
    echo "Error: openapi.json not found"
    exit 1
  fi

  # Extract version field
  if command -v jq >/dev/null 2>&1; then
    VERSION=$(jq -r '.info.version' openapi.json)
  else
    # Fallback: use grep and sed if jq is not available
    VERSION=$(grep -A 2 '"version"' openapi.json | grep -o '[0-9]\+\.[0-9]\+\.[0-9]\+' | head -1)
  fi

  if [ -z "$VERSION" ]; then
    echo "Error: Could not extract version from openapi.json"
    exit 1
  fi

  echo "Using version from openapi.json: $VERSION"
fi

# For dev releases, append a short SHA if not already present
if [[ "$TYPE" == "dev" && ! "$VERSION" =~ -dev\. ]]; then
  SHORT_SHA=$(git rev-parse --short HEAD)
  VERSION="0.0.0-dev.${SHORT_SHA}"
  echo "Dev version: $VERSION"
fi

# Check if gh CLI is installed
if ! command -v gh >/dev/null 2>&1; then
  echo "Error: GitHub CLI (gh) is not installed"
  echo "Install it from: https://cli.github.com/"
  exit 1
fi

# Check if user is authenticated
if ! gh auth status >/dev/null 2>&1; then
  echo "Error: Not authenticated with GitHub CLI"
  echo "Run: gh auth login"
  exit 1
fi

# Confirm the action
echo ""
echo "This will trigger a GitHub Actions workflow to publish:"
echo "  SDK(s):  $SDK"
echo "  Version: $VERSION"
echo "  Type:    $TYPE"
echo ""
read -p "Continue? (y/N) " -n 1 -r
echo
if [[ ! $REPLY =~ ^[Yy]$ ]]; then
  echo "Aborted."
  exit 1
fi

# Trigger workflow dispatch
echo "Triggering workflow dispatch..."
gh workflow run publish.yml \
  -f version="$VERSION" \
  -f release_type="$TYPE" \
  -f sdk="$SDK"

echo ""
echo "✓ Workflow dispatch triggered successfully!"
echo "✓ View the workflow run at: https://github.com/$(gh repo view --json nameWithOwner -q .nameWithOwner)/actions"
echo ""
