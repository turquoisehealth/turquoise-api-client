.PHONY: help tests test-unit test-integration test-python test-typescript test-csharp \
	test-python-unit test-typescript-unit test-csharp-unit \
	test-python-integration test-typescript-integration test-csharp-integration \
	install-test-deps install-python-test-deps install-typescript-test-deps \
	install-csharp-test-deps clean venv

.DEFAULT_GOAL := help

ifneq ($(wildcard .env),)
include .env
export TURQUOISE_CLIENT_ID TURQUOISE_CLIENT_SECRET TURQUOISE_ORGANIZATION_ID
endif

# Python virtual environment directory
VENV := venv
PYTHON := $(VENV)/bin/python3
PIP := $(VENV)/bin/pip
CSHARP_UNIT_PROJECT := csharp/src/TurquoiseHealth.Api.Test/TurquoiseHealth.Api.Test.csproj

# Default target
help:
	@echo "Turquoise Health API Client - Test Suite"
	@echo ""
	@echo "Available targets:"
	@echo "  make venv              - Create Python virtual environment"
	@echo "  make tests             - Run manual-library unit coverage and API smoke tests"
	@echo "  make test-unit         - Run offline manual-library tests with coverage gates"
	@echo "  make test-integration  - Run live API smoke tests (requires credentials)"
	@echo "  make test-python       - Run Python unit coverage and API smoke tests"
	@echo "  make test-typescript   - Run TypeScript unit coverage and API smoke tests"
	@echo "  make test-csharp       - Run C# unit coverage and API smoke tests"
	@echo "  Append -unit or -integration to a language target to run just that suite"
	@echo "  make install-test-deps - Install test dependencies for all languages"
	@echo ""
	@echo "  make clean            - Clean test artifacts"
	@echo ""
	@echo "Prerequisites:"
	@echo "  - Set TURQUOISE_CLIENT_ID, TURQUOISE_CLIENT_SECRET, and TURQUOISE_ORGANIZATION_ID for live API tests only"
	@echo "  - Run 'make venv' to create Python virtual environment (recommended)"
	@echo "  - Install dependencies: make install-test-deps"

# Run all tests
tests: test-unit test-integration
	@echo ""
	@echo "✓ Unit coverage and integration test runs completed (check for skipped API tests)."

test-unit: test-python-unit test-typescript-unit test-csharp-unit

test-integration: test-python-integration test-typescript-integration test-csharp-integration

test-python: test-python-unit test-python-integration
test-typescript: test-typescript-unit test-typescript-integration
test-csharp: test-csharp-unit test-csharp-integration

test-python-unit: install-python-test-deps
	@echo "Running offline Python manual-library tests..."
	@PYTHONPATH=. $(PYTHON) -m pytest python/tests --ignore=python/tests/test_integration.py \
		--cov=python.lib --cov-branch --cov-fail-under=90 \
		--cov-report=term-missing --cov-report=xml:python/tests/coverage/coverage.xml \
		--cov-report=json:python/tests/coverage/coverage.json -v
	@$(PYTHON) -c 'import json; totals = json.load(open("python/tests/coverage/coverage.json"))["totals"]; \
		raise SystemExit(0 if totals["num_statements"] > 0 and totals["num_branches"] > 0 \
		and totals["covered_lines"] >= 0.9 * totals["num_statements"] \
		and totals["covered_branches"] >= 0.9 * totals["num_branches"] \
		else "Manual-library line and branch coverage must each be at least 90%")'

test-typescript-unit: install-typescript-test-deps
	@echo "Running offline TypeScript manual-library tests..."
	@cd typescript/tests && npm run test:unit

test-csharp-unit: install-csharp-test-deps
	@echo "Running offline C# manual-library tests..."
	@DOTNET_ROLL_FORWARD=Major dotnet test $(CSHARP_UNIT_PROJECT) \
		--filter "FullyQualifiedName~TurquoiseHealth.Api.Test.Unit.Lib" \
		/p:CollectCoverage=true '/p:Include=[TurquoiseHealth.Api]TurquoiseHealth.Api.Lib.*' \
		/p:CoverletOutput=./coverage/ /p:CoverletOutputFormat=cobertura \
		/p:Threshold=90 /p:ThresholdType=line%2cbranch --verbosity minimal

test-python-integration: install-python-test-deps
	@echo "Running Python integration tests..."
	@PYTHONPATH=. $(PYTHON) -m pytest python/tests/test_integration.py -v -s

# TypeScript tests
test-typescript-integration: install-typescript-test-deps
	@echo "Running TypeScript integration tests..."
	@cd typescript/tests && \
		npm run test:integration

# C# tests
test-csharp-integration: install-csharp-test-deps
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
install-test-deps: install-python-test-deps install-typescript-test-deps install-csharp-test-deps
	@echo "✓ All test dependencies installed"

install-python-test-deps: venv
	@echo "Installing Python SDK dependencies and test dependencies..."
	@$(PIP) install httpx pydantic typing-extensions
	@$(PIP) install -r python/tests/requirements-test.txt

install-typescript-test-deps:
	@echo "Installing TypeScript test dependencies..."
	@cd typescript/tests && npm install

install-csharp-test-deps:
	@echo "Installing C# test dependencies..."
	@dotnet restore csharp/tests/TurquoiseHealth.Api.IntegrationTests.csproj
	@dotnet restore $(CSHARP_UNIT_PROJECT)

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
