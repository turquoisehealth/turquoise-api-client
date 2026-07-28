#!/bin/bash
set -e

# Script to test npm dev package publishing locally
# This simulates what happens in the GitHub Actions workflow for dev builds

echo "🧪 Testing npm dev package deployment locally"
echo "This matches the CI workflow steps for dev builds"
echo ""

# Get short git SHA (matches CI: SHORT_SHA="${{ github.sha }}"; SHORT_SHA="${SHORT_SHA:0:7}")
SHORT_SHA=$(git rev-parse --short=7 HEAD)
DEV_VERSION="0.0.0-dev.${SHORT_SHA}"

echo "📦 Dev version: ${DEV_VERSION}"
echo ""

# Step 1: Regenerate SDKs (matches CI workflow)
if [ -n "$FERN_TOKEN" ]; then
  echo "🌿 Regenerating SDKs with Fern..."
  fern generate
  echo ""
else
  echo "⚠️  Skipping SDK regeneration (FERN_TOKEN not set)"
  echo "   CI does: fern generate"
  echo ""
fi

# Step 2: Update library exports (matches CI workflow)
echo "📝 Updating library exports..."
python3 scripts/update_lib_exports.py
echo ""

# Step 3: Stamp package version (matches CI workflow)
echo "📝 Stamping package version to ${DEV_VERSION}..."
# CI uses: npm --prefix typescript version "$VERSION" --no-git-tag-version --allow-same-version
npm --prefix typescript version "$DEV_VERSION" --no-git-tag-version --allow-same-version
echo ""

# Backup package.json for cleanup
cp typescript/package.json typescript/package.json.backup

# Step 4: Install dependencies (matches CI workflow)
echo "📦 Installing dependencies (npm ci)..."
cd typescript
npm ci
echo ""

# Step 5: Dry-run publish (CI does: npm publish --access public --tag dev)
echo "🔍 Running dry-run publish to npm with 'dev' tag..."
echo ""

if npm publish --access public --tag dev --dry-run; then
  echo ""
  echo "✅ Dry-run successful! Package is ready to publish."
  echo ""
  echo "This matches the CI workflow. To actually publish (requires NPM_TOKEN):"
  echo "  npm publish --access public --tag dev"
  echo ""
  echo "Users would install with:"
  echo "  npm install @turquoisehealth/api@dev"
  echo ""
else
  echo ""
  echo "❌ Dry-run failed. Check the errors above."
  echo ""
fi

# Restore original package.json
echo "🔄 Restoring original package.json..."
mv package.json.backup package.json

cd ..
echo ""
echo "✨ Test complete!"
