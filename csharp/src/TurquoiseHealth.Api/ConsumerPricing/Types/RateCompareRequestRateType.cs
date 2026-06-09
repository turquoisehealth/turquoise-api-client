using System.Text.Json.Serialization;
using TurquoiseHealth.Api.Core;

namespace TurquoiseHealth.Api;

[JsonConverter(typeof(StringEnumSerializer<RateCompareRequestRateType>))]
[Serializable]
public readonly record struct RateCompareRequestRateType : IStringEnum
{
    public static readonly RateCompareRequestRateType Cash = new(Values.Cash);

    public static readonly RateCompareRequestRateType Negotiated = new(Values.Negotiated);

    public RateCompareRequestRateType(string value)
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
    public static RateCompareRequestRateType FromCustom(string value)
    {
        return new RateCompareRequestRateType(value);
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

    public static bool operator ==(RateCompareRequestRateType value1, string value2) =>
        value1.Value.Equals(value2);

    public static bool operator !=(RateCompareRequestRateType value1, string value2) =>
        !value1.Value.Equals(value2);

    public static explicit operator string(RateCompareRequestRateType value) => value.Value;

    public static explicit operator RateCompareRequestRateType(string value) => new(value);

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
