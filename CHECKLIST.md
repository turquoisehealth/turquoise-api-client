# Custom Libraries - Developer Checklist

This checklist helps you work with the custom libraries system.

## ✅ Adding a New Custom Library

### Python

- [ ] Create file in `python/lib/your_library.py`
- [ ] Add to exports in `python/lib/__init__.py`:
  ```python
  from .your_library import YourLibrary
  __all__ = [..., "YourLibrary"]
  ```
- [ ] Run `make update-lib-exports` to add to `python/__init__.py`
- [ ] Test import: `python3 -c "from turquoise_health import YourLibrary"`
- [ ] Add examples to `examples/python_custom_libs.py`
- [ ] Update documentation if needed

### TypeScript

- [ ] Create file in `typescript/lib/YourLibrary.ts`
- [ ] Add to exports in `typescript/lib/index.ts`:
  ```typescript
  export { YourLibrary } from "./YourLibrary.js";
  ```
- [ ] Verify `typescript/index.ts` has `export * as lib from "./lib/index.js";`
- [ ] Test import: `node -e "const { lib } = require('./typescript'); console.log(lib.YourLibrary)"`
- [ ] Add examples to `examples/typescript_custom_libs.ts`
- [ ] Update documentation if needed

### C#

- [ ] Create file in `csharp/src/TurquoiseHealth.Api/Lib/YourLibrary.cs`
- [ ] Use namespace `TurquoiseHealth.Api.Lib`
- [ ] Add XML documentation comments
- [ ] Test compile: `cd csharp && dotnet build`
- [ ] Add examples to `examples/csharp_custom_libs.cs`
- [ ] Update documentation if needed

## ✅ After Running `fern generate`

- [ ] Run `make update-lib-exports`
- [ ] Verify Python exports:
  ```bash
  cd python && python3 -c "import __init__; print('lib' in __init__.__all__)"
  ```
- [ ] Verify TypeScript exports:
  ```bash
  grep -q "lib" typescript/index.ts && echo "OK"
  ```
- [ ] Test imports still work
- [ ] Commit updated files

## ✅ Before Publishing

- [ ] Run `make update-lib-exports`
- [ ] Test all imports work:
  - [ ] Python: `from turquoise_health import APIAuthHandler`
  - [ ] TypeScript: `import { lib } from "@turquoise-health/api"`
  - [ ] C#: `using TurquoiseHealth.Api.Lib;`
- [ ] Run example files:
  - [ ] `python3 examples/python_custom_libs.py`
  - [ ] `node examples/typescript_custom_libs.ts` (or compile first)
  - [ ] `dotnet run examples/csharp_custom_libs.cs`
- [ ] Update CHANGELOG if custom libraries changed
- [ ] Verify CI/CD workflows pass

## ✅ Verifying Setup

- [ ] `.fernignore` files exist in:
  - [ ] `python/.fernignore`
  - [ ] `typescript/.fernignore`
  - [ ] `csharp/.fernignore`
- [ ] GitHub workflows updated:
  - [ ] `.github/workflows/generate.yml` has "Update library exports" step
  - [ ] `.github/workflows/publish.yml` has "Update library exports" step
- [ ] Makefile has `update-lib-exports` target
- [ ] Update script exists: `scripts/update_lib_exports.py`

## ✅ Documentation

- [ ] README.md has custom libraries section
- [ ] MANUAL_LIBRARIES.md is up to date
- [ ] QUICKSTART_CUSTOM_LIBS.md is up to date
- [ ] Examples in `examples/` directory work
- [ ] Inline code comments are clear

## ✅ Testing

### Local Testing

```bash
# Test update script
make update-lib-exports

# Test Python
cd python
python3 -c "from __init__ import APIAuthHandler; print('OK')"

# Test TypeScript
cd typescript
node -e "const { lib } = require('./index.js'); console.log('OK')"

# Test C#
cd csharp
dotnet build src/TurquoiseHealth.Api/TurquoiseHealth.Api.csproj
```

### Package Testing (Pre-publish)

```bash
# Python - build and install locally
cd python
python3 -m build
pip install dist/*.whl
python3 -c "from turquoise_health import APIAuthHandler; print('OK')"

# TypeScript - pack and install locally
cd typescript
npm pack
npm install turquoise-health-api-*.tgz
node -e "const { lib } = require('@turquoise-health/api'); console.log('OK')"

# C# - pack and test locally
cd csharp
dotnet pack
dotnet add package TurquoiseHealth.Api --source ./bin/Debug
```

## 🚨 Common Issues

### Import not working after adding library

**Solution:**
- Run `make update-lib-exports`
- Check file is in correct `lib/` directory
- Verify export in `lib/__init__.py` or `lib/index.ts`

### Fern deleted my lib directory

**Solution:**
- Verify `.fernignore` exists with `lib/` pattern
- Restore from git: `git checkout python/lib typescript/lib csharp/src/TurquoiseHealth.Api/Lib`

### CI/CD workflow fails after regeneration

**Solution:**
- Check "Update library exports" step exists in workflows
- Verify `scripts/update_lib_exports.py` is committed
- Test script locally: `python3 scripts/update_lib_exports.py`

### Package users can't import custom libraries

**Solution:**
- Verify exports are in `__init__.py` / `index.ts`
- Check `.fernignore` protected the lib directories
- Rebuild and republish package

## 📋 Quick Commands

```bash
# Add new library and update exports
make update-lib-exports

# Test everything
make test

# Clean and rebuild
make clean
make update-lib-exports

# Verify Python package structure
python3 -c "import sys; sys.path.insert(0, 'python'); import __init__; print(__init__.__all__)"

# Verify TypeScript exports
cat typescript/index.ts | grep lib

# Run update script manually
python3 scripts/update_lib_exports.py
```

## 📚 Reference Documents

- `MANUAL_LIBRARIES.md` - Complete guide
- `QUICKSTART_CUSTOM_LIBS.md` - Quick reference
- `SETUP_SUMMARY.md` - What was created
- `lib/README.md` - System overview
- `examples/` - Working code examples

---

**Keep this checklist handy when working with custom libraries!**
