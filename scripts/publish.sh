#!/bin/bash
set -e

# Script to tag and publish SDKs
# Usage: ./scripts/publish.sh v3.2.0

VERSION=$1

if [ -z "$VERSION" ]; then
  echo "Usage: $0 <version>"
  echo "Example: $0 v3.2.0"
  exit 1
fi

# Ensure version starts with 'v'
if [[ ! "$VERSION" =~ ^v[0-9]+\.[0-9]+\.[0-9]+(-.*)?$ ]]; then
  echo "Error: Version must be in format v*.*.* (e.g., v3.2.0)"
  exit 1
fi

# Check if tag already exists
if git rev-parse "$VERSION" >/dev/null 2>&1; then
  echo "Error: Tag $VERSION already exists"
  exit 1
fi

# Ensure we're on main branch
CURRENT_BRANCH=$(git branch --show-current)
if [ "$CURRENT_BRANCH" != "main" ]; then
  echo "Warning: You are on branch '$CURRENT_BRANCH', not 'main'"
  read -p "Continue anyway? (y/N) " -n 1 -r
  echo
  if [[ ! $REPLY =~ ^[Yy]$ ]]; then
    exit 1
  fi
fi

# Check for uncommitted changes
if ! git diff-index --quiet HEAD --; then
  echo "Error: You have uncommitted changes. Please commit or stash them first."
  exit 1
fi

# Fetch latest from origin
echo "Fetching latest changes from origin..."
git fetch origin

# Check if local branch is behind origin
if [ "$CURRENT_BRANCH" = "main" ]; then
  LOCAL=$(git rev-parse @)
  REMOTE=$(git rev-parse @{u} 2>/dev/null || echo "")

  if [ -n "$REMOTE" ]; then
    if [ "$LOCAL" != "$REMOTE" ]; then
      BASE=$(git merge-base @ @{u})
      if [ "$LOCAL" = "$BASE" ]; then
        echo "Error: Your local branch is behind origin/main. Please pull first:"
        echo "  git pull origin main"
        exit 1
      elif [ "$REMOTE" = "$BASE" ]; then
        echo "Warning: Your local branch is ahead of origin/main"
      else
        echo "Error: Your local branch has diverged from origin/main. Please sync first."
        exit 1
      fi
    fi
  fi
fi

# Confirm the action
echo "This will:"
echo "  1. Create tag: $VERSION"
echo "  2. Push tag to origin"
echo "  3. Trigger the publish workflow to release SDKs to PyPI, npm, and NuGet"
echo ""
read -p "Continue? (y/N) " -n 1 -r
echo
if [[ ! $REPLY =~ ^[Yy]$ ]]; then
  echo "Aborted."
  exit 1
fi

# Create and push tag
echo "Creating tag $VERSION..."
git tag -a "$VERSION" -m "Release $VERSION"

echo "Pushing tag to origin..."
git push origin "$VERSION"

echo ""
echo "✓ Tag $VERSION pushed successfully!"
echo "✓ GitHub Actions publish workflow will now run"
echo ""
echo "Monitor the workflow at:"
echo "  https://github.com/$(git config --get remote.origin.url | sed 's/.*github.com[:/]\(.*\)\.git/\1/')/actions"
