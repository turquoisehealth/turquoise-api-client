# Quick Start: Adding Custom Libraries

This is a quick reference for adding new manually-coded libraries to the SDK packages.

## Adding a New Python Library

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

3. **Update SDK exports** in `python/__init__.py`:
   - Add to `_dynamic_imports`:
     ```python
     "RateCalculator": ".lib",
     ```
   - Add to `__all__`:
     ```python
     "RateCalculator",
     ```

4. **Or run the auto-update script**:
   ```bash
   make update-lib-exports
   ```

5. **Test the import**:
   ```python
   from turquoise_health import RateCalculator
   # or
   from turquoise_health.lib import RateCalculator
   ```

## Adding a New TypeScript Library

1. **Create your library file** in `typescript/lib/`:
   ```bash
   # Example: typescript/lib/RateCalculator.ts
   ```

2. **Export it** from `typescript/lib/index.ts`:
   ```typescript
   export { APIAuthHandler } from "./APIAuthHandler.js";
   export { RateCalculator } from "./RateCalculator.js";  // Add your new library
   ```

3. **SDK exports are automatic** via `typescript/index.ts`:
   ```typescript
   export * as lib from "./lib/index.js";
   ```

4. **Test the import**:
   ```typescript
   import { lib } from "@turquoise-health/api";
   const calculator = new lib.RateCalculator();
   ```

## Adding a New C# Library

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

Fern may overwrite the export files. After running `fern generate`:

### Automated (Recommended)
```bash
make update-lib-exports
```

### Manual

**Python** - Update `python/__init__.py`:
- Add each new library to `_dynamic_imports`
- Add each new library to `__all__`
- Add `"lib": ".lib"` if not present

**TypeScript** - Update `typescript/index.ts`:
- Add `export * as lib from "./lib/index.js";` if not present

**C#** - No action needed

## File Protection

The `.fernignore` files protect your `lib/` directories from being deleted:
- `python/.fernignore`
- `typescript/.fernignore`
- `csharp/.fernignore`

## Best Practices

1. **Keep lib files focused** - One class/module per file
2. **Document thoroughly** - Add docstrings and examples
3. **Follow language conventions** - Match the style of generated code
4. **Test your libraries** - Add tests to ensure they work correctly
5. **Version carefully** - Manual libraries are part of the public API

## Example Library Template

### Python
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

### TypeScript
```typescript
/**
 * MyLibrary - Brief description
 *
 * @example
 * ```typescript
 * import { lib } from "@turquoise-health/api";
 * const myLib = new lib.MyLibrary();
 * ```
 */
export class MyLibrary {
    constructor() {}
}
```

### C#
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
- Run `make update-lib-exports`
- Check that the file is in the correct `lib/` directory
- Verify the export in `lib/__init__.py` or `lib/index.ts`

**Fern deleted my lib directory?**
- Check that `.fernignore` exists and contains `lib/` pattern
- Restore from git: `git checkout -- python/lib typescript/lib csharp/src/TurquoiseHealth.Api/Lib`

**Can't import from package root?**
- Check `_dynamic_imports` and `__all__` in `python/__init__.py`
- Check `export * as lib` in `typescript/index.ts`
- For C#, check the namespace is `TurquoiseHealth.Api.Lib`
