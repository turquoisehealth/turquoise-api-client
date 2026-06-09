using System.Text.Json.Serialization;
using TurquoiseHealth.Api.Core;

namespace TurquoiseHealth.Api;

[JsonConverter(typeof(StringEnumSerializer<PricesRequestRateType>))]
[Serializable]
public readonly record struct PricesRequestRateType : IStringEnum
{
    public static readonly PricesRequestRateType Cash = new(Values.Cash);

    public static readonly PricesRequestRateType Negotiated = new(Values.Negotiated);

    public PricesRequestRateType(string value)
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
    public static PricesRequestRateType FromCustom(string value)
    {
        return new PricesRequestRateType(value);
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

    public static bool operator ==(PricesRequestRateType value1, string value2) =>
        value1.Value.Equals(value2);

    public static bool operator !=(PricesRequestRateType value1, string value2) =>
        !value1.Value.Equals(value2);

    public static explicit operator string(PricesRequestRateType value) => value.Value;

    public static explicit operator PricesRequestRateType(string value) => new(value);

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
