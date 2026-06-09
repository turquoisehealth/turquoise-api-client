using System.Text.Json.Serialization;
using TurquoiseHealth.Api.Core;

namespace TurquoiseHealth.Api;

[JsonConverter(typeof(StringEnumSerializer<RateRateType>))]
[Serializable]
public readonly record struct RateRateType : IStringEnum
{
    public static readonly RateRateType Cash = new(Values.Cash);

    public static readonly RateRateType Negotiated = new(Values.Negotiated);

    public RateRateType(string value)
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
    public static RateRateType FromCustom(string value)
    {
        return new RateRateType(value);
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

    public static bool operator ==(RateRateType value1, string value2) =>
        value1.Value.Equals(value2);

    public static bool operator !=(RateRateType value1, string value2) =>
        !value1.Value.Equals(value2);

    public static explicit operator string(RateRateType value) => value.Value;

    public static explicit operator RateRateType(string value) => new(value);

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
