using System.Text.Json.Serialization;
using TurquoiseHealth.Api.Core;

namespace TurquoiseHealth.Api;

[JsonConverter(typeof(StringEnumSerializer<ConsumerSitePersonalizedEstimateErrorCode>))]
[Serializable]
public readonly record struct ConsumerSitePersonalizedEstimateErrorCode : IStringEnum
{
    public static readonly ConsumerSitePersonalizedEstimateErrorCode InternalServerError = new(
        Values.InternalServerError
    );

    public static readonly ConsumerSitePersonalizedEstimateErrorCode EligibilityServiceError = new(
        Values.EligibilityServiceError
    );

    public static readonly ConsumerSitePersonalizedEstimateErrorCode UnsupportedTpa = new(
        Values.UnsupportedTpa
    );

    public static readonly ConsumerSitePersonalizedEstimateErrorCode UnavailablePayer = new(
        Values.UnavailablePayer
    );

    public static readonly ConsumerSitePersonalizedEstimateErrorCode InvalidDob = new(
        Values.InvalidDob
    );

    public static readonly ConsumerSitePersonalizedEstimateErrorCode InvalidMemberId = new(
        Values.InvalidMemberId
    );

    public static readonly ConsumerSitePersonalizedEstimateErrorCode InvalidMemberName = new(
        Values.InvalidMemberName
    );

    public static readonly ConsumerSitePersonalizedEstimateErrorCode MemberNotFound = new(
        Values.MemberNotFound
    );

    public static readonly ConsumerSitePersonalizedEstimateErrorCode NoActiveCoverage = new(
        Values.NoActiveCoverage
    );

    public static readonly ConsumerSitePersonalizedEstimateErrorCode PlanNotFound = new(
        Values.PlanNotFound
    );

    public static readonly ConsumerSitePersonalizedEstimateErrorCode PricingUnavailable = new(
        Values.PricingUnavailable
    );

    public static readonly ConsumerSitePersonalizedEstimateErrorCode InvalidJson = new(
        Values.InvalidJson
    );

    public static readonly ConsumerSitePersonalizedEstimateErrorCode Unauthorized = new(
        Values.Unauthorized
    );

    public static readonly ConsumerSitePersonalizedEstimateErrorCode Forbidden = new(
        Values.Forbidden
    );

    public static readonly ConsumerSitePersonalizedEstimateErrorCode AuthorizationUnavailable = new(
        Values.AuthorizationUnavailable
    );

    public static readonly ConsumerSitePersonalizedEstimateErrorCode InvalidRequest = new(
        Values.InvalidRequest
    );

    public static readonly ConsumerSitePersonalizedEstimateErrorCode InvalidIdFormat = new(
        Values.InvalidIdFormat
    );

    public static readonly ConsumerSitePersonalizedEstimateErrorCode InsufficientScope = new(
        Values.InsufficientScope
    );

    public static readonly ConsumerSitePersonalizedEstimateErrorCode LocationAmbiguous = new(
        Values.LocationAmbiguous
    );

    public static readonly ConsumerSitePersonalizedEstimateErrorCode UnresolvableLocation = new(
        Values.UnresolvableLocation
    );

    public static readonly ConsumerSitePersonalizedEstimateErrorCode NotFound = new(
        Values.NotFound
    );

    public static readonly ConsumerSitePersonalizedEstimateErrorCode InvalidExpand = new(
        Values.InvalidExpand
    );

    public static readonly ConsumerSitePersonalizedEstimateErrorCode PermissionDenied = new(
        Values.PermissionDenied
    );

    public static readonly ConsumerSitePersonalizedEstimateErrorCode RateLimited = new(
        Values.RateLimited
    );

    public static readonly ConsumerSitePersonalizedEstimateErrorCode SearchUnavailable = new(
        Values.SearchUnavailable
    );

    public static readonly ConsumerSitePersonalizedEstimateErrorCode ConflictingParameters = new(
        Values.ConflictingParameters
    );

    public ConsumerSitePersonalizedEstimateErrorCode(string value)
    {
        Value = value;
    }

    /// <summary>
    /// The string value of the enum.
    /// </summary>
    public string Value { get; }

    /// <summary>
    /// Create a string enum with the given value.
    /// </summary>
    public static ConsumerSitePersonalizedEstimateErrorCode FromCustom(string value)
    {
        return new ConsumerSitePersonalizedEstimateErrorCode(value);
    }

    public bool Equals(string? other)
    {
        return Value.Equals(other);
    }

    /// <summary>
    /// Returns the string value of the enum.
    /// </summary>
    public override string ToString()
    {
        return Value;
    }

    public static bool operator ==(
        ConsumerSitePersonalizedEstimateErrorCode value1,
        string value2
    ) => value1.Value.Equals(value2);

    public static bool operator !=(
        ConsumerSitePersonalizedEstimateErrorCode value1,
        string value2
    ) => !value1.Value.Equals(value2);

    public static explicit operator string(ConsumerSitePersonalizedEstimateErrorCode value) =>
        value.Value;

    public static explicit operator ConsumerSitePersonalizedEstimateErrorCode(string value) =>
        new(value);

    /// <summary>
    /// Constant strings for enum values
    /// </summary>
    [Serializable]
    public static class Values
    {
        public const string InternalServerError = "internal_server_error";

        public const string EligibilityServiceError = "eligibility_service_error";

        public const string UnsupportedTpa = "unsupported_tpa";

        public const string UnavailablePayer = "unavailable_payer";

        public const string InvalidDob = "invalid_dob";

        public const string InvalidMemberId = "invalid_member_id";

        public const string InvalidMemberName = "invalid_member_name";

        public const string MemberNotFound = "member_not_found";

        public const string NoActiveCoverage = "no_active_coverage";

        public const string PlanNotFound = "plan_not_found";

        public const string PricingUnavailable = "pricing_unavailable";

        public const string InvalidJson = "invalid_json";

        public const string Unauthorized = "unauthorized";

        public const string Forbidden = "forbidden";

        public const string AuthorizationUnavailable = "authorization_unavailable";

        public const string InvalidRequest = "invalid_request";

        public const string InvalidIdFormat = "invalid_id_format";

        public const string InsufficientScope = "insufficient_scope";

        public const string LocationAmbiguous = "location_ambiguous";

        public const string UnresolvableLocation = "unresolvable_location";

        public const string NotFound = "not_found";

        public const string InvalidExpand = "invalid_expand";

        public const string PermissionDenied = "permission_denied";

        public const string RateLimited = "rate_limited";

        public const string SearchUnavailable = "search_unavailable";

        public const string ConflictingParameters = "conflicting_parameters";
    }
}
