#!/bin/bash
set -e

# Script to regenerate SDKs locally (same as generate.yml workflow)
# Usage: ./scripts/generate.sh [openapi-url]

OPENAPI_URL=$1

echo "Regenerating SDKs from OpenAPI spec..."
echo ""

# Check if Fern CLI is installed
if ! command -v fern >/dev/null 2>&1; then
  echo "Error: Fern CLI is not installed."
  echo "Install it with: npm install -g fern-api"
  exit 1
fi

# Update openapi.json from URL if provided
if [ -n "$OPENAPI_URL" ]; then
  echo "Fetching updated OpenAPI spec from $OPENAPI_URL..."
  curl -fsSL "$OPENAPI_URL" -o openapi.json
  echo "✓ Updated openapi.json"
  echo ""
fi

# Check if FERN_TOKEN is set
if [ -z "$FERN_TOKEN" ]; then
  echo "Warning: FERN_TOKEN is not set. Fern generation may fail."
  echo "Set it with: export FERN_TOKEN=your-token"
  echo ""
fi

# Generate SDKs
echo "Running fern generate..."
fern generate

echo ""
echo "Updating library exports..."
python3 scripts/update_lib_exports.py

echo ""
echo "✓ SDK regeneration complete!"
echo ""
echo "Next steps:"
echo "  1. Review the changes: git status"
echo "  2. Test the SDKs: make test"
echo "  3. Commit the changes: git add . && git commit -m 'chore: regenerate SDKs'"
