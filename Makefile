.PHONY: help tests test-python test-typescript test-csharp install-test-deps clean venv

.DEFAULT_GOAL := help

ifneq ($(wildcard .env),)
include .env
export TURQUOISE_CLIENT_ID TURQUOISE_CLIENT_SECRET TURQUOISE_ORGANIZATION_ID
endif

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
	@echo ""
	@echo "  make clean            - Clean test artifacts"
	@echo ""
	@echo "Prerequisites:"
	@echo "  - Set TURQUOISE_CLIENT_ID, TURQUOISE_CLIENT_SECRET, and TURQUOISE_ORGANIZATION_ID in .env or the environment before running tests"
	@echo "  - Run 'make venv' to create Python virtual environment (recommended)"
	@echo "  - Install dependencies: make install-test-deps"
	@echo "  - Install Fern CLI: npm install -g fern-api"

# Run all tests
tests: test-python test-typescript test-csharp
	@echo ""
	@echo "✓ All integration tests completed successfully!"

# Python tests
test-python: install-test-deps
	@echo "Running Python integration tests..."
	@PYTHONPATH=. $(PYTHON) -m pytest python/tests/test_integration.py -v -s

# TypeScript tests
test-typescript: install-test-deps
	@echo "Running TypeScript integration tests..."
	@cd typescript/tests && \
		npm test

# C# tests
test-csharp: install-test-deps
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
	@echo "Installing Python SDK dependencies and test dependencies..."
	@$(PIP) install httpx pydantic typing-extensions
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

# End of Makefile

