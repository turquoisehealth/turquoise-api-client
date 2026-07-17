#!/usr/bin/env python3
"""
Post-generation script to add manual library exports to generated SDK files.

This script updates the auto-generated export files to include the manual `lib/` modules.
Run this after executing `fern generate`.

Usage:
    python scripts/update_lib_exports.py
"""

import re
from pathlib import Path


def update_python_init():
    """Update python/__init__.py to include lib exports."""
    init_file = Path("python/__init__.py")

    if not init_file.exists():
        print(f"⚠️  {init_file} not found, skipping Python updates")
        return

    content = init_file.read_text()
    modified = False

    # Add to _dynamic_imports if not present
    if '"APIAuthHandler": ".lib"' not in content:
        content = re.sub(
            r'(_dynamic_imports: typing\.Dict\[str, str\] = \{)',
            r'\1\n    "APIAuthHandler": ".lib",',
            content,
            count=1
        )
        modified = True
        print("✓ Added APIAuthHandler to Python _dynamic_imports")

    if '"lib": ".lib"' not in content:
        content = re.sub(
            r'("consumer_pricing": "\.consumer_pricing",)',
            r'\1\n    "lib": ".lib",',
            content,
            count=1
        )
        modified = True
        print("✓ Added lib module to Python _dynamic_imports")

    # Add to __all__ if not present
    if '"APIAuthHandler"' not in content or (
        content.find('"APIAuthHandler"') > content.find('__all__ = [')
    ):
        content = re.sub(
            r'(__all__ = \[)',
            r'\1\n    "APIAuthHandler",',
            content,
            count=1
        )
        modified = True
        print("✓ Added APIAuthHandler to Python __all__")

    if '"lib"' not in re.search(r'__all__ = \[(.*?)\]', content, re.DOTALL).group(1):
        content = re.sub(
            r'("consumer_pricing",\n\])',
            r'"consumer_pricing",\n    "lib",\n]',
            content,
            count=1
        )
        modified = True
        print("✓ Added lib to Python __all__")

    if modified:
        init_file.write_text(content)
        print(f"✅ Updated {init_file}")
    else:
        print(f"✓ {init_file} already up to date")


def update_typescript_index():
    """Update typescript/index.ts to include lib exports."""
    index_file = Path("typescript/index.ts")

    if not index_file.exists():
        print(f"⚠️  {index_file} not found, skipping TypeScript updates")
        return

    content = index_file.read_text()

    if 'from "./lib/index.js"' not in content:
        # Add lib export at the end
        content = content.rstrip() + '\nexport * as lib from "./lib/index.js";\n'
        index_file.write_text(content)
        print(f"✅ Updated {index_file}")
    else:
        print(f"✓ {index_file} already up to date")


def main():
    print("🔧 Updating SDK exports to include manual libraries...\n")

    update_python_init()
    print()
    update_typescript_index()
    print()

    print("✨ Done! Manual library exports have been updated.")
    print("\nC# libraries are automatically included via the namespace.")


if __name__ == "__main__":
    main()
