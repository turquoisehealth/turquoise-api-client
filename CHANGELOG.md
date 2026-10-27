# Changelog

Notable API-level changes to this SDK are summarized in this file. More granular, auto-generated diffs can be found in `oasdiffs/X.X.XX.md`

## [3.2.114] - 2026-10-27

**Fixes:** No standalone fixes are called out in the API diff.

**New functionality:** Adds statistics endpoints for personalized estimates and prices, supports `ssp` and `ssp_grouper` package types (defaulting to `ssp_grouper`), and expands personalized-estimate response details and error codes.

**Potentially breaking changes:** Removes the personalized-estimate and price comparison endpoints without deprecation. The personalized-estimates contract is also tightened: `consent_attested` must be a boolean, and several response fields are now required.
