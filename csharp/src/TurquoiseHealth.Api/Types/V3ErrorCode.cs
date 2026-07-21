using System.Text.Json.Serialization;
using TurquoiseHealth.Api.Core;

namespace TurquoiseHealth.Api;

[JsonConverter(typeof(StringEnumSerializer<V3ErrorCode>))]
[Serializable]
public readonly record struct V3ErrorCode : IStringEnum
{
    public static readonly V3ErrorCode InvalidRequest = new(Values.InvalidRequest);

    public static readonly V3ErrorCode InvalidIdFormat = new(Values.InvalidIdFormat);

    public static readonly V3ErrorCode InsufficientScope = new(Values.InsufficientScope);

    public static readonly V3ErrorCode LocationAmbiguous = new(Values.LocationAmbiguous);

    public static readonly V3ErrorCode UnresolvableLocation = new(Values.UnresolvableLocation);

    public static readonly V3ErrorCode NotFound = new(Values.NotFound);

    public static readonly V3ErrorCode InvalidExpand = new(Values.InvalidExpand);

    public static readonly V3ErrorCode PermissionDenied = new(Values.PermissionDenied);

    public static readonly V3ErrorCode RateLimited = new(Values.RateLimited);

    public static readonly V3ErrorCode SearchUnavailable = new(Values.SearchUnavailable);

    public static readonly V3ErrorCode ConflictingParameters = new(Values.ConflictingParameters);

    public V3ErrorCode(string value)
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
    public static V3ErrorCode FromCustom(string value)
    {
        return new V3ErrorCode(value);
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

    public static bool operator ==(V3ErrorCode value1, string value2) =>
        value1.Value.Equals(value2);

    public static bool operator !=(V3ErrorCode value1, string value2) =>
        !value1.Value.Equals(value2);

    public static explicit operator string(V3ErrorCode value) => value.Value;

    public static explicit operator V3ErrorCode(string value) => new(value);

    /// <summary>
    /// Constant strings for enum values
    /// </summary>
    [Serializable]
    public static class Values
    {
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
