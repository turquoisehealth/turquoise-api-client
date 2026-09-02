using System.Text.Json.Serialization;
using TurquoiseHealth.Api.Core;

namespace TurquoiseHealth.Api;

[JsonConverter(typeof(StringEnumSerializer<PriceComparisonObject>))]
[Serializable]
public readonly record struct PriceComparisonObject : IStringEnum
{
    public static readonly PriceComparisonObject PriceComparison = new(Values.PriceComparison);

    public PriceComparisonObject(string value)
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
    public static PriceComparisonObject FromCustom(string value)
    {
        return new PriceComparisonObject(value);
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

    public static bool operator ==(PriceComparisonObject value1, string value2) =>
        value1.Value.Equals(value2);

    public static bool operator !=(PriceComparisonObject value1, string value2) =>
        !value1.Value.Equals(value2);

    public static explicit operator string(PriceComparisonObject value) => value.Value;

    public static explicit operator PriceComparisonObject(string value) => new(value);

    /// <summary>
    /// Constant strings for enum values
    /// </summary>
    [Serializable]
    public static class Values
    {
        public const string PriceComparison = "price_comparison";
    }
}
