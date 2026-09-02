# Integration Tests

This repository contains integration tests for the Turquoise Health API clients.

## Overview

These tests perform smoke testing by calling real API endpoints to validate that each client can successfully:

- Initialize and authenticate
- Make API requests
- Parse responses
- Handle various endpoint types (v1 and v2)

The tests connect to the production API at `https://api.turquoise.health`.

## Prerequisites

Before running the tests:

1. **Set OAuth client credentials** in `.env` or as environment variables:

   ```bash
   export TURQUOISE_CLIENT_ID="your-client-id"
   export TURQUOISE_CLIENT_SECRET="your-client-secret"
   export TURQUOISE_ORGANIZATION_ID="your-org-id"
   ```

2. **Create a Python virtual environment:**

   ```bash
   python3 -m venv venv
   source venv/bin/activate  # On macOS/Linux
   # or
   venv\Scripts\activate  # On Windows
   ```

3. **Install test dependencies** for all languages:

   ```bash
   make install-test-deps
   ```

## Running Tests

### Run All Tests

```bash
make test
```

### Run Tests by Language

**Python only:**

```bash
make test-python
```

**TypeScript only:**

```bash
make test-typescript
```

**C# only:**

```bash
make test-csharp
```

## Test Structure

### Python (`python/tests/`)

- Uses pytest framework
- Tests in `test_integration.py`
- Dependencies in `requirements-test.txt`

**Run directly:**

```bash
python3 -m pytest python/tests/test_integration.py -v
```

### TypeScript (`typescript/tests/`)

- Uses Jest framework
- Tests in `integration.test.ts`
- Dependencies in `package.json`

**Run directly:**

```bash
cd typescript/tests
npm install
npm test
```

### C# (`csharp/tests/`)

- Uses NUnit framework
- Tests in `IntegrationTests.cs`
- Project file: `TurquoiseHealth.Api.IntegrationTests.csproj`

**Run directly:**

```bash
cd csharp/tests
dotnet test
```

## What Gets Tested

Each client tests the following endpoints:

### V1 Endpoints

- `getSSPs()` - List shoppable service packages
- `getSSPs(search)` - Search SSPs by keyword
- `getInsuranceNetworks()` - List insurance networks
- `getProviders()` - List healthcare providers

### V2 Endpoints

- `v2ListSsps()` - List SSPs (v2)
- `v2ListNetworks()` - List networks (v2)
- `v2ListProviders()` - List providers (v2)

## Cleaning Up

To remove test artifacts and dependencies:

```bash
make clean
```

## Troubleshooting

**Tests are skipped:**

- Ensure `TURQUOISE_CLIENT_ID`, `TURQUOISE_CLIENT_SECRET`, and `TURQUOISE_ORGANIZATION_ID` are set in your environment or `.env`

**Import errors (Python):**

- Ensure you're running tests from the project root directory
- Install test dependencies: `python3 -m pip install -r python/tests/requirements-test.txt`

**Module not found (TypeScript):**

- Install dependencies: `cd typescript/tests && npm install`
- Ensure parent package is built

**Build errors (C#):**

- Ensure .NET SDK is installed: [Download here](https://dotnet.microsoft.com/download)
- Restore packages: `cd csharp/tests && dotnet restore`
- Build the main project first: `cd csharp/src/TurquoiseHealth.Api && dotnet build`

## Contributing

When adding new endpoints to the API clients:

1. Add corresponding test cases to each language's integration test file
2. Ensure tests follow the existing pattern
3. Run `make test` to verify all clients work correctly
