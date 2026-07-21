using System.Text.Json.Serialization;
using TurquoiseHealth.Api.Core;

namespace TurquoiseHealth.Api;

[JsonConverter(typeof(StringEnumSerializer<ErrorCode>))]
[Serializable]
public readonly record struct ErrorCode : IStringEnum
{
    public static readonly ErrorCode InvalidRequest = new(Values.InvalidRequest);

    public static readonly ErrorCode InvalidStateFormat = new(Values.InvalidStateFormat);

    public static readonly ErrorCode InvalidIdFormat = new(Values.InvalidIdFormat);

    public static readonly ErrorCode InvalidExpand = new(Values.InvalidExpand);

    public static readonly ErrorCode InsufficientScope = new(Values.InsufficientScope);

    public static readonly ErrorCode LocationRequired = new(Values.LocationRequired);

    public static readonly ErrorCode LocationAmbiguous = new(Values.LocationAmbiguous);

    public static readonly ErrorCode UnresolvableLocation = new(Values.UnresolvableLocation);

    public static readonly ErrorCode NotFound = new(Values.NotFound);

    public static readonly ErrorCode InsufficientData = new(Values.InsufficientData);

    public static readonly ErrorCode RateLimited = new(Values.RateLimited);

    public static readonly ErrorCode PermissionDenied = new(Values.PermissionDenied);

    public static readonly ErrorCode SearchUnavailable = new(Values.SearchUnavailable);

    public ErrorCode(string value)
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
    public static ErrorCode FromCustom(string value)
    {
        return new ErrorCode(value);
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

    public static bool operator ==(ErrorCode value1, string value2) => value1.Value.Equals(value2);

    public static bool operator !=(ErrorCode value1, string value2) => !value1.Value.Equals(value2);

    public static explicit operator string(ErrorCode value) => value.Value;

    public static explicit operator ErrorCode(string value) => new(value);

    /// <summary>
    /// Constant strings for enum values
    /// </summary>
    [Serializable]
    public static class Values
    {
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
