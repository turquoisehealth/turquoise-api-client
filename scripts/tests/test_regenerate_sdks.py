import tempfile
import unittest
from pathlib import Path
from unittest.mock import patch

from scripts import regenerate_sdks


GENERATED_AUTH = {
    "python/client.py": (
        "token : typing.Optional[typing.Union[str, typing.Callable[[], str]]]\n"
        "token: typing.Optional[typing.Union[str, typing.Callable[[], str]]] = None\n"
        'client = TurquoiseHealth(token="YOUR_TOKEN", )\n'
    ) * 2,
    "python/consumer_pricing/client.py": 'client = TurquoiseHealth(token="YOUR_TOKEN", )\n',
    "python/core/client_wrapper.py": (
        "        token: typing.Optional[typing.Union[str, typing.Callable[[], str]]] = None,\n" * 3
        + "        self._token = token\n"
        "        if isinstance(self._token, str) or self._token is None:\n"
        "            return self._token\n"
        "        else:\n"
        "            return self._token()\n"
    ),
    "typescript/BaseClient.ts": "    token?: core.Supplier<core.BearerToken | undefined>;\n",
    "typescript/auth/BearerAuthProvider.ts": (
        "        token?: core.Supplier<core.BearerToken | undefined>;\n"
        "    private readonly token: core.Supplier<core.BearerToken | undefined>;\n"
        "        this.token = options.token;\n"
        "        const token = await core.Supplier.get(this.token);\n"
    ),
    "typescript/core/auth/BearerToken.ts": "export type BearerToken = string;\n",
    "typescript/core/auth/index.ts": 'export { BearerToken } from "./BearerToken.js";\n',
    "csharp/src/TurquoiseHealth.Api/TurquoiseHealthApiClient.cs": (
        "    public TurquoiseHealthApiClient(string token, ClientOptions? clientOptions = null)\n"
        "    {\n"
        "        var defaultHeaders = new Headers(\n"
        "            new Dictionary<string, string>()\n"
        "            {\n"
        '                { "Authorization", $"Bearer {token}" },\n'
        '                { "X-Fern-Language", "C#" },\n'
        '                { "X-Fern-SDK-Name", "TurquoiseHealth.Api" },\n'
        '                { "X-Fern-SDK-Version", Version.Current },\n'
        "            }\n"
        "        );\n"
        "        clientOptions ??= new ClientOptions();\n"
    ),
}


class GeneratedAuthPatchTests(unittest.TestCase):
    def setUp(self):
        self.directory = tempfile.TemporaryDirectory()
        self.addCleanup(self.directory.cleanup)
        self.root = Path(self.directory.name)
        root_patch = patch.object(regenerate_sdks, "REPOSITORY_ROOT", self.root)
        root_patch.start()
        self.addCleanup(root_patch.stop)
        for relative_path, content in GENERATED_AUTH.items():
            path = self.root / relative_path
            path.parent.mkdir(parents=True, exist_ok=True)
            path.write_text(content)

    def contents(self):
        return {relative_path: (self.root / relative_path).read_text() for relative_path in GENERATED_AUTH}

    def test_patches_all_generated_auth_and_is_idempotent(self):
        regenerate_sdks.patch_generated_auth()
        first = self.contents()
        self.assertNotEqual(first, GENERATED_AUTH)
        for relative_path, content in first.items():
            with self.subTest(path=relative_path):
                self.assertNotEqual(content, GENERATED_AUTH[relative_path])
        self.assertNotIn("typing.Union[str,", first["python/client.py"])
        self.assertEqual(first["python/client.py"].count('token=lambda: "YOUR_TOKEN"'), 2)
        self.assertIn('token=lambda: "YOUR_TOKEN"', first["python/consumer_pricing/client.py"])
        wrapper = first["python/core/client_wrapper.py"]
        self.assertEqual(wrapper.count("token: typing.Optional[typing.Callable[[], str]]"), 3)
        self.assertIn("if token is not None and not callable(token):", wrapper)
        self.assertIn("        return self._token()\n", wrapper)
        self.assertNotIn("isinstance(self._token, str)", wrapper)
        self.assertIn("token?: core.BearerTokenSupplier;", first["typescript/BaseClient.ts"])
        provider = first["typescript/auth/BearerAuthProvider.ts"]
        self.assertIn("private readonly token?: core.BearerTokenSupplier;", provider)
        self.assertIn('typeof options.token !== "function"', provider)
        self.assertIn("this.token == null ? undefined : await this.token()", provider)
        self.assertIn("export type BearerTokenSupplier = () =>", first["typescript/core/auth/BearerToken.ts"])
        self.assertIn("export type { BearerTokenSupplier }", first["typescript/core/auth/index.ts"])
        csharp = first["csharp/src/TurquoiseHealth.Api/TurquoiseHealthApiClient.cs"]
        self.assertIn("Func<string> tokenSupplier", csharp)
        self.assertIn('defaultHeaders["Authorization"] = new Func<string>(() => $"Bearer {tokenSupplier()}");', csharp)
        self.assertNotIn('{ "Authorization",', csharp)
        regenerate_sdks.patch_generated_auth()
        self.assertEqual(self.contents(), first)

    def test_missing_expected_text_in_each_file_fails_before_writing(self):
        for relative_path in GENERATED_AUTH:
            with self.subTest(path=relative_path):
                path = self.root / relative_path
                path.write_text("// unexpected generator output\n")
                before = self.contents()
                with self.assertRaisesRegex(ValueError, "Could not patch supplier-only auth") as error:
                    regenerate_sdks.patch_generated_auth()
                self.assertIn(str(path), str(error.exception))
                self.assertEqual(self.contents(), before)
                path.write_text(GENERATED_AUTH[relative_path])

    def test_missing_file_fails_before_writing(self):
        path = self.root / "typescript/core/auth/index.ts"
        path.unlink()
        with self.assertRaises(FileNotFoundError):
            regenerate_sdks.patch_generated_auth()
        self.assertEqual((self.root / "python/client.py").read_text(), GENERATED_AUTH["python/client.py"])

    def test_duplicate_expected_text_fails(self):
        path = self.root / "typescript/BaseClient.ts"
        path.write_text(GENERATED_AUTH["typescript/BaseClient.ts"] * 2)
        with self.assertRaisesRegex(ValueError, "found 2"):
            regenerate_sdks.patch_generated_auth()

    def test_changed_csharp_auth_header_fails(self):
        path = self.root / "csharp/src/TurquoiseHealth.Api/TurquoiseHealthApiClient.cs"
        path.write_text(path.read_text().replace("{token}", "{token.Trim()}"))
        with self.assertRaisesRegex(ValueError, "Could not patch supplier-only auth"):
            regenerate_sdks.patch_generated_auth()

    def test_individual_patches_fail_on_drift(self):
        for original, patched in [
            ("old", "new"),
            ("anchor", "guard\nanchor"),
            ("remove\nanchor", "anchor"),
        ]:
            with self.subTest(original=original):
                for content in [original, patched]:
                    self.assertEqual(
                        regenerate_sdks.patch_generated_text(content, original, patched, path=Path("client")),
                        patched,
                    )
                with self.assertRaisesRegex(ValueError, "found 0"):
                    regenerate_sdks.patch_generated_text("unexpected", original, patched, path=Path("client"))

    def test_main_patches_after_fern_and_before_exports_and_versions(self):
        calls = []
        with (
            patch.object(regenerate_sdks, "load_environment", side_effect=lambda: calls.append("env")),
            patch.object(regenerate_sdks, "run_fern", side_effect=lambda: calls.append("fern")),
            patch.object(regenerate_sdks, "patch_generated_auth", side_effect=lambda: calls.append("auth")),
            patch.object(regenerate_sdks, "update_library_exports", side_effect=lambda: calls.append("exports")),
            patch.object(regenerate_sdks, "update_package_versions", side_effect=lambda: calls.append("versions")),
        ):
            regenerate_sdks.main()
        self.assertEqual(calls, ["env", "fern", "auth", "exports", "versions"])

    def test_main_stops_if_auth_patching_fails(self):
        with (
            patch.object(regenerate_sdks, "load_environment"),
            patch.object(regenerate_sdks, "run_fern"),
            patch.object(regenerate_sdks, "patch_generated_auth", side_effect=ValueError("drift")),
            patch.object(regenerate_sdks, "update_library_exports") as exports,
            patch.object(regenerate_sdks, "update_package_versions") as versions,
        ):
            with self.assertRaisesRegex(ValueError, "drift"):
                regenerate_sdks.main()
        exports.assert_not_called()
        versions.assert_not_called()


if __name__ == "__main__":
    unittest.main()
