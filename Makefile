.PHONY: help test test-python test-typescript test-csharp install-test-deps clean venv update-lib-exports generate publish

# Python virtual environment directory
VENV := venv
PYTHON := $(VENV)/bin/python3
PIP := $(VENV)/bin/pip

# Default target
help:
	@echo "Turquoise Health API Client - Test Suite"
	@echo ""
	@echo "Available targets:"
	@echo "  make venv              - Create Python virtual environment"
	@echo "  make test              - Run all integration tests (Python, TypeScript, C#)"
	@echo "  make test-python       - Run Python integration tests"
	@echo "  make test-typescript   - Run TypeScript integration tests"
	@echo "  make test-csharp       - Run C# integration tests"
	@echo "  make install-test-deps - Install test dependencies for all languages"
	@echo "  make update-lib-exports - Update SDK exports after Fern generation"
	@echo "  make generate          - Regenerate SDKs from openapi.json (requires FERN_TOKEN)"
	@echo "  make publish VERSION=v1.0.0 - Tag and publish SDKs to registries"
	@echo "  make clean            - Clean test artifacts"
	@echo ""
	@echo "Prerequisites:"
	@echo "  - Set TURQUOISE_API_TOKEN environment variable before running tests"
	@echo "  - Set FERN_TOKEN environment variable before running 'make generate'"
	@echo "  - Run 'make venv' to create Python virtual environment (recommended)"
	@echo "  - Install dependencies: make install-test-deps"
	@echo "  - Install Fern CLI: npm install -g fern-api"

# Run all tests
test: test-python test-typescript test-csharp
	@echo ""
	@echo "✓ All integration tests completed successfully!"

# Python tests
test-python: venv
	@echo "Running Python integration tests..."
	@$(PYTHON) -m pytest python/tests/test_integration.py -v -s

# TypeScript tests
test-typescript:
	@echo "Running TypeScript integration tests..."
	@cd typescript/tests && \
		npm test

# C# tests
test-csharp:
	@echo "Running C# integration tests..."
	@cd csharp/tests && \
		dotnet test --verbosity normal

# Create Python virtual environment
venv:
	@if [ ! -d "$(VENV)" ]; then \
		echo "Creating Python virtual environment..."; \
		python3 -m venv $(VENV); \
		echo "✓ Virtual environment created at $(VENV)"; \
	fi

# Install test dependencies
install-test-deps: venv
	@echo "Installing test dependencies..."
	@echo "Installing Python test dependencies..."
	@$(PIP) install -r python/tests/requirements-test.txt
	@echo "Installing TypeScript test dependencies..."
	@cd typescript/tests && npm install
	@echo "Installing C# test dependencies..."
	@if command -v dotnet >/dev/null 2>&1; then \
		cd csharp/tests && dotnet restore; \
	else \
		echo "⚠  Skipping C# dependencies (.NET SDK not found)"; \
	fi
	@echo "✓ All test dependencies installed"

# Clean test artifacts
clean:
	@echo "Cleaning test artifacts..."
	@rm -rf $(VENV) 2>/dev/null || true
	@find python -type d -name "__pycache__" -exec rm -rf {} + 2>/dev/null || true
	@find python -type d -name ".pytest_cache" -exec rm -rf {} + 2>/dev/null || true
	@find python -type d -name "*.egg-info" -exec rm -rf {} + 2>/dev/null || true
	@rm -rf typescript/tests/node_modules 2>/dev/null || true
	@rm -rf typescript/tests/coverage 2>/dev/null || true
	@find csharp -type d -name "bin" -exec rm -rf {} + 2>/dev/null || true
	@find csharp -type d -name "obj" -exec rm -rf {} + 2>/dev/null || true
	@find csharp -type d -name "TestResults" -exec rm -rf {} + 2>/dev/null || true
	@echo "✓ Cleaned test artifacts"

# Update library exports after Fern generation
update-lib-exports:
	@echo "Updating SDK exports to include manual libraries..."
	@python3 scripts/update_lib_exports.py

# Regenerate SDKs from OpenAPI spec
generate:
	@./scripts/generate.sh

# Publish SDKs by creating and pushing a version tag
publish:
	@if [ -z "$(VERSION)" ]; then \
		echo "Error: VERSION is required"; \
		echo "Usage: make publish VERSION=v3.2.0"; \
		exit 1; \
	fi
	@./scripts/publish.sh $(VERSION)
