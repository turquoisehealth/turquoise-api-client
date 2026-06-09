using System.Text.Json;
using OneOf;
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
    /// Discover available service/surgery packages (SSPs). Supports optional name filtering and pagination.
    /// </summary>
    /// <example><code>
    /// await client.ConsumerPricing.GetSspsAsync(new GetSspsRequest());
    /// </code></example>
    public async Task<ServicePackagePage> GetSspsAsync(
        GetSspsRequest request,
        RequestOptions? options = null,
        CancellationToken cancellationToken = default
    )
    {
        var _query = new Dictionary<string, object>();
        if (request.SspName != null)
        {
            _query["ssp_name"] = request.SspName;
        }
        if (request.SspDescription != null)
        {
            _query["ssp_description"] = request.SspDescription;
        }
        if (request.Search != null)
        {
            _query["search"] = request.Search;
        }
        if (request.Page != null)
        {
            _query["page"] = request.Page.Value.ToString();
        }
        if (request.PageSize != null)
        {
            _query["page_size"] = request.PageSize.Value.ToString();
        }
        var response = await _client
            .SendRequestAsync(
                new JsonRequest
                {
                    BaseUrl = _client.Options.BaseUrl,
                    Method = HttpMethod.Get,
                    Path = "v1/consumer-pricing/ssps",
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
                return JsonUtils.Deserialize<ServicePackagePage>(responseBody)!;
            }
            catch (JsonException e)
            {
                throw new TurquoiseHealthApiException("Failed to deserialize response", e);
            }
        }

        {
            var responseBody = await response.Raw.Content.ReadAsStringAsync();
            try
            {
                switch (response.StatusCode)
                {
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
            throw new TurquoiseHealthApiApiException(
                $"Error with status code {response.StatusCode}",
                response.StatusCode,
                responseBody
            );
        }
    }

    /// <summary>
    /// Discover insurance networks, optionally filtered by SSP or payer name.
    /// </summary>
    /// <example><code>
    /// await client.ConsumerPricing.GetInsuranceNetworksAsync(new GetInsuranceNetworksRequest());
    /// </code></example>
    public async Task<InsuranceNetworkPage> GetInsuranceNetworksAsync(
        GetInsuranceNetworksRequest request,
        RequestOptions? options = null,
        CancellationToken cancellationToken = default
    )
    {
        var _query = new Dictionary<string, object>();
        if (request.SspId != null)
        {
            _query["ssp_id"] = request.SspId;
        }
        if (request.PayerName != null)
        {
            _query["payer_name"] = request.PayerName;
        }
        if (request.Page != null)
        {
            _query["page"] = request.Page.Value.ToString();
        }
        if (request.PageSize != null)
        {
            _query["page_size"] = request.PageSize.Value.ToString();
        }
        var response = await _client
            .SendRequestAsync(
                new JsonRequest
                {
                    BaseUrl = _client.Options.BaseUrl,
                    Method = HttpMethod.Get,
                    Path = "v1/consumer-pricing/networks",
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
                return JsonUtils.Deserialize<InsuranceNetworkPage>(responseBody)!;
            }
            catch (JsonException e)
            {
                throw new TurquoiseHealthApiException("Failed to deserialize response", e);
            }
        }

        {
            var responseBody = await response.Raw.Content.ReadAsStringAsync();
            try
            {
                switch (response.StatusCode)
                {
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
            throw new TurquoiseHealthApiApiException(
                $"Error with status code {response.StatusCode}",
                response.StatusCode,
                responseBody
            );
        }
    }

    /// <summary>
    /// Search for providers by name, NPI, or location.
    /// </summary>
    /// <example><code>
    /// await client.ConsumerPricing.GetProvidersAsync(new GetProvidersRequest());
    /// </code></example>
    public async Task<ProviderPage> GetProvidersAsync(
        GetProvidersRequest request,
        RequestOptions? options = null,
        CancellationToken cancellationToken = default
    )
    {
        var _query = new Dictionary<string, object>();
        if (request.ProviderName != null)
        {
            _query["provider_name"] = request.ProviderName;
        }
        if (request.Npi != null)
        {
            _query["npi"] = request.Npi;
        }
        if (request.City != null)
        {
            _query["city"] = request.City;
        }
        if (request.State != null)
        {
            _query["state"] = request.State;
        }
        if (request.ZipCode != null)
        {
            _query["zip_code"] = request.ZipCode;
        }
        if (request.Page != null)
        {
            _query["page"] = request.Page.Value.ToString();
        }
        if (request.PageSize != null)
        {
            _query["page_size"] = request.PageSize.Value.ToString();
        }
        var response = await _client
            .SendRequestAsync(
                new JsonRequest
                {
                    BaseUrl = _client.Options.BaseUrl,
                    Method = HttpMethod.Get,
                    Path = "v1/consumer-pricing/providers",
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
                return JsonUtils.Deserialize<ProviderPage>(responseBody)!;
            }
            catch (JsonException e)
            {
                throw new TurquoiseHealthApiException("Failed to deserialize response", e);
            }
        }

        {
            var responseBody = await response.Raw.Content.ReadAsStringAsync();
            try
            {
                switch (response.StatusCode)
                {
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
            throw new TurquoiseHealthApiApiException(
                $"Error with status code {response.StatusCode}",
                response.StatusCode,
                responseBody
            );
        }
    }

    /// <summary>
    /// List of providers that can satisfy a given SSP, with the total expected price, in a given geographic area. Requests support location filtering via ZIP code, CBSA, state, or coordinates. When no Network ID is provided, cash prices are shown.
    /// </summary>
    /// <example><code>
    /// await client.ConsumerPricing.GetSspPricesAsync(
    ///     new PackagePricesRequest
    ///     {
    ///         SspId = "DE000",
    ///         Location = new LocationInput
    ///         {
    ///             GeoSpace = new GeoSpace { ZipCodes = new List&lt;string&gt;() { "80129" } },
    ///         },
    ///         NetworkId = "-7695283351826393948",
    ///         SortBy = CareNavSortType.PriceAsc,
    ///     }
    /// );
    /// </code></example>
    public async Task<SspPricePage> GetSspPricesAsync(
        PackagePricesRequest request,
        RequestOptions? options = null,
        CancellationToken cancellationToken = default
    )
    {
        var _query = new Dictionary<string, object>();
        if (request.Page != null)
        {
            _query["page"] = request.Page.Value.ToString();
        }
        if (request.PageSize != null)
        {
            _query["page_size"] = request.PageSize.Value.ToString();
        }
        var response = await _client
            .SendRequestAsync(
                new JsonRequest
                {
                    BaseUrl = _client.Options.BaseUrl,
                    Method = HttpMethod.Post,
                    Path = string.Format(
                        "v1/consumer-pricing/ssps/{0}/prices",
                        ValueConvert.ToPathParameterString(request.SspId)
                    ),
                    Body = request,
                    Query = _query,
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
                return JsonUtils.Deserialize<SspPricePage>(responseBody)!;
            }
            catch (JsonException e)
            {
                throw new TurquoiseHealthApiException("Failed to deserialize response", e);
            }
        }

        {
            var responseBody = await response.Raw.Content.ReadAsStringAsync();
            try
            {
                switch (response.StatusCode)
                {
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
            throw new TurquoiseHealthApiApiException(
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
    /// await client.ConsumerPricing.GetProviderSspPricesAsync(
    ///     new ProviderPackageBreakdownRequest { SspId = "DE000", ProviderId = "provider_id" }
    /// );
    /// </code></example>
    public async Task<OneOf<ProviderBreakdown, NoDataResponse>> GetProviderSspPricesAsync(
        ProviderPackageBreakdownRequest request,
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
                    Path = string.Format(
                        "v1/consumer-pricing/ssps/{0}/providers/{1}/prices",
                        ValueConvert.ToPathParameterString(request.SspId),
                        ValueConvert.ToPathParameterString(request.ProviderId)
                    ),
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
                return JsonUtils.Deserialize<OneOf<ProviderBreakdown, NoDataResponse>>(
                    responseBody
                )!;
            }
            catch (JsonException e)
            {
                throw new TurquoiseHealthApiException("Failed to deserialize response", e);
            }
        }

        {
            var responseBody = await response.Raw.Content.ReadAsStringAsync();
            try
            {
                switch (response.StatusCode)
                {
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
            throw new TurquoiseHealthApiApiException(
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
    /// await client.ConsumerPricing.CompareSspPricesAsync(
    ///     new PriceComparisonRequest
    ///     {
    ///         SspId = "DE000",
    ///         Location = new LocationInput { GeoSpace = new GeoSpace { State = "CO" } },
    ///         NetworkId = "-7695283351826393948",
    ///     }
    /// );
    /// </code></example>
    public async Task<OneOf<PricesComparison, NoDataResponse>> CompareSspPricesAsync(
        PriceComparisonRequest request,
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
                    Path = string.Format(
                        "v1/consumer-pricing/ssps/{0}/price-comparison",
                        ValueConvert.ToPathParameterString(request.SspId)
                    ),
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
                return JsonUtils.Deserialize<OneOf<PricesComparison, NoDataResponse>>(
                    responseBody
                )!;
            }
            catch (JsonException e)
            {
                throw new TurquoiseHealthApiException("Failed to deserialize response", e);
            }
        }

        {
            var responseBody = await response.Raw.Content.ReadAsStringAsync();
            try
            {
                switch (response.StatusCode)
                {
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
            throw new TurquoiseHealthApiApiException(
                $"Error with status code {response.StatusCode}",
                response.StatusCode,
                responseBody
            );
        }
    }

    /// <summary>
    /// Discover available service/surgery packages (SSPs). Supports optional name filtering and pagination.
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
                throw new TurquoiseHealthApiException("Failed to deserialize response", e);
            }
        }

        {
            var responseBody = await response.Raw.Content.ReadAsStringAsync();
            try
            {
                switch (response.StatusCode)
                {
                    case 400:
                        throw new BadRequestError(
                            JsonUtils.Deserialize<ErrorResponse>(responseBody)
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
            throw new TurquoiseHealthApiApiException(
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
                throw new TurquoiseHealthApiException("Failed to deserialize response", e);
            }
        }

        {
            var responseBody = await response.Raw.Content.ReadAsStringAsync();
            try
            {
                switch (response.StatusCode)
                {
                    case 400:
                        throw new BadRequestError(
                            JsonUtils.Deserialize<ErrorResponse>(responseBody)
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
            throw new TurquoiseHealthApiApiException(
                $"Error with status code {response.StatusCode}",
                response.StatusCode,
                responseBody
            );
        }
    }

    /// <summary>
    /// Search for providers by name, NPI, or location.
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
                throw new TurquoiseHealthApiException("Failed to deserialize response", e);
            }
        }

        {
            var responseBody = await response.Raw.Content.ReadAsStringAsync();
            try
            {
                switch (response.StatusCode)
                {
                    case 400:
                        throw new BadRequestError(
                            JsonUtils.Deserialize<ErrorResponse>(responseBody)
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
            throw new TurquoiseHealthApiApiException(
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
                throw new TurquoiseHealthApiException("Failed to deserialize response", e);
            }
        }

        {
            var responseBody = await response.Raw.Content.ReadAsStringAsync();
            try
            {
                switch (response.StatusCode)
                {
                    case 400:
                        throw new BadRequestError(
                            JsonUtils.Deserialize<ErrorResponse>(responseBody)
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
            throw new TurquoiseHealthApiApiException(
                $"Error with status code {response.StatusCode}",
                response.StatusCode,
                responseBody
            );
        }
    }

    /// <summary>
    /// Discover insurance networks, optionally filtered by network name, payer name, or payer ID.
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
                throw new TurquoiseHealthApiException("Failed to deserialize response", e);
            }
        }

        {
            var responseBody = await response.Raw.Content.ReadAsStringAsync();
            try
            {
                switch (response.StatusCode)
                {
                    case 400:
                        throw new BadRequestError(
                            JsonUtils.Deserialize<ErrorResponse>(responseBody)
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
            throw new TurquoiseHealthApiApiException(
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
                throw new TurquoiseHealthApiException("Failed to deserialize response", e);
            }
        }

        {
            var responseBody = await response.Raw.Content.ReadAsStringAsync();
            try
            {
                switch (response.StatusCode)
                {
                    case 400:
                        throw new BadRequestError(
                            JsonUtils.Deserialize<ErrorResponse>(responseBody)
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
            throw new TurquoiseHealthApiApiException(
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
    ///         RateType = PricesRequestRateType.Negotiated,
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
                throw new TurquoiseHealthApiException("Failed to deserialize response", e);
            }
        }

        {
            var responseBody = await response.Raw.Content.ReadAsStringAsync();
            try
            {
                switch (response.StatusCode)
                {
                    case 400:
                        throw new BadRequestError(
                            JsonUtils.Deserialize<ErrorResponse>(responseBody)
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
            throw new TurquoiseHealthApiApiException(
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
                throw new TurquoiseHealthApiException("Failed to deserialize response", e);
            }
        }

        {
            var responseBody = await response.Raw.Content.ReadAsStringAsync();
            try
            {
                switch (response.StatusCode)
                {
                    case 400:
                        throw new BadRequestError(
                            JsonUtils.Deserialize<ErrorResponse>(responseBody)
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
            throw new TurquoiseHealthApiApiException(
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
                throw new TurquoiseHealthApiException("Failed to deserialize response", e);
            }
        }

        {
            var responseBody = await response.Raw.Content.ReadAsStringAsync();
            try
            {
                switch (response.StatusCode)
                {
                    case 400:
                        throw new BadRequestError(
                            JsonUtils.Deserialize<ErrorResponse>(responseBody)
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
            throw new TurquoiseHealthApiApiException(
                $"Error with status code {response.StatusCode}",
                response.StatusCode,
                responseBody
            );
        }
    }
}
