# SDK Tests

The test strategy separates deterministic unit coverage of the manual libraries
from lightweight smoke tests against the live API. Both use local SDK source,
not published packages.

## Overview

The integration suites perform smoke testing by calling real API endpoints to validate that each client can successfully:

- Initialize and authenticate
- Make API requests
- Parse responses
- Call selected V3 endpoints

Only the integration tests connect to production at `https://api.turquoise.health`.
Manual-library unit tests require no credentials and never call the real API.

## Prerequisites

Before running the tests:

1. **For integration tests only**, set OAuth client credentials in the
   repository-root `.env` or as environment variables:

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

Make targets create the root Python virtual environment and install their
language's dependencies automatically. Install Python, Node.js 18+ (for native
`fetch` in auth tests), and the .NET 10 SDK. The C# unit project targets .NET 8;
its make target permits major-version runtime roll-forward when only a newer
runtime is installed.

## Running Tests

### Run Unit Coverage and API Smoke Tests

```bash
make tests
```

To run only offline manual-library coverage or only live smoke tests:

```bash
make test-unit
make test-integration
```

`make tests` includes these two suites, not every test in the repository. It
does not run the generated C# core/serialization/mock-endpoint tests outside
`Unit/Lib`, SDK regeneration tests in `scripts/tests/`, or the runnable examples.

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

Each language target runs both unit coverage and smoke tests. Append `-unit` or
`-integration` to select one suite, for example `make test-python-unit` or
`make test-csharp-integration`.

## Test Structure

### Python (`python/tests/`)

- Uses pytest framework
- Unit tests in `test_api_auth_handler.py`; smoke tests in `test_integration.py`
- Dependencies in `requirements-test.txt`

**Run directly:**

```bash
python3 -m pytest python/tests/test_integration.py -v
```

### TypeScript (`typescript/tests/`)

- Uses Jest framework
- Unit tests in `api-auth-handler.test.ts`; smoke tests in `integration.test.ts`
- Dependencies in `package.json`

**Run directly:**

```bash
cd typescript/tests
npm install
npm run test:integration
```

### C# (`csharp/tests/`)

- Uses NUnit framework
- Tests in `IntegrationTests.cs`
- Project file: `TurquoiseHealth.Api.IntegrationTests.csproj`
- Manual-library unit tests are in
  `csharp/src/TurquoiseHealth.Api.Test/Unit/Lib/APIAuthHandlerTests.cs`

**Run directly:**

```bash
cd csharp/tests
dotnet test
```

## What Gets Tested

### Manual-Library Unit Tests

Python and TypeScript mock the HTTP transport and clock. C# uses a local
WireMock server and token lifetimes that exercise cache decisions without sleeps.
Tests isolate and restore authentication environment variables.

Coverage includes:

- Static tokens, environment tokens, and missing configuration errors.
- Token-source precedence: static token, provider, then OAuth.
- Credential/environment fallback and explicit overrides.
- OAuth request URL, method, headers, and JSON body.
- Token caching, refresh buffers, default lifetime, and zero lifetime.
- Supplier/callable/provider reevaluation and error propagation.
- HTTP errors, malformed JSON, and missing/empty access tokens.
- Connection cleanup after success and failure in Python.
- C# concurrent cache access and bearer-header replacement on each request.

Python and TypeScript also test exact expiration/buffer boundaries and account
for time spent waiting for the OAuth response using mocked clocks.

### Coverage Gates and Reports

Unit targets fail below **90% line or branch coverage** of the manual libraries.
Jest additionally requires 90% statement and function coverage. Coverage includes
all files in `python/lib/`, `typescript/lib/`, and C# `TurquoiseHealth.Api.Lib.*`,
including `RefreshTokenHandler`. Generated SDK code is excluded from these gates.

Reports are generated at:

- Python: `python/tests/coverage/coverage.xml` and `coverage.json`.
- TypeScript: `typescript/tests/coverage/` (HTML, LCOV, and JSON).
- C#: `csharp/src/TurquoiseHealth.Api.Test/coverage/coverage.cobertura.xml`.

All three also print terminal summaries. Add tests when changing or extending a
manual library. Test directories and the custom C# coverage dependency
configuration are protected from Fern regeneration.

### V3 API Smoke Tests

Each client lists service packages, networks, and California providers with
`page_size=5`. Python and TypeScript also search packages for `MRI`.
There are four smoke tests each in Python and TypeScript, and three in C#.

Assertions check basic response structure and nonempty list results; search
tests only require an item array. These are not exhaustive endpoint tests and
do not test price queries or traverse pagination.

Missing credentials skip the smoke tests, so a successful command does not
necessarily mean live requests were tested. Unit coverage still runs without
credentials. Ordinary `make` stops at a failure; later suites may not run.

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
- Jest loads SDK source directly; no SDK build or npm link is needed

**Build errors (C#):**

- Ensure .NET SDK is installed: [Download here](https://dotnet.microsoft.com/download)
- Restore packages: `cd csharp/tests && dotnet restore`
- Build the main project first: `cd csharp/src/TurquoiseHealth.Api && dotnet build`
