using System.Text.Json.Serialization;
using TurquoiseHealth.Api.Core;

namespace TurquoiseHealth.Api;

[JsonConverter(typeof(StringEnumSerializer<ConsumerSitePersonalizedEstimateErrorCodeV2>))]
[Serializable]
public readonly record struct ConsumerSitePersonalizedEstimateErrorCodeV2 : IStringEnum
{
    public static readonly ConsumerSitePersonalizedEstimateErrorCodeV2 InternalServerError = new(
        Values.InternalServerError
    );

    public static readonly ConsumerSitePersonalizedEstimateErrorCodeV2 EligibilityServiceError =
        new(Values.EligibilityServiceError);

    public static readonly ConsumerSitePersonalizedEstimateErrorCodeV2 UnsupportedTpa = new(
        Values.UnsupportedTpa
    );

    public static readonly ConsumerSitePersonalizedEstimateErrorCodeV2 UnavailablePayer = new(
        Values.UnavailablePayer
    );

    public static readonly ConsumerSitePersonalizedEstimateErrorCodeV2 InvalidDob = new(
        Values.InvalidDob
    );

    public static readonly ConsumerSitePersonalizedEstimateErrorCodeV2 InvalidMemberId = new(
        Values.InvalidMemberId
    );

    public static readonly ConsumerSitePersonalizedEstimateErrorCodeV2 InvalidMemberName = new(
        Values.InvalidMemberName
    );

    public static readonly ConsumerSitePersonalizedEstimateErrorCodeV2 MemberNotFound = new(
        Values.MemberNotFound
    );

    public static readonly ConsumerSitePersonalizedEstimateErrorCodeV2 NoActiveCoverage = new(
        Values.NoActiveCoverage
    );

    public static readonly ConsumerSitePersonalizedEstimateErrorCodeV2 PlanNotFound = new(
        Values.PlanNotFound
    );

    public static readonly ConsumerSitePersonalizedEstimateErrorCodeV2 PricingUnavailable = new(
        Values.PricingUnavailable
    );

    public static readonly ConsumerSitePersonalizedEstimateErrorCodeV2 InvalidJson = new(
        Values.InvalidJson
    );

    public static readonly ConsumerSitePersonalizedEstimateErrorCodeV2 Unauthorized = new(
        Values.Unauthorized
    );

    public static readonly ConsumerSitePersonalizedEstimateErrorCodeV2 Forbidden = new(
        Values.Forbidden
    );

    public static readonly ConsumerSitePersonalizedEstimateErrorCodeV2 AuthorizationUnavailable =
        new(Values.AuthorizationUnavailable);

    public static readonly ConsumerSitePersonalizedEstimateErrorCodeV2 InvalidRequest = new(
        Values.InvalidRequest
    );

    public static readonly ConsumerSitePersonalizedEstimateErrorCodeV2 InvalidStateFormat = new(
        Values.InvalidStateFormat
    );

    public static readonly ConsumerSitePersonalizedEstimateErrorCodeV2 InvalidIdFormat = new(
        Values.InvalidIdFormat
    );

    public static readonly ConsumerSitePersonalizedEstimateErrorCodeV2 InvalidExpand = new(
        Values.InvalidExpand
    );

    public static readonly ConsumerSitePersonalizedEstimateErrorCodeV2 InsufficientScope = new(
        Values.InsufficientScope
    );

    public static readonly ConsumerSitePersonalizedEstimateErrorCodeV2 LocationRequired = new(
        Values.LocationRequired
    );

    public static readonly ConsumerSitePersonalizedEstimateErrorCodeV2 LocationAmbiguous = new(
        Values.LocationAmbiguous
    );

    public static readonly ConsumerSitePersonalizedEstimateErrorCodeV2 UnresolvableLocation = new(
        Values.UnresolvableLocation
    );

    public static readonly ConsumerSitePersonalizedEstimateErrorCodeV2 NotFound = new(
        Values.NotFound
    );

    public static readonly ConsumerSitePersonalizedEstimateErrorCodeV2 InsufficientData = new(
        Values.InsufficientData
    );

    public static readonly ConsumerSitePersonalizedEstimateErrorCodeV2 RateLimited = new(
        Values.RateLimited
    );

    public static readonly ConsumerSitePersonalizedEstimateErrorCodeV2 PermissionDenied = new(
        Values.PermissionDenied
    );

    public static readonly ConsumerSitePersonalizedEstimateErrorCodeV2 SearchUnavailable = new(
        Values.SearchUnavailable
    );

    public ConsumerSitePersonalizedEstimateErrorCodeV2(string value)
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
    public static ConsumerSitePersonalizedEstimateErrorCodeV2 FromCustom(string value)
    {
        return new ConsumerSitePersonalizedEstimateErrorCodeV2(value);
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
        ConsumerSitePersonalizedEstimateErrorCodeV2 value1,
        string value2
    ) => value1.Value.Equals(value2);

    public static bool operator !=(
        ConsumerSitePersonalizedEstimateErrorCodeV2 value1,
        string value2
    ) => !value1.Value.Equals(value2);

    public static explicit operator string(ConsumerSitePersonalizedEstimateErrorCodeV2 value) =>
        value.Value;

    public static explicit operator ConsumerSitePersonalizedEstimateErrorCodeV2(string value) =>
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

        public const string InvalidRequest = "INVALID_REQUEST";

        public const string InvalidStateFormat = "INVALID_STATE_FORMAT";

        public const string InvalidIdFormat = "INVALID_ID_FORMAT";

        public const string InvalidExpand = "INVALID_EXPAND";

        public const string InsufficientScope = "INSUFFICIENT_SCOPE";

        public const string LocationRequired = "LOCATION_REQUIRED";

        public const string LocationAmbiguous = "LOCATION_AMBIGUOUS";

        public const string UnresolvableLocation = "UNRESOLVABLE_LOCATION";

        public const string NotFound = "NOT_FOUND";

        public const string InsufficientData = "INSUFFICIENT_DATA";

        public const string RateLimited = "RATE_LIMITED";

        public const string PermissionDenied = "PERMISSION_DENIED";

        public const string SearchUnavailable = "SEARCH_UNAVAILABLE";
    }
}
