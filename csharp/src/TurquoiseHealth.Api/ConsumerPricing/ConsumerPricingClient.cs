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
    /// Retrieve a list of providers, filtered over a search by name, NPI, provider type, or location. Additionally, filter to a list of providers with Turquoise price estimates available by a selected payer, network, or package. Results reflect provider, payer, and package combinations that Turquoise has priced services for; the list may not be comprehensive of all contacted payer networks or services that are available.
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
                throw new TurquoiseHealthApiClientException("Failed to deserialize response", e);
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
                            JsonUtils.Deserialize<V3ErrorResponse>(responseBody)
                        );
                    case 404:
                        throw new NotFoundError(
                            JsonUtils.Deserialize<V3ErrorResponse>(responseBody)
                        );
                    case 422:
                        throw new UnprocessableEntityError(
                            JsonUtils.Deserialize<V3ErrorResponse>(responseBody)
                        );
                    case 429:
                        throw new TooManyRequestsError(
                            JsonUtils.Deserialize<V3ErrorResponse>(responseBody)
                        );
                    case 500:
                        throw new InternalServerError(
                            JsonUtils.Deserialize<V3ErrorResponse>(responseBody)
                        );
                }
            }
            catch (JsonException)
            {
                // unable to map error response, throwing generic error
            }
            throw new TurquoiseHealthApiClientApiException(
                $"Error with status code {response.StatusCode}",
                response.StatusCode,
                responseBody
            );
        }
    }

    /// <summary>
    /// Fetch provider types. Results return valid inputs to filter by provider types across the API.
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
                throw new TurquoiseHealthApiClientException("Failed to deserialize response", e);
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
                            JsonUtils.Deserialize<V3ErrorResponse>(responseBody)
                        );
                    case 404:
                        throw new NotFoundError(
                            JsonUtils.Deserialize<V3ErrorResponse>(responseBody)
                        );
                    case 422:
                        throw new UnprocessableEntityError(
                            JsonUtils.Deserialize<V3ErrorResponse>(responseBody)
                        );
                    case 429:
                        throw new TooManyRequestsError(
                            JsonUtils.Deserialize<V3ErrorResponse>(responseBody)
                        );
                    case 500:
                        throw new InternalServerError(
                            JsonUtils.Deserialize<V3ErrorResponse>(responseBody)
                        );
                }
            }
            catch (JsonException)
            {
                // unable to map error response, throwing generic error
            }
            throw new TurquoiseHealthApiClientApiException(
                $"Error with status code {response.StatusCode}",
                response.StatusCode,
                responseBody
            );
        }
    }

    /// <summary>
    /// Fetch details for a single provider by provider ID.
    /// </summary>
    /// <example><code>
    /// await client.ConsumerPricing.V3GetProviderAsync(new V3GetProviderRequest { ProviderId = "5756" });
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
                throw new TurquoiseHealthApiClientException("Failed to deserialize response", e);
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
                            JsonUtils.Deserialize<V3ErrorResponse>(responseBody)
                        );
                    case 404:
                        throw new NotFoundError(
                            JsonUtils.Deserialize<V3ErrorResponse>(responseBody)
                        );
                    case 422:
                        throw new UnprocessableEntityError(
                            JsonUtils.Deserialize<V3ErrorResponse>(responseBody)
                        );
                    case 429:
                        throw new TooManyRequestsError(
                            JsonUtils.Deserialize<V3ErrorResponse>(responseBody)
                        );
                    case 500:
                        throw new InternalServerError(
                            JsonUtils.Deserialize<V3ErrorResponse>(responseBody)
                        );
                }
            }
            catch (JsonException)
            {
                // unable to map error response, throwing generic error
            }
            throw new TurquoiseHealthApiClientApiException(
                $"Error with status code {response.StatusCode}",
                response.StatusCode,
                responseBody
            );
        }
    }

    /// <summary>
    /// Retrieve a list of payers, filtered over a search by name, location for care, or specific providers and services priced in the Turquoise data. Location scopes to payers with at least one price at an in-area provider. Results reflect provider, payer, and package combinations that Turquoise has priced services for; the list may not be comprehensive of all contracted providers or services that are available.
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
                throw new TurquoiseHealthApiClientException("Failed to deserialize response", e);
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
                            JsonUtils.Deserialize<V3ErrorResponse>(responseBody)
                        );
                    case 404:
                        throw new NotFoundError(
                            JsonUtils.Deserialize<V3ErrorResponse>(responseBody)
                        );
                    case 422:
                        throw new UnprocessableEntityError(
                            JsonUtils.Deserialize<V3ErrorResponse>(responseBody)
                        );
                    case 429:
                        throw new TooManyRequestsError(
                            JsonUtils.Deserialize<V3ErrorResponse>(responseBody)
                        );
                    case 500:
                        throw new InternalServerError(
                            JsonUtils.Deserialize<V3ErrorResponse>(responseBody)
                        );
                }
            }
            catch (JsonException)
            {
                // unable to map error response, throwing generic error
            }
            throw new TurquoiseHealthApiClientApiException(
                $"Error with status code {response.StatusCode}",
                response.StatusCode,
                responseBody
            );
        }
    }

    /// <summary>
    /// Fetch details about a single payer by payer ID.
    /// </summary>
    /// <example><code>
    /// await client.ConsumerPricing.V3GetPayerAsync(new V3GetPayerRequest { PayerId = "76" });
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
                throw new TurquoiseHealthApiClientException("Failed to deserialize response", e);
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
                            JsonUtils.Deserialize<V3ErrorResponse>(responseBody)
                        );
                    case 404:
                        throw new NotFoundError(
                            JsonUtils.Deserialize<V3ErrorResponse>(responseBody)
                        );
                    case 422:
                        throw new UnprocessableEntityError(
                            JsonUtils.Deserialize<V3ErrorResponse>(responseBody)
                        );
                    case 429:
                        throw new TooManyRequestsError(
                            JsonUtils.Deserialize<V3ErrorResponse>(responseBody)
                        );
                    case 500:
                        throw new InternalServerError(
                            JsonUtils.Deserialize<V3ErrorResponse>(responseBody)
                        );
                }
            }
            catch (JsonException)
            {
                // unable to map error response, throwing generic error
            }
            throw new TurquoiseHealthApiClientApiException(
                $"Error with status code {response.StatusCode}",
                response.StatusCode,
                responseBody
            );
        }
    }

    /// <summary>
    /// Retrieve a list of payer networks, filtered over a search by name, payer organization (e.g., Cigna), location for care, or specific providers and services priced in the Turquoise data. Location scopes to payer networks with at least one price at an in-area provider. Results reflect provider, payer network, and package combinations that Turquoise has priced services for; the list may not be comprehensive of all contracted providers or services that are available.
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
                throw new TurquoiseHealthApiClientException("Failed to deserialize response", e);
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
                            JsonUtils.Deserialize<V3ErrorResponse>(responseBody)
                        );
                    case 404:
                        throw new NotFoundError(
                            JsonUtils.Deserialize<V3ErrorResponse>(responseBody)
                        );
                    case 422:
                        throw new UnprocessableEntityError(
                            JsonUtils.Deserialize<V3ErrorResponse>(responseBody)
                        );
                    case 429:
                        throw new TooManyRequestsError(
                            JsonUtils.Deserialize<V3ErrorResponse>(responseBody)
                        );
                    case 500:
                        throw new InternalServerError(
                            JsonUtils.Deserialize<V3ErrorResponse>(responseBody)
                        );
                }
            }
            catch (JsonException)
            {
                // unable to map error response, throwing generic error
            }
            throw new TurquoiseHealthApiClientApiException(
                $"Error with status code {response.StatusCode}",
                response.StatusCode,
                responseBody
            );
        }
    }

    /// <summary>
    /// Fetch details about a single network by network ID.
    /// </summary>
    /// <example><code>
    /// await client.ConsumerPricing.V3GetNetworkAsync(
    ///     new V3GetNetworkRequest { NetworkId = "-3776001016975145508" }
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
                throw new TurquoiseHealthApiClientException("Failed to deserialize response", e);
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
                            JsonUtils.Deserialize<V3ErrorResponse>(responseBody)
                        );
                    case 404:
                        throw new NotFoundError(
                            JsonUtils.Deserialize<V3ErrorResponse>(responseBody)
                        );
                    case 422:
                        throw new UnprocessableEntityError(
                            JsonUtils.Deserialize<V3ErrorResponse>(responseBody)
                        );
                    case 429:
                        throw new TooManyRequestsError(
                            JsonUtils.Deserialize<V3ErrorResponse>(responseBody)
                        );
                    case 500:
                        throw new InternalServerError(
                            JsonUtils.Deserialize<V3ErrorResponse>(responseBody)
                        );
                }
            }
            catch (JsonException)
            {
                // unable to map error response, throwing generic error
            }
            throw new TurquoiseHealthApiClientApiException(
                $"Error with status code {response.StatusCode}",
                response.StatusCode,
                responseBody
            );
        }
    }

    /// <summary>
    /// Return a list of service packages, filtered over name or anchor code. Additionally, filter to a list of packages with Turquoise price estimates available by a selected payer, network, or provider. Results reflect provider, payer, and package combinations that Turquoise has priced services for; the list may not be comprehensive of all payer networks or providers that support this service. When multiple relationship filters are combined, they must be satisfied by the same price row, so results are always fulfillable via GET /v3/prices.
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
                throw new TurquoiseHealthApiClientException("Failed to deserialize response", e);
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
                            JsonUtils.Deserialize<V3ErrorResponse>(responseBody)
                        );
                    case 404:
                        throw new NotFoundError(
                            JsonUtils.Deserialize<V3ErrorResponse>(responseBody)
                        );
                    case 422:
                        throw new UnprocessableEntityError(
                            JsonUtils.Deserialize<V3ErrorResponse>(responseBody)
                        );
                    case 429:
                        throw new TooManyRequestsError(
                            JsonUtils.Deserialize<V3ErrorResponse>(responseBody)
                        );
                    case 500:
                        throw new InternalServerError(
                            JsonUtils.Deserialize<V3ErrorResponse>(responseBody)
                        );
                }
            }
            catch (JsonException)
            {
                // unable to map error response, throwing generic error
            }
            throw new TurquoiseHealthApiClientApiException(
                $"Error with status code {response.StatusCode}",
                response.StatusCode,
                responseBody
            );
        }
    }

    /// <summary>
    /// Fetch package details by package ID.
    /// </summary>
    /// <example><code>
    /// await client.ConsumerPricing.V3GetPackageAsync(new V3GetPackageRequest { PackageId = "OB002" });
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
                throw new TurquoiseHealthApiClientException("Failed to deserialize response", e);
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
                            JsonUtils.Deserialize<V3ErrorResponse>(responseBody)
                        );
                    case 404:
                        throw new NotFoundError(
                            JsonUtils.Deserialize<V3ErrorResponse>(responseBody)
                        );
                    case 422:
                        throw new UnprocessableEntityError(
                            JsonUtils.Deserialize<V3ErrorResponse>(responseBody)
                        );
                    case 429:
                        throw new TooManyRequestsError(
                            JsonUtils.Deserialize<V3ErrorResponse>(responseBody)
                        );
                    case 500:
                        throw new InternalServerError(
                            JsonUtils.Deserialize<V3ErrorResponse>(responseBody)
                        );
                }
            }
            catch (JsonException)
            {
                // unable to map error response, throwing generic error
            }
            throw new TurquoiseHealthApiClientApiException(
                $"Error with status code {response.StatusCode}",
                response.StatusCode,
                responseBody
            );
        }
    }

    /// <summary>
    /// Review a service package's composition. This includes common codes and fee types, and association rates across variations of packages for a given service.
    /// </summary>
    /// <example><code>
    /// await client.ConsumerPricing.V3ListPackageLineItemsAsync(
    ///     new V3ListPackageLineItemsRequest { PackageId = "OB002" }
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
                throw new TurquoiseHealthApiClientException("Failed to deserialize response", e);
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
                            JsonUtils.Deserialize<V3ErrorResponse>(responseBody)
                        );
                    case 404:
                        throw new NotFoundError(
                            JsonUtils.Deserialize<V3ErrorResponse>(responseBody)
                        );
                    case 422:
                        throw new UnprocessableEntityError(
                            JsonUtils.Deserialize<V3ErrorResponse>(responseBody)
                        );
                    case 429:
                        throw new TooManyRequestsError(
                            JsonUtils.Deserialize<V3ErrorResponse>(responseBody)
                        );
                    case 500:
                        throw new InternalServerError(
                            JsonUtils.Deserialize<V3ErrorResponse>(responseBody)
                        );
                }
            }
            catch (JsonException)
            {
                // unable to map error response, throwing generic error
            }
            throw new TurquoiseHealthApiClientApiException(
                $"Error with status code {response.StatusCode}",
                response.StatusCode,
                responseBody
            );
        }
    }

    /// <summary>
    /// Return summary statistics (min, max, average, median, quartiles) over prices matching the same filters as /query, excluding sorting and pagination. Use this endpoint to compare prices across relevant criteria.
    /// </summary>
    /// <example><code>
    /// await client.ConsumerPricing.V3ComparePricesAsync(
    ///     new V3PricesCompareRequest
    ///     {
    ///         PackageId = "OB002",
    ///         ProviderId = "5756",
    ///         Pricing = new V3PricesCompareRequestPricing(
    ///             new V3PricesCompareRequestPricing.Negotiated(
    ///                 new V3PricingNegotiated { NetworkId = "-3776001016975145508" }
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
                throw new TurquoiseHealthApiClientException("Failed to deserialize response", e);
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
                            JsonUtils.Deserialize<V3ErrorResponse>(responseBody)
                        );
                    case 404:
                        throw new NotFoundError(
                            JsonUtils.Deserialize<V3ErrorResponse>(responseBody)
                        );
                    case 422:
                        throw new UnprocessableEntityError(
                            JsonUtils.Deserialize<V3ErrorResponse>(responseBody)
                        );
                    case 429:
                        throw new TooManyRequestsError(
                            JsonUtils.Deserialize<V3ErrorResponse>(responseBody)
                        );
                    case 500:
                        throw new InternalServerError(
                            JsonUtils.Deserialize<V3ErrorResponse>(responseBody)
                        );
                }
            }
            catch (JsonException)
            {
                // unable to map error response, throwing generic error
            }
            throw new TurquoiseHealthApiClientApiException(
                $"Error with status code {response.StatusCode}",
                response.StatusCode,
                responseBody
            );
        }
    }

    /// <summary>
    /// Return prices for valid provider, service package, and pricing arrangement (cash or negotiated) combinations.
    /// </summary>
    /// <example><code>
    /// await client.ConsumerPricing.V3QueryPricesAsync(
    ///     new V3PricesQueryRequest
    ///     {
    ///         PackageId = "OB002",
    ///         ProviderId = "5756",
    ///         Pricing = new V3PricesQueryRequestPricing(
    ///             new V3PricesQueryRequestPricing.Negotiated(
    ///                 new V3PricingNegotiated { NetworkId = "-3776001016975145508" }
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
                throw new TurquoiseHealthApiClientException("Failed to deserialize response", e);
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
                            JsonUtils.Deserialize<V3ErrorResponse>(responseBody)
                        );
                    case 404:
                        throw new NotFoundError(
                            JsonUtils.Deserialize<V3ErrorResponse>(responseBody)
                        );
                    case 422:
                        throw new UnprocessableEntityError(
                            JsonUtils.Deserialize<V3ErrorResponse>(responseBody)
                        );
                    case 429:
                        throw new TooManyRequestsError(
                            JsonUtils.Deserialize<V3ErrorResponse>(responseBody)
                        );
                    case 500:
                        throw new InternalServerError(
                            JsonUtils.Deserialize<V3ErrorResponse>(responseBody)
                        );
                }
            }
            catch (JsonException)
            {
                // unable to map error response, throwing generic error
            }
            throw new TurquoiseHealthApiClientApiException(
                $"Error with status code {response.StatusCode}",
                response.StatusCode,
                responseBody
            );
        }
    }

    /// <summary>
    /// Fetch a single price by its unique price ID. Expand line items to review the priced package composition.
    /// </summary>
    /// <example><code>
    /// await client.ConsumerPricing.V3GetPriceAsync(
    ///     new V3GetPriceRequest { PriceId = "prc_5756.OB002.-3776001016975145508" }
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
                throw new TurquoiseHealthApiClientException("Failed to deserialize response", e);
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
                            JsonUtils.Deserialize<V3ErrorResponse>(responseBody)
                        );
                    case 404:
                        throw new NotFoundError(
                            JsonUtils.Deserialize<V3ErrorResponse>(responseBody)
                        );
                    case 422:
                        throw new UnprocessableEntityError(
                            JsonUtils.Deserialize<V3ErrorResponse>(responseBody)
                        );
                    case 429:
                        throw new TooManyRequestsError(
                            JsonUtils.Deserialize<V3ErrorResponse>(responseBody)
                        );
                    case 500:
                        throw new InternalServerError(
                            JsonUtils.Deserialize<V3ErrorResponse>(responseBody)
                        );
                }
            }
            catch (JsonException)
            {
                // unable to map error response, throwing generic error
            }
            throw new TurquoiseHealthApiClientApiException(
                $"Error with status code {response.StatusCode}",
                response.StatusCode,
                responseBody
            );
        }
    }
}
