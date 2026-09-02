# SDK Release Process

## 1. Update openapi.json

Make sure that the `/openapi.json` file in the project root reflects the version you want to release. You can run the following to pull the latest:

```bash
python scripts/pull_openapi_json.py https://turquoise.health/api/docs/openapi.json
```

## 2. Create a release branch

```bash
git switch main
git pull --ff-only origin main
git switch -c chore/release-X.X.XX
```

Of course, replace `X.X.XX` with the value from the `info.version` field in `/openapi.json`.

## 3. Regenerate the SDKs

Set a `FERN_TOKEN` in `.env` if not already set, and then run the generation script from the repository root:

```bash
python scripts/regenerate_sdks.py
```

This script will run Fern to build the SDKs from `/openapi.json`, update the manual library exports, and synchronize the package versions.

Review the changes in:

- `python/`
- `typescript/`
- `csharp/`

Generated SDK source files are committed intentionally because the publish workflow in GitHub builds the files checked into the release tag.

## 4. Run Tests

Set OAuth client credentials in `.env` if not already set to allow the tests to run fully.

- `TURQUOISE_CLIENT_ID`
- `TURQUOISE_CLIENT_SECRET`
- `TURQUOISE_ORGANIZATION_ID`

Then you can run all the test suites with the following make target:

```bash
make tests
```

Review the output and address as necessary.

## 5. Commit and open the release PR

```bash
git add openapi.json python/ typescript/ csharp/
git status
git diff --cached --stat
git commit -m "chore: release SDKs for X.X.XX"
```

Open a pull request targeting `main`. Include the API version, validation results, and a summary of any breaking changes. Also verify that the semantic versioning correctly reflects the changes being published (major, minor, patch etc.)

The release PR should be carefully reviewed before merging. The merged commit contains the exact SDK source and package metadata that will be published.

## 6. Create and push the release tag

After the PR is merged, pull `main` and create an annotated tag:

```bash
git switch main
git pull --ff-only origin main
git tag -a vX.X.XX -m "Release vX.X.XX"
git push origin vX.X.XX
git switch --detach vX.X.XX
```

## 7. Validate and trigger the publish workflow

From the clean, tagged release checkout, run:

```bash
python3 scripts/trigger_publish_workflow.py --sdk all
```

The script uses the version in the checked-out `openapi.json` and derives the tag from it. It validates that:

- The working tree is clean.
- `HEAD` is detached at the matching `vX.X.XX` tag.
- The package versions match `openapi.json`.
- GitHub CLI is installed and authenticated.

Use `--sdk python`, `--sdk typescript`, or `--sdk csharp` to publish only that SDK. You can also use `--dry` to validate without triggering GitHub Actions, but even without the --dry option the script will still ask for confirmation after validation and before triggering the workflow in Github.

The GitHub Actions workflow receives the tag ref, version, and SDK selection:

- The `vX.X.XX` tag as the workflow ref
- `X.X.XX` as the version
- `all`, or one specific SDK

If validation passes, it builds and publishes to:

- PyPI for Python
- npm with the `latest` tag for TypeScript
- NuGet for C#

Note that the repositories can take up to a few hours to show the latest version even after a successful publish.

## Release Checklist

- [ ] Confirmed `info.version` in the upstream OpenAPI document
- [ ] Created a release branch from current `main`
- [ ] Regenerated `openapi.json` and all SDKs locally
- [ ] Ran `make tests` successfully
- [ ] Reviewed and committed the expected release files
- [ ] Merged the release PR
- [ ] Created and pushed the matching `v*` tag
- [ ] Successfully ran the publish workflow against that tag
- [ ] Verified the selected packages in their registries
