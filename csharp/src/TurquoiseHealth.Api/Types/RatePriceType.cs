using System.Text.Json.Serialization;
using TurquoiseHealth.Api.Core;

namespace TurquoiseHealth.Api;

[JsonConverter(typeof(StringEnumSerializer<RatePriceType>))]
[Serializable]
public readonly record struct RatePriceType : IStringEnum
{
    public static readonly RatePriceType Cash = new(Values.Cash);

    public static readonly RatePriceType Negotiated = new(Values.Negotiated);

    public RatePriceType(string value)
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
    public static RatePriceType FromCustom(string value)
    {
        return new RatePriceType(value);
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

    public static bool operator ==(RatePriceType value1, string value2) =>
        value1.Value.Equals(value2);

    public static bool operator !=(RatePriceType value1, string value2) =>
        !value1.Value.Equals(value2);

    public static explicit operator string(RatePriceType value) => value.Value;

    public static explicit operator RatePriceType(string value) => new(value);

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
