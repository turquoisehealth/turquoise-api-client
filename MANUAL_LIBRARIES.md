# Manual Libraries Integration

This document describes how to add manually-coded (non-Fern-generated) libraries to the SDK packages.

## Overview

Each language SDK has a `lib/` directory for hand-written code that extends the generated SDK:

- **Python**: `python/lib/`
- **TypeScript**: `typescript/lib/`
- **C#**: `csharp/src/TurquoiseHealth.Api/Lib/`

These directories are protected from deletion during Fern regeneration via `.fernignore` files.

## Directory Structure

```
python/
├── .fernignore          # Protects lib/ from deletion
├── __init__.py          # Auto-generated (requires manual additions)
└── lib/
    ├── __init__.py      # Manual library exports
    └── api_auth_handler.py  # Example custom library

typescript/
├── .fernignore          # Protects lib/ from deletion
├── index.ts             # Auto-generated (requires manual additions)
└── lib/
    ├── index.ts         # Manual library exports
    └── APIAuthHandler.ts    # Example custom library

csharp/
├── .fernignore          # Protects Lib/ from deletion
└── src/
    └── TurquoiseHealth.Api/
        └── Lib/
            └── APIAuthHandler.cs  # Example custom library
```

## After Fern Regeneration

When you run `fern generate`, you need to re-add the manual library exports to the generated files:

### Python (`python/__init__.py`)

Add to the `_dynamic_imports` dictionary:
```python
_dynamic_imports: typing.Dict[str, str] = {
    "APIAuthHandler": ".lib",  # Add this
    "AsyncTurquoiseHealth": ".client",
    # ... rest of generated imports
    "lib": ".lib",  # Add this at the end
}
```

Add to the `__all__` list:
```python
__all__ = [
    "APIAuthHandler",  # Add this
    "AsyncTurquoiseHealth",
    # ... rest of generated exports
    "lib",  # Add this at the end
]
```

### TypeScript (`typescript/index.ts`)

Add at the end of the file:
```typescript
export * as lib from "./lib/index.js";
```

### C#

No changes needed - the `Lib` namespace is automatically included.

## Usage Examples

### Python

```python
from turquoise_health import TurquoiseHealth, APIAuthHandler

# Use the custom auth handler
auth = APIAuthHandler.from_env()
client = TurquoiseHealth(token=auth.get_token())

# Or import from lib module
from turquoise_health.lib import APIAuthHandler
```

### TypeScript

```typescript
import { TurquoiseHealthApiClient, lib } from "@turquoise-health/api";

// Use the custom auth handler
const auth = lib.APIAuthHandler.fromEnv();
const client = new TurquoiseHealthApiClient({ token: await auth.getToken() });
```

### C#

```csharp
using TurquoiseHealth.Api;
using TurquoiseHealth.Api.Lib;

// Use the custom auth handler
var auth = APIAuthHandler.FromEnv();
var client = new TurquoiseHealthApiClient(token: auth.GetToken());
```

## Adding New Manual Libraries

1. Create your library file in the appropriate `lib/` directory
2. Export it from the `lib/__init__.py` or `lib/index.ts` file
3. After Fern regeneration, re-add the exports to the main export files (see above)
4. Run the post-generation script: `make update-lib-exports` (if available)

## Automated Export Maintenance

Run this script after Fern generation to automatically add lib exports:

```bash
python scripts/update_lib_exports.py
```

Or use the Make target:

```bash
make update-lib-exports
```
