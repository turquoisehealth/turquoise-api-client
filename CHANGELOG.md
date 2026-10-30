# Changelog

Notable API-level changes to this SDK are summarized in this file. More granular, auto-generated diffs can be found in `oasdiffs/X.X.XX.md`

## [3.2.114] - 2026-10-27

See `oasdiffs/3.2.114.md` for a full generated diff.

**Fixes (SDK)**
- C#: added token-management support, and an empty token string is no longer accepted.
- TypeScript: an expired token is no longer cached for up to an hour.

**New functionality**
- `POST /v3/prices/statistics` and `POST /v3/personalized-estimates/statistics` replace the removed `/compare` endpoints (see below).
- Package `type` now supports `ssp_grouper` alongside `ssp`. `ssp_grouper` is the default and the main package type.
- Personalized-estimate providers now include `npi`, `address`, `context` and `object`. Money values now include `currency`.

**Potentially breaking changes**
- **Removed** `POST /v3/prices/compare` and `POST /v3/personalized-estimates/compare`. Migrate to the matching `/statistics` endpoint with the same request filters.
- **Package `type`:** now an enum (`ssp`, `ssp_grouper`) instead of the constant `ssp`. When `type` is omitted it now defaults to `ssp_grouper`.
- **`POST /v3/personalized-estimates` request:** `member_eligibility.consent_attested` must be a boolean.
- **`POST /v3/personalized-estimates` response:**
  - `provider.name` is now required and never null.
  - `provider.address` is now required.
  - `total_allowed_amount` and `member_cost_share.total` are now typed money objects with required `amount` and `minor_units`.
  - `error.code` is now a typed string enum. The set of values is unchanged.
- **Generated SDK types renamed or removed** (wire format unchanged):
  - `Provider` is now `ConsumerPricingProvider`, and `ListEnvelope_Provider_` is now `ListEnvelope_ConsumerPricingProvider_`.
  - The personalized-estimate and page-meta response types are now `ConsumerSitePersonalizedEstimate` and `ConsumerSitePageMeta`.
  - Removed: `PricesCompareRequest`, `PriceComparison`, `ConsumerSitePriceComparison*` and `ConsumerPricingV2ErrorCode`.
