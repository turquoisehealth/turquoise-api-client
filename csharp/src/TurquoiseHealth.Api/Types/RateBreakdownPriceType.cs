using System.Text.Json.Serialization;
using TurquoiseHealth.Api.Core;

namespace TurquoiseHealth.Api;

[JsonConverter(typeof(StringEnumSerializer<RateBreakdownPriceType>))]
[Serializable]
public readonly record struct RateBreakdownPriceType : IStringEnum
{
    public static readonly RateBreakdownPriceType Cash = new(Values.Cash);

    public static readonly RateBreakdownPriceType Negotiated = new(Values.Negotiated);

    public RateBreakdownPriceType(string value)
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
    public static RateBreakdownPriceType FromCustom(string value)
    {
        return new RateBreakdownPriceType(value);
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

    public static bool operator ==(RateBreakdownPriceType value1, string value2) =>
        value1.Value.Equals(value2);

    public static bool operator !=(RateBreakdownPriceType value1, string value2) =>
        !value1.Value.Equals(value2);

    public static explicit operator string(RateBreakdownPriceType value) => value.Value;

    public static explicit operator RateBreakdownPriceType(string value) => new(value);

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
