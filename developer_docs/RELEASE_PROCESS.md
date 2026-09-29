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

## 4. Update the changelog

Generate the `CHANGELOG.md` entry for this release from the OpenAPI diff against the previous release tag:

```bash
git fetch --tags origin
python scripts/generate_changelog.py
```

This requires the [oasdiff](https://github.com/oasdiff/oasdiff) CLI:

```bash
curl -fsSL https://raw.githubusercontent.com/oasdiff/oasdiff/main/install.sh | sh
```

The script writes the raw oasdiff output to `oasdiffs/X.X.XX.md` and puts an explicit review placeholder in `CHANGELOG.md`. Open that report, identify the meaningful customer impact, and replace the placeholder with a concise summary organized by Added, Changed, Deprecated, and Breaking Changes as applicable. Keep the raw report in `oasdiffs/` for review history, but do not copy it into the changelog: it may contain repeated low-level schema details and is generally not publish-ready. The summary in `CHANGELOG.md` is the same text that will be posted to the GitHub Release in step 8, and it's what customers see when deciding whether to upgrade. The PR opened in step 6 also gets an automated breaking-change check and changelog preview (see `.github/workflows/openapi-diff.yml`); use those to double check the semantic version bump below is correct.

The GitHub Release script refuses to publish while the review placeholder remains. Treat that failure as a reminder to finish and review the customer-facing summary before continuing.

For every release after the repository bootstrap, the script compares against the latest available `v*` release tag. It fails if that tag is missing or cannot be resolved; fetch the tags and retry rather than treating the condition as an initial release. Use `--initial-release` only when deliberately bootstrapping the repository's first release:

```bash
python scripts/generate_changelog.py --initial-release
```

## 5. Run Tests

Set OAuth client credentials in `.env` if not already set to allow the tests to run fully.

- `TURQUOISE_CLIENT_ID`
- `TURQUOISE_CLIENT_SECRET`
- `TURQUOISE_ORGANIZATION_ID`

Then you can run all the test suites with the following make target:

```bash
make tests
```

Review the output and address as necessary.

## 6. Commit and open the release PR

```bash
git add openapi.json python/ typescript/ csharp/ CHANGELOG.md oasdiffs/
git status
git diff --cached --stat
git commit -m "chore: release SDKs for X.X.XX"
```

Open a pull request targeting `main`. Include the API version, validation results, and a summary of any breaking changes. Also verify that the semantic versioning correctly reflects the changes being published (major, minor, patch etc.)

The release PR should be carefully reviewed before merging. The merged commit contains the exact SDK source and package metadata that will be published.

## 7. Create and push the release tag

After the PR is merged, pull `main` and create an annotated tag:

```bash
git switch main
git pull --ff-only origin main
git tag -a vX.X.XX -m "Release vX.X.XX"
git push origin vX.X.XX
git switch --detach vX.X.XX
```

## 8. Validate and trigger the publish workflow

From the clean, tagged release checkout, run:

```bash
python scripts/trigger_publish_workflow.py --sdk all
```

The script uses the version in the checked-out `openapi.json` and derives the tag from it. It validates that:

- The working tree is clean.
- `HEAD` is detached at the matching `vX.X.XX` tag.
- The package versions match `openapi.json`.
- GitHub CLI is installed and authenticated.

Use `--sdk python`, `--sdk typescript`, or `--sdk csharp` to publish only that SDK. You can also use `--dry` to validate without triggering GitHub Actions, but even without the --dry option the script will still ask for confirmation after validation and before triggering the workflow in Github.

### Retrying a failed SDK publish

Release tags are immutable, but a failed package publish can be retried from a corrective commit without creating a new version. Push the fix to a branch or commit, check out the original release tag, and provide the corrective source with `--source-ref`:

```bash
git switch --detach vX.X.XX
python scripts/trigger_publish_workflow.py \
	--sdk csharp \
	--source-ref chore/fix-csharp-package
```

The script derives `X.X.XX` from the checked-out `openapi.json`, validates that the release tag `vX.X.XX` exists, and triggers the workflow from `source_ref` so the retry inputs are available. The workflow publishes version `X.X.XX` while building the selected SDK from `source_ref`. If the workflow definition must come from another ref, provide it with `--workflow-ref <ref>`. Use this only when the package version has not already been accepted by the registry. Published package versions cannot be overwritten.

The GitHub Actions workflow receives the release tag, version, SDK selection, and optional build source:

- The `vX.X.XX` release tag as the release version reference
- `X.X.XX` as the version
- `all`, or one specific SDK
- The optional `source_ref` for a corrective build

If validation passes, it builds and publishes to:

- PyPI for Python
- npm with the `latest` tag for TypeScript
- NuGet for C#

After the packages are successfully published and verified, create the GitHub Release for the tag. This combines the `CHANGELOG.md` entry from step 4 with GitHub's auto-generated list of merged PRs (categorized per `.github/release.yml`):

```bash
python scripts/create_github_release.py
```

Only after the package versions are live should the repository notification be sent. Customers watching the repository or its releases feed are notified automatically; this is the primary place they should look to see what changed before upgrading.

## Release Checklist

- [ ] Confirmed `info.version` in the upstream OpenAPI document
- [ ] Created a release branch from current `main`
- [ ] Regenerated `openapi.json` and all SDKs locally
- [ ] Generated the `CHANGELOG.md` entry, reviewed the raw oasdiff report, and replaced the review placeholder with a customer-facing summary
- [ ] Ran `make tests` successfully
- [ ] Reviewed and committed the expected release files
- [ ] Merged the release PR
- [ ] Created and pushed the matching `v*` tag
- [ ] Successfully ran the publish workflow against that tag
- [ ] Verified the selected packages in their registries
- [ ] Created the GitHub Release for that tag
