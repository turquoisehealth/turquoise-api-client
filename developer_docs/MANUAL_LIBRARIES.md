# Custom Libraries System

**Developer Documentation**: This guide is for maintainers and contributors working on the SDK itself.

Complete guide to adding manually-coded (non-Fern-generated) libraries to the SDK packages.

## Table of Contents

- [Overview](#overview)
- [Directory Structure](#directory-structure)
- [Testing Custom Library Imports](#testing-custom-library-imports)
- [Adding New Libraries](#adding-new-libraries)
- [After Fern Regeneration](#after-fern-regeneration)
- [Developer Checklist](#developer-checklist)
- [Best Practices](#best-practices)
- [Troubleshooting](#troubleshooting)
- [CI/CD Integration](#cicd-integration)

## Overview

This repository supports **manually-coded libraries** that will be bundled into the published SDK packages alongside the Fern-generated code. Users can import these custom utilities just like the generated API client.

### Key Features

1. **Protected lib directories** - Won't be deleted during Fern regeneration
2. **Automatic export management** - Script updates exports after generation
3. **CI/CD integration** - GitHub workflows automatically maintain exports
4. **Multi-language support** - Python, TypeScript, and C# all configured
5. **Example implementation** - APIAuthHandler utility included

### Language-Specific Directories

Each language SDK has a `lib/` directory for hand-written code that extends the generated SDK:

- **Python**: `python/lib/`
- **TypeScript**: `typescript/lib/`
- **C#**: `csharp/src/TurquoiseHealth.Api/Lib/`

These directories are protected from deletion during Fern regeneration via `.fernignore` files.

## Directory Structure

```shell
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

scripts/
└── _lib_exports.py   # Auto-updates exports after Fern generation

examples/
├── python_custom_libs.py    # Python usage examples
├── typescript_custom_libs.ts # TypeScript usage examples
└── csharp_custom_libs.cs    # C# usage examples
```

## Testing Custom Library Imports

After adding a custom library, verify that end users can import it correctly:

**Python:**

```python
from turquoise_health import TurquoiseHealth, APIAuthHandler

auth = APIAuthHandler.from_env()
client = TurquoiseHealth(token=auth.get_token())
```

```typescript
import { TurquoiseHealthApiClient, lib } from "@turquoisehealth/api";

const auth = lib.APIAuthHandler.fromEnv();
const client = new TurquoiseHealthApiClient({ token: auth.asSupplier() });
```

**C#:**

```csharp
using TurquoiseHealth.Api.Lib;

var auth = APIAuthHandler.FromEnv();
var client = new TurquoiseHealthApiClient(token: auth.GetToken());
```

## Adding New Libraries

### Python

1. **Create your library file** in `python/lib/`:

   ```bash
   # Example: python/lib/rate_calculator.py
   ```

2. **Export it** from `python/lib/__init__.py`:

   ```python
   from .rate_calculator import RateCalculator

   __all__ = [
       "APIAuthHandler",
       "RateCalculator",  # Add your new library
   ]
   ```

3. **Update SDK exports** (automated):

   ```bash
   ./scripts/generate.sh
   ```

4. **Test the import**:

   ```python
   from turquoise_health import RateCalculator
   # or
   from turquoise_health.lib import RateCalculator
   ```

### TypeScript

1. **Create your library file** in `typescript/lib/`:

   ```bash
   # Example: typescript/lib/RateCalculator.ts
   ```

2. **Export it** from `typescript/lib/index.ts`:

   ```typescript
   export { APIAuthHandler } from "./APIAuthHandler.js";
   export { RateCalculator } from "./RateCalculator.js";  # Add your new library
   ```

3. **SDK exports are automatic** via `typescript/index.ts`:

   ```typescript
   export * as lib from "./lib/index.js";
   ```

4. **Test the import**:

   ```typescript
   import { lib } from "@turquoisehealth/api";
   const calculator = new lib.RateCalculator();
   ```

### C\#

1. **Create your library file** in `csharp/src/TurquoiseHealth.Api/Lib/`:

   ```bash
   # Example: csharp/src/TurquoiseHealth.Api/Lib/RateCalculator.cs
   ```

2. **Use the correct namespace**:

   ```csharp
   namespace TurquoiseHealth.Api.Lib;

   public class RateCalculator
   {
       // Your implementation
   }
   ```

3. **Exports are automatic** via the namespace.

4. **Test the import**:

   ```csharp
   using TurquoiseHealth.Api.Lib;

   var calculator = new RateCalculator();
   ```

## After Fern Regeneration

When you run `fern generate`, the export files may be overwritten. Use the automated script:

### Automated (Recommended)

```bash
./scripts/generate.sh
```

### Manual Updates

**Python** - Update `python/__init__.py`:

- Add each new library to `_dynamic_imports` dictionary:

  ```python
  "YourLibrary": ".lib",
  ```

- Add each new library to `__all__` list:

  ```python
  "YourLibrary",
  ```

- Ensure `"lib": ".lib"` is in `_dynamic_imports`
- Ensure `"lib"` is in `__all__`

**TypeScript** - Update `typescript/index.ts`:

- Add at the end of the file:

  ```typescript
  export * as lib from "./lib/index.js";
  ```

**C#** - No action needed (namespace automatically included)

## Developer Checklist

### Adding a New Custom Library

#### Python

- [ ] Create file in `python/lib/your_library.py`
- [ ] Add to exports in `python/lib/__init__.py`:

  ```python
  from .your_library import YourLibrary
  __all__ = [..., "YourLibrary"]
  ```

- [ ] Run `./scripts/generate.sh`
- [ ] Test import: `python3 -c "from turquoise_health import YourLibrary"`
- [ ] Add examples to `examples/python_custom_libs.py`

#### TypeScript

- [ ] Create file in `typescript/lib/YourLibrary.ts`
- [ ] Add to exports in `typescript/lib/index.ts`:

  ```typescript
  export { YourLibrary } from "./YourLibrary.js";
  ```

- [ ] Verify `typescript/index.ts` has `export * as lib from "./lib/index.js";`
- [ ] Test import: `node -e "const { lib } = require('./typescript'); console.log(lib.YourLibrary)"`
- [ ] Add examples to `examples/typescript_custom_libs.ts`

#### C\#

- [ ] Create file in `csharp/src/TurquoiseHealth.Api/Lib/YourLibrary.cs`
- [ ] Use namespace `TurquoiseHealth.Api.Lib`
- [ ] Add XML documentation comments
- [ ] Test compile: `cd csharp && dotnet build`
- [ ] Add examples to `examples/csharp_custom_libs.cs`

### After Running `fern generate`

- [ ] Run `./scripts/generate.sh`
- [ ] Verify Python exports: `cd python && python3 -c "import __init__; print('lib' in __init__.__all__)"`
- [ ] Verify TypeScript exports: `grep -q "lib" typescript/index.ts && echo "OK"`
- [ ] Test all imports still work
- [ ] Commit updated files

### Before Publishing

- [ ] Run `./scripts/generate.sh`
- [ ] Test all imports work across all languages
- [ ] Run example files
- [ ] Update CHANGELOG if custom libraries changed
- [ ] Verify CI/CD workflows pass

## Best Practices

1. **Keep lib files focused** - One class/module per file
2. **Document thoroughly** - Add docstrings/comments and examples
3. **Follow language conventions** - Match the style of generated code
4. **Test your libraries** - Add tests to ensure they work correctly
5. **Version carefully** - Manual libraries are part of the public API

### Library Templates

**Python:**

```python
"""
MyLibrary - Brief description

Example usage:
    from turquoise_health import MyLibrary
    lib = MyLibrary()
"""

class MyLibrary:
    """Your library implementation."""

    def __init__(self):
        pass
```

**TypeScript:**

```typescript
/**
 * MyLibrary - Brief description
 *
 * @example
 * ```typescript
 * import { lib } from "@turquoisehealth/api";
 * const myLib = new lib.MyLibrary();
 * ```
 */
export class MyLibrary {
    constructor() {}
}
```

**C#:**

```csharp
using System;

namespace TurquoiseHealth.Api.Lib;

/// <summary>
/// MyLibrary - Brief description
/// </summary>
/// <example>
/// <code>
/// using TurquoiseHealth.Api.Lib;
/// var myLib = new MyLibrary();
/// </code>
/// </example>
public class MyLibrary
{
    public MyLibrary() {}
}
```

## Troubleshooting

**Import not working after adding a new library?**

- Run `./scripts/generate.sh`
- Check that the file is in the correct `lib/` directory
- Verify the export in `lib/__init__.py` or `lib/index.ts`

**Fern deleted my lib directory?**

- Check that `.fernignore` exists and contains `lib/` pattern
- Restore from git: `git checkout -- python/lib typescript/lib csharp/src/TurquoiseHealth.Api/Lib`

**Can't import from package root?**

- Python: Check `_dynamic_imports` and `__all__` in `python/__init__.py`
- TypeScript: Check `export * as lib` in `typescript/index.ts`
- C#: Check the namespace is `TurquoiseHealth.Api.Lib`

**Export update not working?**

- Verify `scripts/_lib_exports.py` exists
- Check Python version: `python3 --version` (requires 3.6+)
- Run `./scripts/generate.sh` so Fern generation and export updates stay together

## CI/CD Integration

The local generation script runs the update script automatically:

- `scripts/generate.sh` - Updates exports during SDK regeneration

### Local Development Workflow

After running `./scripts/generate.sh`:

```bash
./scripts/generate.sh
```

### Testing Your Custom Library Imports

```bash
# Python
python3 -c "from turquoise_health import APIAuthHandler; print('✓ Import successful')"

# TypeScript
node -e "const { lib } = require('./typescript'); console.log('✓ Import successful')"

# C#
cd csharp && dotnet build
```

## File Protection

The `.fernignore` files protect your `lib/` directories from being deleted:

- `python/.fernignore`
- `typescript/.fernignore`
- `csharp/.fernignore`

**Note:** The `lib/` directories are protected by `.fernignore` but the main export files (`__init__.py`, `index.ts`) are regenerated by Fern. Always use `./scripts/generate.sh` so export updates are applied afterward.
