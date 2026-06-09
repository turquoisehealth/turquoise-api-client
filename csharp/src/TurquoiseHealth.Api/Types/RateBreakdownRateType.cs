using System.Text.Json.Serialization;
using TurquoiseHealth.Api.Core;

namespace TurquoiseHealth.Api;

[JsonConverter(typeof(StringEnumSerializer<RateBreakdownRateType>))]
[Serializable]
public readonly record struct RateBreakdownRateType : IStringEnum
{
    public static readonly RateBreakdownRateType Cash = new(Values.Cash);

    public static readonly RateBreakdownRateType Negotiated = new(Values.Negotiated);

    public RateBreakdownRateType(string value)
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
    public static RateBreakdownRateType FromCustom(string value)
    {
        return new RateBreakdownRateType(value);
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

    public static bool operator ==(RateBreakdownRateType value1, string value2) =>
        value1.Value.Equals(value2);

    public static bool operator !=(RateBreakdownRateType value1, string value2) =>
        !value1.Value.Equals(value2);

    public static explicit operator string(RateBreakdownRateType value) => value.Value;

    public static explicit operator RateBreakdownRateType(string value) => new(value);

    /// <summary>
    /// Constant strings for enum values
    /// </summary>
    [Serializable]
    public static class Values
    {
        public const string Cash = "cash";

        public const string Negotiated = "negotiated";
    }
}
