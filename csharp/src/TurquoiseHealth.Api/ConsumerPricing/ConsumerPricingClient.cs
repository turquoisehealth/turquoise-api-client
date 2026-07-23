using System.Text.Json;
using TurquoiseHealth.Api.Core;

namespace TurquoiseHealth.Api;

public partial class ConsumerPricingClient
{
    private RawClient _client;

    internal ConsumerPricingClient(RawClient client)
    {
        _client = client;
    }

    /// <summary>
    /// Discover available service/surgery packages (SSPs).Supports substring matching on name and description fields as well as a 'search' parameter for semantic search across both fields. When 'search' is provided, other filter parameters are ignored, and pagination is limited to first page of top results.You can also provide a minimum similarity score threshold (0-1) for semantic search results using the 'min_score' parameter.
    /// </summary>
    /// <example><code>
    /// await client.ConsumerPricing.V2ListSspsAsync(new V2ListSspsRequest());
    /// </code></example>
    public async Task<EnvelopeSsp> V2ListSspsAsync(
        V2ListSspsRequest request,
        RequestOptions? options = null,
        CancellationToken cancellationToken = default
    )
    {
        var _query = new Dictionary<string, object>();
        if (request.Name != null)
        {
            _query["name"] = request.Name;
        }
        if (request.Description != null)
        {
            _query["description"] = request.Description;
        }
        if (request.Search != null)
        {
            _query["search"] = request.Search;
        }
        if (request.MinScore != null)
        {
            _query["min_score"] = request.MinScore.Value.ToString();
        }
        if (request.PageSize != null)
        {
            _query["page_size"] = request.PageSize.Value.ToString();
        }
        if (request.Cursor != null)
        {
            _query["cursor"] = request.Cursor;
        }
        var response = await _client
            .SendRequestAsync(
                new JsonRequest
                {
                    BaseUrl = _client.Options.BaseUrl,
                    Method = HttpMethod.Get,
                    Path = "v2/consumer-pricing/ssps",
                    Query = _query,
                    Options = options,
                },
                cancellationToken
            )
            .ConfigureAwait(false);
        if (response.StatusCode is >= 200 and < 400)
        {
            var responseBody = await response.Raw.Content.ReadAsStringAsync();
            try
            {
                return JsonUtils.Deserialize<EnvelopeSsp>(responseBody)!;
            }
            catch (JsonException e)
            {
                throw new TurquoisehealthApiException("Failed to deserialize response", e);
            }
        }

        {
            var responseBody = await response.Raw.Content.ReadAsStringAsync();
            try
            {
                switch (response.StatusCode)
                {
                    case 400:
                        throw new BadRequestError(JsonUtils.Deserialize<object>(responseBody));
                    case 422:
                        throw new UnprocessableEntityError(
                            JsonUtils.Deserialize<object>(responseBody)
                        );
                    case 429:
                        throw new TooManyRequestsError(JsonUtils.Deserialize<object>(responseBody));
                    case 500:
                        throw new InternalServerError(JsonUtils.Deserialize<object>(responseBody));
                }
            }
            catch (JsonException)
            {
                // unable to map error response, throwing generic error
            }
            throw new TurquoisehealthApiApiException(
                $"Error with status code {response.StatusCode}",
                response.StatusCode,
                responseBody
            );
        }
    }

    /// <summary>
    /// Retrieve an SSP name and patient-ready description from the relevant SSP ID.
    /// </summary>
    /// <example><code>
    /// await client.ConsumerPricing.V2GetSspAsync(new V2GetSspRequest { SspId = "GA002" });
    /// </code></example>
    public async Task<SingleResourceEnvelopeSsp> V2GetSspAsync(
        V2GetSspRequest request,
        RequestOptions? options = null,
        CancellationToken cancellationToken = default
    )
    {
        var response = await _client
            .SendRequestAsync(
                new JsonRequest
                {
                    BaseUrl = _client.Options.BaseUrl,
                    Method = HttpMethod.Get,
                    Path = string.Format(
                        "v2/consumer-pricing/ssps/{0}",
                        ValueConvert.ToPathParameterString(request.SspId)
                    ),
                    Options = options,
                },
                cancellationToken
            )
            .ConfigureAwait(false);
        if (response.StatusCode is >= 200 and < 400)
        {
            var responseBody = await response.Raw.Content.ReadAsStringAsync();
            try
            {
                return JsonUtils.Deserialize<SingleResourceEnvelopeSsp>(responseBody)!;
            }
            catch (JsonException e)
            {
                throw new TurquoisehealthApiException("Failed to deserialize response", e);
            }
        }

        {
            var responseBody = await response.Raw.Content.ReadAsStringAsync();
            try
            {
                switch (response.StatusCode)
                {
                    case 400:
                        throw new BadRequestError(JsonUtils.Deserialize<object>(responseBody));
                    case 422:
                        throw new UnprocessableEntityError(
                            JsonUtils.Deserialize<object>(responseBody)
                        );
                    case 429:
                        throw new TooManyRequestsError(JsonUtils.Deserialize<object>(responseBody));
                    case 500:
                        throw new InternalServerError(JsonUtils.Deserialize<object>(responseBody));
                }
            }
            catch (JsonException)
            {
                // unable to map error response, throwing generic error
            }
            throw new TurquoisehealthApiApiException(
                $"Error with status code {response.StatusCode}",
                response.StatusCode,
                responseBody
            );
        }
    }

    /// <summary>
    /// Search for providers by name, NPI, or location. Supports a 'search' parameter for semantic search across provider names, which can be combined with location filters. When 'search' is provided, the name and npi filters are ignored, and pagination is limited to first page of top results. You can also provide a minimum similarity score threshold (0-1) for semantic search results using the 'min_score' parameter.
    /// </summary>
    /// <example><code>
    /// await client.ConsumerPricing.V2ListProvidersAsync(new V2ListProvidersRequest());
    /// </code></example>
    public async Task<EnvelopeProvider> V2ListProvidersAsync(
        V2ListProvidersRequest request,
        RequestOptions? options = null,
        CancellationToken cancellationToken = default
    )
    {
        var _query = new Dictionary<string, object>();
        if (request.Name != null)
        {
            _query["name"] = request.Name;
        }
        if (request.Npi != null)
        {
            _query["npi"] = request.Npi;
        }
        if (request.Search != null)
        {
            _query["search"] = request.Search;
        }
        if (request.MinScore != null)
        {
            _query["min_score"] = request.MinScore.Value.ToString();
        }
        if (request.PageSize != null)
        {
            _query["page_size"] = request.PageSize.Value.ToString();
        }
        if (request.Cursor != null)
        {
            _query["cursor"] = request.Cursor;
        }
        if (request.NearLat != null)
        {
            _query["near.lat"] = request.NearLat.Value.ToString();
        }
        if (request.NearLng != null)
        {
            _query["near.lng"] = request.NearLng.Value.ToString();
        }
        if (request.NearRadiusM != null)
        {
            _query["near.radius_m"] = request.NearRadiusM.Value.ToString();
        }
        if (request.WithinState != null)
        {
            _query["within.state"] = request.WithinState;
        }
        if (request.WithinCbsaName != null)
        {
            _query["within.cbsa_name"] = request.WithinCbsaName;
        }
        if (request.WithinZipCodes != null)
        {
            _query["within.zip_codes"] = JsonUtils.Serialize(request.WithinZipCodes);
        }
        if (request.ZipAnchor != null)
        {
            _query["zip_anchor"] = request.ZipAnchor;
        }
        var response = await _client
            .SendRequestAsync(
                new JsonRequest
                {
                    BaseUrl = _client.Options.BaseUrl,
                    Method = HttpMethod.Get,
                    Path = "v2/consumer-pricing/providers",
                    Query = _query,
                    Options = options,
                },
                cancellationToken
            )
            .ConfigureAwait(false);
        if (response.StatusCode is >= 200 and < 400)
        {
            var responseBody = await response.Raw.Content.ReadAsStringAsync();
            try
            {
                return JsonUtils.Deserialize<EnvelopeProvider>(responseBody)!;
            }
            catch (JsonException e)
            {
                throw new TurquoisehealthApiException("Failed to deserialize response", e);
            }
        }

        {
            var responseBody = await response.Raw.Content.ReadAsStringAsync();
            try
            {
                switch (response.StatusCode)
                {
                    case 400:
                        throw new BadRequestError(JsonUtils.Deserialize<object>(responseBody));
                    case 422:
                        throw new UnprocessableEntityError(
                            JsonUtils.Deserialize<object>(responseBody)
                        );
                    case 429:
                        throw new TooManyRequestsError(JsonUtils.Deserialize<object>(responseBody));
                    case 500:
                        throw new InternalServerError(JsonUtils.Deserialize<object>(responseBody));
                }
            }
            catch (JsonException)
            {
                // unable to map error response, throwing generic error
            }
            throw new TurquoisehealthApiApiException(
                $"Error with status code {response.StatusCode}",
                response.StatusCode,
                responseBody
            );
        }
    }

    /// <summary>
    /// Return relevant information about a specific provider from the provider ID, including name, NPI, and location.
    /// </summary>
    /// <example><code>
    /// await client.ConsumerPricing.V2GetProviderAsync(new V2GetProviderRequest { ProviderId = "21929" });
    /// </code></example>
    public async Task<SingleResourceEnvelopeProvider> V2GetProviderAsync(
        V2GetProviderRequest request,
        RequestOptions? options = null,
        CancellationToken cancellationToken = default
    )
    {
        var response = await _client
            .SendRequestAsync(
                new JsonRequest
                {
                    BaseUrl = _client.Options.BaseUrl,
                    Method = HttpMethod.Get,
                    Path = string.Format(
                        "v2/consumer-pricing/providers/{0}",
                        ValueConvert.ToPathParameterString(request.ProviderId)
                    ),
                    Options = options,
                },
                cancellationToken
            )
            .ConfigureAwait(false);
        if (response.StatusCode is >= 200 and < 400)
        {
            var responseBody = await response.Raw.Content.ReadAsStringAsync();
            try
            {
                return JsonUtils.Deserialize<SingleResourceEnvelopeProvider>(responseBody)!;
            }
            catch (JsonException e)
            {
                throw new TurquoisehealthApiException("Failed to deserialize response", e);
            }
        }

        {
            var responseBody = await response.Raw.Content.ReadAsStringAsync();
            try
            {
                switch (response.StatusCode)
                {
                    case 400:
                        throw new BadRequestError(JsonUtils.Deserialize<object>(responseBody));
                    case 422:
                        throw new UnprocessableEntityError(
                            JsonUtils.Deserialize<object>(responseBody)
                        );
                    case 429:
                        throw new TooManyRequestsError(JsonUtils.Deserialize<object>(responseBody));
                    case 500:
                        throw new InternalServerError(JsonUtils.Deserialize<object>(responseBody));
                }
            }
            catch (JsonException)
            {
                // unable to map error response, throwing generic error
            }
            throw new TurquoisehealthApiApiException(
                $"Error with status code {response.StatusCode}",
                response.StatusCode,
                responseBody
            );
        }
    }

    /// <summary>
    /// Discover insurance networks, optionally filtered by network name, payer name, or payer ID, or by semantic search across network and payer names. When using the 'search' parameter for semantic search, other filter parameters (name, payer_id) are ignored, and pagination is limited to the first page of top results.
    /// </summary>
    /// <example><code>
    /// await client.ConsumerPricing.V2ListNetworksAsync(new V2ListNetworksRequest());
    /// </code></example>
    public async Task<EnvelopeNetwork> V2ListNetworksAsync(
        V2ListNetworksRequest request,
        RequestOptions? options = null,
        CancellationToken cancellationToken = default
    )
    {
        var _query = new Dictionary<string, object>();
        if (request.Name != null)
        {
            _query["name"] = request.Name;
        }
        if (request.PayerId != null)
        {
            _query["payer_id"] = request.PayerId;
        }
        if (request.Search != null)
        {
            _query["search"] = request.Search;
        }
        if (request.MinScore != null)
        {
            _query["min_score"] = request.MinScore.Value.ToString();
        }
        if (request.PageSize != null)
        {
            _query["page_size"] = request.PageSize.Value.ToString();
        }
        if (request.Cursor != null)
        {
            _query["cursor"] = request.Cursor;
        }
        var response = await _client
            .SendRequestAsync(
                new JsonRequest
                {
                    BaseUrl = _client.Options.BaseUrl,
                    Method = HttpMethod.Get,
                    Path = "v2/consumer-pricing/payers/networks",
                    Query = _query,
                    Options = options,
                },
                cancellationToken
            )
            .ConfigureAwait(false);
        if (response.StatusCode is >= 200 and < 400)
        {
            var responseBody = await response.Raw.Content.ReadAsStringAsync();
            try
            {
                return JsonUtils.Deserialize<EnvelopeNetwork>(responseBody)!;
            }
            catch (JsonException e)
            {
                throw new TurquoisehealthApiException("Failed to deserialize response", e);
            }
        }

        {
            var responseBody = await response.Raw.Content.ReadAsStringAsync();
            try
            {
                switch (response.StatusCode)
                {
                    case 400:
                        throw new BadRequestError(JsonUtils.Deserialize<object>(responseBody));
                    case 422:
                        throw new UnprocessableEntityError(
                            JsonUtils.Deserialize<object>(responseBody)
                        );
                    case 429:
                        throw new TooManyRequestsError(JsonUtils.Deserialize<object>(responseBody));
                    case 500:
                        throw new InternalServerError(JsonUtils.Deserialize<object>(responseBody));
                }
            }
            catch (JsonException)
            {
                // unable to map error response, throwing generic error
            }
            throw new TurquoisehealthApiApiException(
                $"Error with status code {response.StatusCode}",
                response.StatusCode,
                responseBody
            );
        }
    }

    /// <summary>
    /// Return relevant information about a specific insurance network from the network ID.
    /// </summary>
    /// <example><code>
    /// await client.ConsumerPricing.V2GetNetworkAsync(
    ///     new V2GetNetworkRequest { NetworkId = "8361580493441765265" }
    /// );
    /// </code></example>
    public async Task<SingleResourceEnvelopeNetwork> V2GetNetworkAsync(
        V2GetNetworkRequest request,
        RequestOptions? options = null,
        CancellationToken cancellationToken = default
    )
    {
        var response = await _client
            .SendRequestAsync(
                new JsonRequest
                {
                    BaseUrl = _client.Options.BaseUrl,
                    Method = HttpMethod.Get,
                    Path = string.Format(
                        "v2/consumer-pricing/payers/networks/{0}",
                        ValueConvert.ToPathParameterString(request.NetworkId)
                    ),
                    Options = options,
                },
                cancellationToken
            )
            .ConfigureAwait(false);
        if (response.StatusCode is >= 200 and < 400)
        {
            var responseBody = await response.Raw.Content.ReadAsStringAsync();
            try
            {
                return JsonUtils.Deserialize<SingleResourceEnvelopeNetwork>(responseBody)!;
            }
            catch (JsonException e)
            {
                throw new TurquoisehealthApiException("Failed to deserialize response", e);
            }
        }

        {
            var responseBody = await response.Raw.Content.ReadAsStringAsync();
            try
            {
                switch (response.StatusCode)
                {
                    case 400:
                        throw new BadRequestError(JsonUtils.Deserialize<object>(responseBody));
                    case 422:
                        throw new UnprocessableEntityError(
                            JsonUtils.Deserialize<object>(responseBody)
                        );
                    case 429:
                        throw new TooManyRequestsError(JsonUtils.Deserialize<object>(responseBody));
                    case 500:
                        throw new InternalServerError(JsonUtils.Deserialize<object>(responseBody));
                }
            }
            catch (JsonException)
            {
                // unable to map error response, throwing generic error
            }
            throw new TurquoisehealthApiApiException(
                $"Error with status code {response.StatusCode}",
                response.StatusCode,
                responseBody
            );
        }
    }

    /// <summary>
    /// List of providers that can satisfy a given SSP, with the total expected price, in a given geographic area. Requests support location filtering via ZIP code, CBSA name, state, or coordinates. When no Network ID is provided, cash prices are shown.
    /// </summary>
    /// <example><code>
    /// await client.ConsumerPricing.V2ListPricesAsync(
    ///     new PricesRequest
    ///     {
    ///         SspId = "GA002",
    ///         NetworkId = "8361580493441765265",
    ///         Location = new RateCompareLocation { ZipAnchor = "80202" },
    ///     }
    /// );
    /// </code></example>
    public async Task<EnvelopeRate> V2ListPricesAsync(
        PricesRequest request,
        RequestOptions? options = null,
        CancellationToken cancellationToken = default
    )
    {
        var response = await _client
            .SendRequestAsync(
                new JsonRequest
                {
                    BaseUrl = _client.Options.BaseUrl,
                    Method = HttpMethod.Post,
                    Path = "v2/consumer-pricing/prices",
                    Body = request,
                    ContentType = "application/json",
                    Options = options,
                },
                cancellationToken
            )
            .ConfigureAwait(false);
        if (response.StatusCode is >= 200 and < 400)
        {
            var responseBody = await response.Raw.Content.ReadAsStringAsync();
            try
            {
                return JsonUtils.Deserialize<EnvelopeRate>(responseBody)!;
            }
            catch (JsonException e)
            {
                throw new TurquoisehealthApiException("Failed to deserialize response", e);
            }
        }

        {
            var responseBody = await response.Raw.Content.ReadAsStringAsync();
            try
            {
                switch (response.StatusCode)
                {
                    case 400:
                        throw new BadRequestError(JsonUtils.Deserialize<object>(responseBody));
                    case 422:
                        throw new UnprocessableEntityError(
                            JsonUtils.Deserialize<object>(responseBody)
                        );
                    case 429:
                        throw new TooManyRequestsError(JsonUtils.Deserialize<object>(responseBody));
                    case 500:
                        throw new InternalServerError(JsonUtils.Deserialize<object>(responseBody));
                }
            }
            catch (JsonException)
            {
                // unable to map error response, throwing generic error
            }
            throw new TurquoisehealthApiApiException(
                $"Error with status code {response.StatusCode}",
                response.StatusCode,
                responseBody
            );
        }
    }

    /// <summary>
    /// Get SSP breakdown and fee information about a selected SSP from a single provider. When no Network ID is provided, cash prices are shown.
    /// </summary>
    /// <example><code>
    /// await client.ConsumerPricing.V2GetPriceBreakdownAsync(
    ///     new ProviderBreakdownRequest { ProviderId = "21929", SspId = "RA011" }
    /// );
    /// </code></example>
    public async Task<SingleResourceEnvelopeRateBreakdown> V2GetPriceBreakdownAsync(
        ProviderBreakdownRequest request,
        RequestOptions? options = null,
        CancellationToken cancellationToken = default
    )
    {
        var response = await _client
            .SendRequestAsync(
                new JsonRequest
                {
                    BaseUrl = _client.Options.BaseUrl,
                    Method = HttpMethod.Post,
                    Path = "v2/consumer-pricing/prices/provider-breakdown",
                    Body = request,
                    ContentType = "application/json",
                    Options = options,
                },
                cancellationToken
            )
            .ConfigureAwait(false);
        if (response.StatusCode is >= 200 and < 400)
        {
            var responseBody = await response.Raw.Content.ReadAsStringAsync();
            try
            {
                return JsonUtils.Deserialize<SingleResourceEnvelopeRateBreakdown>(responseBody)!;
            }
            catch (JsonException e)
            {
                throw new TurquoisehealthApiException("Failed to deserialize response", e);
            }
        }

        {
            var responseBody = await response.Raw.Content.ReadAsStringAsync();
            try
            {
                switch (response.StatusCode)
                {
                    case 400:
                        throw new BadRequestError(JsonUtils.Deserialize<object>(responseBody));
                    case 422:
                        throw new UnprocessableEntityError(
                            JsonUtils.Deserialize<object>(responseBody)
                        );
                    case 429:
                        throw new TooManyRequestsError(JsonUtils.Deserialize<object>(responseBody));
                    case 500:
                        throw new InternalServerError(JsonUtils.Deserialize<object>(responseBody));
                }
            }
            catch (JsonException)
            {
                // unable to map error response, throwing generic error
            }
            throw new TurquoisehealthApiApiException(
                $"Error with status code {response.StatusCode}",
                response.StatusCode,
                responseBody
            );
        }
    }

    /// <summary>
    /// Get summary statistics (min, max, average, quartiles) for prices matching the selected filters. When no Network ID is provided, cash prices are shown.
    /// </summary>
    /// <example><code>
    /// await client.ConsumerPricing.V2ComparePricesAsync(
    ///     new RateCompareRequest
    ///     {
    ///         SspId = "RA011",
    ///         NetworkId = "8361580493441765265",
    ///         Location = new RateCompareLocation { WithinState = "CO" },
    ///     }
    /// );
    /// </code></example>
    public async Task<RateComparison> V2ComparePricesAsync(
        RateCompareRequest request,
        RequestOptions? options = null,
        CancellationToken cancellationToken = default
    )
    {
        var response = await _client
            .SendRequestAsync(
                new JsonRequest
                {
                    BaseUrl = _client.Options.BaseUrl,
                    Method = HttpMethod.Post,
                    Path = "v2/consumer-pricing/prices/price-comparison",
                    Body = request,
                    ContentType = "application/json",
                    Options = options,
                },
                cancellationToken
            )
            .ConfigureAwait(false);
        if (response.StatusCode is >= 200 and < 400)
        {
            var responseBody = await response.Raw.Content.ReadAsStringAsync();
            try
            {
                return JsonUtils.Deserialize<RateComparison>(responseBody)!;
            }
            catch (JsonException e)
            {
                throw new TurquoisehealthApiException("Failed to deserialize response", e);
            }
        }

        {
            var responseBody = await response.Raw.Content.ReadAsStringAsync();
            try
            {
                switch (response.StatusCode)
                {
                    case 400:
                        throw new BadRequestError(JsonUtils.Deserialize<object>(responseBody));
                    case 422:
                        throw new UnprocessableEntityError(
                            JsonUtils.Deserialize<object>(responseBody)
                        );
                    case 429:
                        throw new TooManyRequestsError(JsonUtils.Deserialize<object>(responseBody));
                    case 500:
                        throw new InternalServerError(JsonUtils.Deserialize<object>(responseBody));
                }
            }
            catch (JsonException)
            {
                // unable to map error response, throwing generic error
            }
            throw new TurquoisehealthApiApiException(
                $"Error with status code {response.StatusCode}",
                response.StatusCode,
                responseBody
            );
        }
    }

    /// <summary>
    /// List negotiated-rate networks, filterable by name (case-insensitive substring, matches network or payer name), payer_id (exact), relationships (provider_id/package_id — networks with at least one matching price), and one location mode: `location.near.*`, `location.within.*` (state/cbsa/zip_codes), or `location.zip`. Location scopes to networks with at least one price at an in-area provider, so `package_id` + location answers 'which networks price this package here' in one call. Results reflect networks Turquoise has priced. The network's payer is an {id, name} stub; the full entity lives at /v3/payers/{id}.
    /// </summary>
    /// <example><code>
    /// await client.ConsumerPricing.V3ListNetworksAsync(new V3ListNetworksRequest());
    /// </code></example>
    public async Task<V3ListEnvelopeNetwork> V3ListNetworksAsync(
        V3ListNetworksRequest request,
        RequestOptions? options = null,
        CancellationToken cancellationToken = default
    )
    {
        var _query = new Dictionary<string, object>();
        if (request.Name != null)
        {
            _query["name"] = request.Name;
        }
        if (request.PayerId != null)
        {
            _query["payer_id"] = request.PayerId;
        }
        if (request.ProviderId != null)
        {
            _query["provider_id"] = request.ProviderId;
        }
        if (request.PackageId != null)
        {
            _query["package_id"] = request.PackageId;
        }
        if (request.Search != null)
        {
            _query["search"] = request.Search;
        }
        if (request.MinScore != null)
        {
            _query["min_score"] = request.MinScore.Value.ToString();
        }
        if (request.PageSize != null)
        {
            _query["page_size"] = request.PageSize.Value.ToString();
        }
        if (request.Cursor != null)
        {
            _query["cursor"] = request.Cursor;
        }
        if (request.LocationNearLat != null)
        {
            _query["location.near.lat"] = request.LocationNearLat.Value.ToString();
        }
        if (request.LocationNearLng != null)
        {
            _query["location.near.lng"] = request.LocationNearLng.Value.ToString();
        }
        if (request.LocationNearRadiusM != null)
        {
            _query["location.near.radius_m"] = request.LocationNearRadiusM.Value.ToString();
        }
        if (request.LocationWithinState != null)
        {
            _query["location.within.state"] = request.LocationWithinState;
        }
        if (request.LocationWithinCbsa != null)
        {
            _query["location.within.cbsa"] = request.LocationWithinCbsa;
        }
        if (request.LocationWithinZipCodes != null)
        {
            _query["location.within.zip_codes"] = request.LocationWithinZipCodes;
        }
        if (request.LocationZip != null)
        {
            _query["location.zip"] = request.LocationZip;
        }
        var response = await _client
            .SendRequestAsync(
                new JsonRequest
                {
                    BaseUrl = _client.Options.BaseUrl,
                    Method = HttpMethod.Get,
                    Path = "v3/networks",
                    Query = _query,
                    Options = options,
                },
                cancellationToken
            )
            .ConfigureAwait(false);
        if (response.StatusCode is >= 200 and < 400)
        {
            var responseBody = await response.Raw.Content.ReadAsStringAsync();
            try
            {
                return JsonUtils.Deserialize<V3ListEnvelopeNetwork>(responseBody)!;
            }
            catch (JsonException e)
            {
                throw new TurquoisehealthApiException("Failed to deserialize response", e);
            }
        }

        {
            var responseBody = await response.Raw.Content.ReadAsStringAsync();
            try
            {
                switch (response.StatusCode)
                {
                    case 400:
                        throw new BadRequestError(JsonUtils.Deserialize<object>(responseBody));
                    case 404:
                        throw new NotFoundError(
                            JsonUtils.Deserialize<V3ErrorResponse>(responseBody)
                        );
                    case 422:
                        throw new UnprocessableEntityError(
                            JsonUtils.Deserialize<object>(responseBody)
                        );
                    case 429:
                        throw new TooManyRequestsError(JsonUtils.Deserialize<object>(responseBody));
                    case 500:
                        throw new InternalServerError(JsonUtils.Deserialize<object>(responseBody));
                }
            }
            catch (JsonException)
            {
                // unable to map error response, throwing generic error
            }
            throw new TurquoisehealthApiApiException(
                $"Error with status code {response.StatusCode}",
                response.StatusCode,
                responseBody
            );
        }
    }

    /// <summary>
    /// Fetch a single network by ID. Missing or unpermissioned IDs return a 404 with the standard error body.
    /// </summary>
    /// <example><code>
    /// await client.ConsumerPricing.V3GetNetworkAsync(
    ///     new V3GetNetworkRequest { NetworkId = "2010265101" }
    /// );
    /// </code></example>
    public async Task<V3Network> V3GetNetworkAsync(
        V3GetNetworkRequest request,
        RequestOptions? options = null,
        CancellationToken cancellationToken = default
    )
    {
        var response = await _client
            .SendRequestAsync(
                new JsonRequest
                {
                    BaseUrl = _client.Options.BaseUrl,
                    Method = HttpMethod.Get,
                    Path = string.Format(
                        "v3/networks/{0}",
                        ValueConvert.ToPathParameterString(request.NetworkId)
                    ),
                    Options = options,
                },
                cancellationToken
            )
            .ConfigureAwait(false);
        if (response.StatusCode is >= 200 and < 400)
        {
            var responseBody = await response.Raw.Content.ReadAsStringAsync();
            try
            {
                return JsonUtils.Deserialize<V3Network>(responseBody)!;
            }
            catch (JsonException e)
            {
                throw new TurquoisehealthApiException("Failed to deserialize response", e);
            }
        }

        {
            var responseBody = await response.Raw.Content.ReadAsStringAsync();
            try
            {
                switch (response.StatusCode)
                {
                    case 400:
                        throw new BadRequestError(JsonUtils.Deserialize<object>(responseBody));
                    case 404:
                        throw new NotFoundError(
                            JsonUtils.Deserialize<V3ErrorResponse>(responseBody)
                        );
                    case 422:
                        throw new UnprocessableEntityError(
                            JsonUtils.Deserialize<object>(responseBody)
                        );
                    case 429:
                        throw new TooManyRequestsError(JsonUtils.Deserialize<object>(responseBody));
                    case 500:
                        throw new InternalServerError(JsonUtils.Deserialize<object>(responseBody));
                }
            }
            catch (JsonException)
            {
                // unable to map error response, throwing generic error
            }
            throw new TurquoisehealthApiApiException(
                $"Error with status code {response.StatusCode}",
                response.StatusCode,
                responseBody
            );
        }
    }

    /// <summary>
    /// List service packages, filterable by name (case-insensitive substring), anchor billing code (exact, matches package base codes only), and relationships (provider_id, network_id, payer_id — packages with at least one matching price). When multiple relationship filters are combined, they must be satisfied by the same price row, so results are always fulfillable via GET /v3/prices. Results reflect packages Turquoise has priced.
    /// </summary>
    /// <example><code>
    /// await client.ConsumerPricing.V3ListPackagesAsync(new V3ListPackagesRequest());
    /// </code></example>
    public async Task<V3ListEnvelopePackage> V3ListPackagesAsync(
        V3ListPackagesRequest request,
        RequestOptions? options = null,
        CancellationToken cancellationToken = default
    )
    {
        var _query = new Dictionary<string, object>();
        if (request.Name != null)
        {
            _query["name"] = request.Name;
        }
        if (request.AnchorCode != null)
        {
            _query["anchor_code"] = request.AnchorCode;
        }
        if (request.ProviderId != null)
        {
            _query["provider_id"] = request.ProviderId;
        }
        if (request.NetworkId != null)
        {
            _query["network_id"] = request.NetworkId;
        }
        if (request.PayerId != null)
        {
            _query["payer_id"] = request.PayerId;
        }
        if (request.Search != null)
        {
            _query["search"] = request.Search;
        }
        if (request.MinScore != null)
        {
            _query["min_score"] = request.MinScore.Value.ToString();
        }
        if (request.PageSize != null)
        {
            _query["page_size"] = request.PageSize.Value.ToString();
        }
        if (request.Cursor != null)
        {
            _query["cursor"] = request.Cursor;
        }
        var response = await _client
            .SendRequestAsync(
                new JsonRequest
                {
                    BaseUrl = _client.Options.BaseUrl,
                    Method = HttpMethod.Get,
                    Path = "v3/packages",
                    Query = _query,
                    Options = options,
                },
                cancellationToken
            )
            .ConfigureAwait(false);
        if (response.StatusCode is >= 200 and < 400)
        {
            var responseBody = await response.Raw.Content.ReadAsStringAsync();
            try
            {
                return JsonUtils.Deserialize<V3ListEnvelopePackage>(responseBody)!;
            }
            catch (JsonException e)
            {
                throw new TurquoisehealthApiException("Failed to deserialize response", e);
            }
        }

        {
            var responseBody = await response.Raw.Content.ReadAsStringAsync();
            try
            {
                switch (response.StatusCode)
                {
                    case 400:
                        throw new BadRequestError(JsonUtils.Deserialize<object>(responseBody));
                    case 404:
                        throw new NotFoundError(
                            JsonUtils.Deserialize<V3ErrorResponse>(responseBody)
                        );
                    case 422:
                        throw new UnprocessableEntityError(
                            JsonUtils.Deserialize<object>(responseBody)
                        );
                    case 429:
                        throw new TooManyRequestsError(JsonUtils.Deserialize<object>(responseBody));
                    case 500:
                        throw new InternalServerError(JsonUtils.Deserialize<object>(responseBody));
                }
            }
            catch (JsonException)
            {
                // unable to map error response, throwing generic error
            }
            throw new TurquoisehealthApiApiException(
                $"Error with status code {response.StatusCode}",
                response.StatusCode,
                responseBody
            );
        }
    }

    /// <summary>
    /// Fetch a single package by ID. Missing or unpermissioned IDs return a 404 with the standard error body.
    /// </summary>
    /// <example><code>
    /// await client.ConsumerPricing.V3GetPackageAsync(new V3GetPackageRequest { PackageId = "GA002" });
    /// </code></example>
    public async Task<V3Package> V3GetPackageAsync(
        V3GetPackageRequest request,
        RequestOptions? options = null,
        CancellationToken cancellationToken = default
    )
    {
        var response = await _client
            .SendRequestAsync(
                new JsonRequest
                {
                    BaseUrl = _client.Options.BaseUrl,
                    Method = HttpMethod.Get,
                    Path = string.Format(
                        "v3/packages/{0}",
                        ValueConvert.ToPathParameterString(request.PackageId)
                    ),
                    Options = options,
                },
                cancellationToken
            )
            .ConfigureAwait(false);
        if (response.StatusCode is >= 200 and < 400)
        {
            var responseBody = await response.Raw.Content.ReadAsStringAsync();
            try
            {
                return JsonUtils.Deserialize<V3Package>(responseBody)!;
            }
            catch (JsonException e)
            {
                throw new TurquoisehealthApiException("Failed to deserialize response", e);
            }
        }

        {
            var responseBody = await response.Raw.Content.ReadAsStringAsync();
            try
            {
                switch (response.StatusCode)
                {
                    case 400:
                        throw new BadRequestError(JsonUtils.Deserialize<object>(responseBody));
                    case 404:
                        throw new NotFoundError(
                            JsonUtils.Deserialize<V3ErrorResponse>(responseBody)
                        );
                    case 422:
                        throw new UnprocessableEntityError(
                            JsonUtils.Deserialize<object>(responseBody)
                        );
                    case 429:
                        throw new TooManyRequestsError(JsonUtils.Deserialize<object>(responseBody));
                    case 500:
                        throw new InternalServerError(JsonUtils.Deserialize<object>(responseBody));
                }
            }
            catch (JsonException)
            {
                // unable to map error response, throwing generic error
            }
            throw new TurquoisehealthApiApiException(
                $"Error with status code {response.StatusCode}",
                response.StatusCode,
                responseBody
            );
        }
    }

    /// <summary>
    /// The package's composition: codes, fee types, and association rates at the current package version.
    /// </summary>
    /// <example><code>
    /// await client.ConsumerPricing.V3ListPackageLineItemsAsync(
    ///     new V3ListPackageLineItemsRequest { PackageId = "GA002" }
    /// );
    /// </code></example>
    public async Task<V3ListEnvelopeLineItem> V3ListPackageLineItemsAsync(
        V3ListPackageLineItemsRequest request,
        RequestOptions? options = null,
        CancellationToken cancellationToken = default
    )
    {
        var response = await _client
            .SendRequestAsync(
                new JsonRequest
                {
                    BaseUrl = _client.Options.BaseUrl,
                    Method = HttpMethod.Get,
                    Path = string.Format(
                        "v3/packages/{0}/line_items",
                        ValueConvert.ToPathParameterString(request.PackageId)
                    ),
                    Options = options,
                },
                cancellationToken
            )
            .ConfigureAwait(false);
        if (response.StatusCode is >= 200 and < 400)
        {
            var responseBody = await response.Raw.Content.ReadAsStringAsync();
            try
            {
                return JsonUtils.Deserialize<V3ListEnvelopeLineItem>(responseBody)!;
            }
            catch (JsonException e)
            {
                throw new TurquoisehealthApiException("Failed to deserialize response", e);
            }
        }

        {
            var responseBody = await response.Raw.Content.ReadAsStringAsync();
            try
            {
                switch (response.StatusCode)
                {
                    case 400:
                        throw new BadRequestError(JsonUtils.Deserialize<object>(responseBody));
                    case 404:
                        throw new NotFoundError(
                            JsonUtils.Deserialize<V3ErrorResponse>(responseBody)
                        );
                    case 422:
                        throw new UnprocessableEntityError(
                            JsonUtils.Deserialize<object>(responseBody)
                        );
                    case 429:
                        throw new TooManyRequestsError(JsonUtils.Deserialize<object>(responseBody));
                    case 500:
                        throw new InternalServerError(JsonUtils.Deserialize<object>(responseBody));
                }
            }
            catch (JsonException)
            {
                // unable to map error response, throwing generic error
            }
            throw new TurquoisehealthApiApiException(
                $"Error with status code {response.StatusCode}",
                response.StatusCode,
                responseBody
            );
        }
    }

    /// <summary>
    /// List payers, filterable by name (case-insensitive substring), relationships (provider_id/package_id — payers with at least one matching price), and one location mode: `location.near.*`, `location.within.*` (state/cbsa/zip_codes), or `location.zip`. Location scopes to payers with at least one price at an in-area provider. Results reflect payers Turquoise has priced.
    /// </summary>
    /// <example><code>
    /// await client.ConsumerPricing.V3ListPayersAsync(new V3ListPayersRequest());
    /// </code></example>
    public async Task<V3ListEnvelopePayer> V3ListPayersAsync(
        V3ListPayersRequest request,
        RequestOptions? options = null,
        CancellationToken cancellationToken = default
    )
    {
        var _query = new Dictionary<string, object>();
        if (request.Name != null)
        {
            _query["name"] = request.Name;
        }
        if (request.ProviderId != null)
        {
            _query["provider_id"] = request.ProviderId;
        }
        if (request.PackageId != null)
        {
            _query["package_id"] = request.PackageId;
        }
        if (request.PageSize != null)
        {
            _query["page_size"] = request.PageSize.Value.ToString();
        }
        if (request.Cursor != null)
        {
            _query["cursor"] = request.Cursor;
        }
        if (request.LocationNearLat != null)
        {
            _query["location.near.lat"] = request.LocationNearLat.Value.ToString();
        }
        if (request.LocationNearLng != null)
        {
            _query["location.near.lng"] = request.LocationNearLng.Value.ToString();
        }
        if (request.LocationNearRadiusM != null)
        {
            _query["location.near.radius_m"] = request.LocationNearRadiusM.Value.ToString();
        }
        if (request.LocationWithinState != null)
        {
            _query["location.within.state"] = request.LocationWithinState;
        }
        if (request.LocationWithinCbsa != null)
        {
            _query["location.within.cbsa"] = request.LocationWithinCbsa;
        }
        if (request.LocationWithinZipCodes != null)
        {
            _query["location.within.zip_codes"] = request.LocationWithinZipCodes;
        }
        if (request.LocationZip != null)
        {
            _query["location.zip"] = request.LocationZip;
        }
        var response = await _client
            .SendRequestAsync(
                new JsonRequest
                {
                    BaseUrl = _client.Options.BaseUrl,
                    Method = HttpMethod.Get,
                    Path = "v3/payers",
                    Query = _query,
                    Options = options,
                },
                cancellationToken
            )
            .ConfigureAwait(false);
        if (response.StatusCode is >= 200 and < 400)
        {
            var responseBody = await response.Raw.Content.ReadAsStringAsync();
            try
            {
                return JsonUtils.Deserialize<V3ListEnvelopePayer>(responseBody)!;
            }
            catch (JsonException e)
            {
                throw new TurquoisehealthApiException("Failed to deserialize response", e);
            }
        }

        {
            var responseBody = await response.Raw.Content.ReadAsStringAsync();
            try
            {
                switch (response.StatusCode)
                {
                    case 400:
                        throw new BadRequestError(JsonUtils.Deserialize<object>(responseBody));
                    case 404:
                        throw new NotFoundError(
                            JsonUtils.Deserialize<V3ErrorResponse>(responseBody)
                        );
                    case 422:
                        throw new UnprocessableEntityError(
                            JsonUtils.Deserialize<object>(responseBody)
                        );
                    case 429:
                        throw new TooManyRequestsError(JsonUtils.Deserialize<object>(responseBody));
                    case 500:
                        throw new InternalServerError(JsonUtils.Deserialize<object>(responseBody));
                }
            }
            catch (JsonException)
            {
                // unable to map error response, throwing generic error
            }
            throw new TurquoisehealthApiApiException(
                $"Error with status code {response.StatusCode}",
                response.StatusCode,
                responseBody
            );
        }
    }

    /// <summary>
    /// Fetch a single payer by ID. Missing or unpermissioned IDs return a 404 with the standard error body.
    /// </summary>
    /// <example><code>
    /// await client.ConsumerPricing.V3GetPayerAsync(new V3GetPayerRequest { PayerId = "7001" });
    /// </code></example>
    public async Task<V3Payer> V3GetPayerAsync(
        V3GetPayerRequest request,
        RequestOptions? options = null,
        CancellationToken cancellationToken = default
    )
    {
        var response = await _client
            .SendRequestAsync(
                new JsonRequest
                {
                    BaseUrl = _client.Options.BaseUrl,
                    Method = HttpMethod.Get,
                    Path = string.Format(
                        "v3/payers/{0}",
                        ValueConvert.ToPathParameterString(request.PayerId)
                    ),
                    Options = options,
                },
                cancellationToken
            )
            .ConfigureAwait(false);
        if (response.StatusCode is >= 200 and < 400)
        {
            var responseBody = await response.Raw.Content.ReadAsStringAsync();
            try
            {
                return JsonUtils.Deserialize<V3Payer>(responseBody)!;
            }
            catch (JsonException e)
            {
                throw new TurquoisehealthApiException("Failed to deserialize response", e);
            }
        }

        {
            var responseBody = await response.Raw.Content.ReadAsStringAsync();
            try
            {
                switch (response.StatusCode)
                {
                    case 400:
                        throw new BadRequestError(JsonUtils.Deserialize<object>(responseBody));
                    case 404:
                        throw new NotFoundError(
                            JsonUtils.Deserialize<V3ErrorResponse>(responseBody)
                        );
                    case 422:
                        throw new UnprocessableEntityError(
                            JsonUtils.Deserialize<object>(responseBody)
                        );
                    case 429:
                        throw new TooManyRequestsError(JsonUtils.Deserialize<object>(responseBody));
                    case 500:
                        throw new InternalServerError(JsonUtils.Deserialize<object>(responseBody));
                }
            }
            catch (JsonException)
            {
                // unable to map error response, throwing generic error
            }
            throw new TurquoisehealthApiApiException(
                $"Error with status code {response.StatusCode}",
                response.StatusCode,
                responseBody
            );
        }
    }

    /// <summary>
    /// Search prices at the provider × package × pricing grain.
    /// </summary>
    /// <example><code>
    /// await client.ConsumerPricing.V3QueryPricesAsync(
    ///     new V3PricesQueryRequest
    ///     {
    ///         PackageId = "RA007",
    ///         ProviderId = "2751",
    ///         Pricing = new V3PricesQueryRequestPricing(
    ///             new V3PricesQueryRequestPricing.Negotiated(
    ///                 new V3PricingNegotiated { NetworkId = "network_id" }
    ///             )
    ///         ),
    ///     }
    /// );
    /// </code></example>
    public async Task<V3ListEnvelopeProviderPackagePrice> V3QueryPricesAsync(
        V3PricesQueryRequest request,
        RequestOptions? options = null,
        CancellationToken cancellationToken = default
    )
    {
        var response = await _client
            .SendRequestAsync(
                new JsonRequest
                {
                    BaseUrl = _client.Options.BaseUrl,
                    Method = HttpMethod.Post,
                    Path = "v3/prices/query",
                    Body = request,
                    ContentType = "application/json",
                    Options = options,
                },
                cancellationToken
            )
            .ConfigureAwait(false);
        if (response.StatusCode is >= 200 and < 400)
        {
            var responseBody = await response.Raw.Content.ReadAsStringAsync();
            try
            {
                return JsonUtils.Deserialize<V3ListEnvelopeProviderPackagePrice>(responseBody)!;
            }
            catch (JsonException e)
            {
                throw new TurquoisehealthApiException("Failed to deserialize response", e);
            }
        }

        {
            var responseBody = await response.Raw.Content.ReadAsStringAsync();
            try
            {
                switch (response.StatusCode)
                {
                    case 400:
                        throw new BadRequestError(JsonUtils.Deserialize<object>(responseBody));
                    case 404:
                        throw new NotFoundError(
                            JsonUtils.Deserialize<V3ErrorResponse>(responseBody)
                        );
                    case 422:
                        throw new UnprocessableEntityError(
                            JsonUtils.Deserialize<object>(responseBody)
                        );
                    case 429:
                        throw new TooManyRequestsError(JsonUtils.Deserialize<object>(responseBody));
                    case 500:
                        throw new InternalServerError(JsonUtils.Deserialize<object>(responseBody));
                }
            }
            catch (JsonException)
            {
                // unable to map error response, throwing generic error
            }
            throw new TurquoisehealthApiApiException(
                $"Error with status code {response.StatusCode}",
                response.StatusCode,
                responseBody
            );
        }
    }

    /// <summary>
    /// Summary statistics (count, min/max/avg/median/q1/q3 as Money) over the prices matching the same request shape as /query, minus sort and pagination.
    /// </summary>
    /// <example><code>
    /// await client.ConsumerPricing.V3ComparePricesAsync(
    ///     new V3PricesCompareRequest
    ///     {
    ///         PackageId = "RA007",
    ///         ProviderId = "2751",
    ///         Pricing = new V3PricesCompareRequestPricing(
    ///             new V3PricesCompareRequestPricing.Negotiated(
    ///                 new V3PricingNegotiated { NetworkId = "network_id" }
    ///             )
    ///         ),
    ///     }
    /// );
    /// </code></example>
    public async Task<V3PriceComparison> V3ComparePricesAsync(
        V3PricesCompareRequest request,
        RequestOptions? options = null,
        CancellationToken cancellationToken = default
    )
    {
        var response = await _client
            .SendRequestAsync(
                new JsonRequest
                {
                    BaseUrl = _client.Options.BaseUrl,
                    Method = HttpMethod.Post,
                    Path = "v3/prices/compare",
                    Body = request,
                    ContentType = "application/json",
                    Options = options,
                },
                cancellationToken
            )
            .ConfigureAwait(false);
        if (response.StatusCode is >= 200 and < 400)
        {
            var responseBody = await response.Raw.Content.ReadAsStringAsync();
            try
            {
                return JsonUtils.Deserialize<V3PriceComparison>(responseBody)!;
            }
            catch (JsonException e)
            {
                throw new TurquoisehealthApiException("Failed to deserialize response", e);
            }
        }

        {
            var responseBody = await response.Raw.Content.ReadAsStringAsync();
            try
            {
                switch (response.StatusCode)
                {
                    case 400:
                        throw new BadRequestError(JsonUtils.Deserialize<object>(responseBody));
                    case 404:
                        throw new NotFoundError(
                            JsonUtils.Deserialize<V3ErrorResponse>(responseBody)
                        );
                    case 422:
                        throw new UnprocessableEntityError(
                            JsonUtils.Deserialize<object>(responseBody)
                        );
                    case 429:
                        throw new TooManyRequestsError(JsonUtils.Deserialize<object>(responseBody));
                    case 500:
                        throw new InternalServerError(JsonUtils.Deserialize<object>(responseBody));
                }
            }
            catch (JsonException)
            {
                // unable to map error response, throwing generic error
            }
            throw new TurquoisehealthApiApiException(
                $"Error with status code {response.StatusCode}",
                response.StatusCode,
                responseBody
            );
        }
    }

    /// <summary>
    /// Fetch a single price by its ID (the id returned by /query). `expand=line_items` attaches the package composition — this replaces v2's provider-breakdown endpoint.
    /// </summary>
    /// <example><code>
    /// await client.ConsumerPricing.V3GetPriceAsync(
    ///     new V3GetPriceRequest { PriceId = "prc_2751.RA007.8361580493441765265" }
    /// );
    /// </code></example>
    public async Task<V3ProviderPackagePrice> V3GetPriceAsync(
        V3GetPriceRequest request,
        RequestOptions? options = null,
        CancellationToken cancellationToken = default
    )
    {
        var _query = new Dictionary<string, object>();
        if (request.Expand != null)
        {
            _query["expand"] = JsonUtils.Serialize(request.Expand);
        }
        var response = await _client
            .SendRequestAsync(
                new JsonRequest
                {
                    BaseUrl = _client.Options.BaseUrl,
                    Method = HttpMethod.Get,
                    Path = string.Format(
                        "v3/prices/{0}",
                        ValueConvert.ToPathParameterString(request.PriceId)
                    ),
                    Query = _query,
                    Options = options,
                },
                cancellationToken
            )
            .ConfigureAwait(false);
        if (response.StatusCode is >= 200 and < 400)
        {
            var responseBody = await response.Raw.Content.ReadAsStringAsync();
            try
            {
                return JsonUtils.Deserialize<V3ProviderPackagePrice>(responseBody)!;
            }
            catch (JsonException e)
            {
                throw new TurquoisehealthApiException("Failed to deserialize response", e);
            }
        }

        {
            var responseBody = await response.Raw.Content.ReadAsStringAsync();
            try
            {
                switch (response.StatusCode)
                {
                    case 400:
                        throw new BadRequestError(JsonUtils.Deserialize<object>(responseBody));
                    case 404:
                        throw new NotFoundError(
                            JsonUtils.Deserialize<V3ErrorResponse>(responseBody)
                        );
                    case 422:
                        throw new UnprocessableEntityError(
                            JsonUtils.Deserialize<object>(responseBody)
                        );
                    case 429:
                        throw new TooManyRequestsError(JsonUtils.Deserialize<object>(responseBody));
                    case 500:
                        throw new InternalServerError(JsonUtils.Deserialize<object>(responseBody));
                }
            }
            catch (JsonException)
            {
                // unable to map error response, throwing generic error
            }
            throw new TurquoisehealthApiApiException(
                $"Error with status code {response.StatusCode}",
                response.StatusCode,
                responseBody
            );
        }
    }

    /// <summary>
    /// Fetch provider types
    /// </summary>
    /// <example><code>
    /// await client.ConsumerPricing.V3GetProviderTypesAsync();
    /// </code></example>
    public async Task<IEnumerable<string>> V3GetProviderTypesAsync(
        RequestOptions? options = null,
        CancellationToken cancellationToken = default
    )
    {
        var response = await _client
            .SendRequestAsync(
                new JsonRequest
                {
                    BaseUrl = _client.Options.BaseUrl,
                    Method = HttpMethod.Get,
                    Path = "v3/providers/types",
                    Options = options,
                },
                cancellationToken
            )
            .ConfigureAwait(false);
        if (response.StatusCode is >= 200 and < 400)
        {
            var responseBody = await response.Raw.Content.ReadAsStringAsync();
            try
            {
                return JsonUtils.Deserialize<IEnumerable<string>>(responseBody)!;
            }
            catch (JsonException e)
            {
                throw new TurquoisehealthApiException("Failed to deserialize response", e);
            }
        }

        {
            var responseBody = await response.Raw.Content.ReadAsStringAsync();
            try
            {
                switch (response.StatusCode)
                {
                    case 400:
                        throw new BadRequestError(JsonUtils.Deserialize<object>(responseBody));
                    case 404:
                        throw new NotFoundError(
                            JsonUtils.Deserialize<V3ErrorResponse>(responseBody)
                        );
                    case 422:
                        throw new UnprocessableEntityError(
                            JsonUtils.Deserialize<object>(responseBody)
                        );
                    case 429:
                        throw new TooManyRequestsError(JsonUtils.Deserialize<object>(responseBody));
                    case 500:
                        throw new InternalServerError(JsonUtils.Deserialize<object>(responseBody));
                }
            }
            catch (JsonException)
            {
                // unable to map error response, throwing generic error
            }
            throw new TurquoisehealthApiApiException(
                $"Error with status code {response.StatusCode}",
                response.StatusCode,
                responseBody
            );
        }
    }

    /// <summary>
    /// List providers, filterable by name (case-insensitive substring), npi/type (exact), relationships (package_id/network_id/payer_id — providers with at least one matching price; combined relationship filters must be satisfied by the same price row, so results are always fulfillable via GET /v3/prices), and one location mode: `location.near.*` (ranked by distance), `location.within.*` (state/cbsa/zip_codes), or `location.zip` (ZIP centroid + default 25km radius). Results reflect providers Turquoise has priced services for.
    /// </summary>
    /// <example><code>
    /// await client.ConsumerPricing.V3ListProvidersAsync(new V3ListProvidersRequest());
    /// </code></example>
    public async Task<V3ListEnvelopeProvider> V3ListProvidersAsync(
        V3ListProvidersRequest request,
        RequestOptions? options = null,
        CancellationToken cancellationToken = default
    )
    {
        var _query = new Dictionary<string, object>();
        if (request.Name != null)
        {
            _query["name"] = request.Name;
        }
        if (request.Npi != null)
        {
            _query["npi"] = request.Npi;
        }
        if (request.Type != null)
        {
            _query["type"] = request.Type;
        }
        if (request.PackageId != null)
        {
            _query["package_id"] = request.PackageId;
        }
        if (request.NetworkId != null)
        {
            _query["network_id"] = request.NetworkId;
        }
        if (request.PayerId != null)
        {
            _query["payer_id"] = request.PayerId;
        }
        if (request.Search != null)
        {
            _query["search"] = request.Search;
        }
        if (request.MinScore != null)
        {
            _query["min_score"] = request.MinScore.Value.ToString();
        }
        if (request.PageSize != null)
        {
            _query["page_size"] = request.PageSize.Value.ToString();
        }
        if (request.Cursor != null)
        {
            _query["cursor"] = request.Cursor;
        }
        if (request.LocationNearLat != null)
        {
            _query["location.near.lat"] = request.LocationNearLat.Value.ToString();
        }
        if (request.LocationNearLng != null)
        {
            _query["location.near.lng"] = request.LocationNearLng.Value.ToString();
        }
        if (request.LocationNearRadiusM != null)
        {
            _query["location.near.radius_m"] = request.LocationNearRadiusM.Value.ToString();
        }
        if (request.LocationWithinState != null)
        {
            _query["location.within.state"] = request.LocationWithinState;
        }
        if (request.LocationWithinCbsa != null)
        {
            _query["location.within.cbsa"] = request.LocationWithinCbsa;
        }
        if (request.LocationWithinZipCodes != null)
        {
            _query["location.within.zip_codes"] = request.LocationWithinZipCodes;
        }
        if (request.LocationZip != null)
        {
            _query["location.zip"] = request.LocationZip;
        }
        var response = await _client
            .SendRequestAsync(
                new JsonRequest
                {
                    BaseUrl = _client.Options.BaseUrl,
                    Method = HttpMethod.Get,
                    Path = "v3/providers",
                    Query = _query,
                    Options = options,
                },
                cancellationToken
            )
            .ConfigureAwait(false);
        if (response.StatusCode is >= 200 and < 400)
        {
            var responseBody = await response.Raw.Content.ReadAsStringAsync();
            try
            {
                return JsonUtils.Deserialize<V3ListEnvelopeProvider>(responseBody)!;
            }
            catch (JsonException e)
            {
                throw new TurquoisehealthApiException("Failed to deserialize response", e);
            }
        }

        {
            var responseBody = await response.Raw.Content.ReadAsStringAsync();
            try
            {
                switch (response.StatusCode)
                {
                    case 400:
                        throw new BadRequestError(JsonUtils.Deserialize<object>(responseBody));
                    case 404:
                        throw new NotFoundError(
                            JsonUtils.Deserialize<V3ErrorResponse>(responseBody)
                        );
                    case 422:
                        throw new UnprocessableEntityError(
                            JsonUtils.Deserialize<object>(responseBody)
                        );
                    case 429:
                        throw new TooManyRequestsError(JsonUtils.Deserialize<object>(responseBody));
                    case 500:
                        throw new InternalServerError(JsonUtils.Deserialize<object>(responseBody));
                }
            }
            catch (JsonException)
            {
                // unable to map error response, throwing generic error
            }
            throw new TurquoisehealthApiApiException(
                $"Error with status code {response.StatusCode}",
                response.StatusCode,
                responseBody
            );
        }
    }

    /// <summary>
    /// Fetch a single provider by ID. Missing IDs return a 404 with the standard error body.
    /// </summary>
    /// <example><code>
    /// await client.ConsumerPricing.V3GetProviderAsync(new V3GetProviderRequest { ProviderId = "2743" });
    /// </code></example>
    public async Task<V3Provider> V3GetProviderAsync(
        V3GetProviderRequest request,
        RequestOptions? options = null,
        CancellationToken cancellationToken = default
    )
    {
        var response = await _client
            .SendRequestAsync(
                new JsonRequest
                {
                    BaseUrl = _client.Options.BaseUrl,
                    Method = HttpMethod.Get,
                    Path = string.Format(
                        "v3/providers/{0}",
                        ValueConvert.ToPathParameterString(request.ProviderId)
                    ),
                    Options = options,
                },
                cancellationToken
            )
            .ConfigureAwait(false);
        if (response.StatusCode is >= 200 and < 400)
        {
            var responseBody = await response.Raw.Content.ReadAsStringAsync();
            try
            {
                return JsonUtils.Deserialize<V3Provider>(responseBody)!;
            }
            catch (JsonException e)
            {
                throw new TurquoisehealthApiException("Failed to deserialize response", e);
            }
        }

        {
            var responseBody = await response.Raw.Content.ReadAsStringAsync();
            try
            {
                switch (response.StatusCode)
                {
                    case 400:
                        throw new BadRequestError(JsonUtils.Deserialize<object>(responseBody));
                    case 404:
                        throw new NotFoundError(
                            JsonUtils.Deserialize<V3ErrorResponse>(responseBody)
                        );
                    case 422:
                        throw new UnprocessableEntityError(
                            JsonUtils.Deserialize<object>(responseBody)
                        );
                    case 429:
                        throw new TooManyRequestsError(JsonUtils.Deserialize<object>(responseBody));
                    case 500:
                        throw new InternalServerError(JsonUtils.Deserialize<object>(responseBody));
                }
            }
            catch (JsonException)
            {
                // unable to map error response, throwing generic error
            }
            throw new TurquoisehealthApiApiException(
                $"Error with status code {response.StatusCode}",
                response.StatusCode,
                responseBody
            );
        }
    }
}
