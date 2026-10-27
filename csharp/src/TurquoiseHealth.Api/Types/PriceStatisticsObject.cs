using System.Text.Json.Serialization;
using TurquoiseHealth.Api.Core;

namespace TurquoiseHealth.Api;

[JsonConverter(typeof(StringEnumSerializer<PriceStatisticsObject>))]
[Serializable]
public readonly record struct PriceStatisticsObject : IStringEnum
{
    public static readonly PriceStatisticsObject PriceStatistics = new(Values.PriceStatistics);

    public PriceStatisticsObject(string value)
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
    public static PriceStatisticsObject FromCustom(string value)
    {
        return new PriceStatisticsObject(value);
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

    public static bool operator ==(PriceStatisticsObject value1, string value2) =>
        value1.Value.Equals(value2);

    public static bool operator !=(PriceStatisticsObject value1, string value2) =>
        !value1.Value.Equals(value2);

    public static explicit operator string(PriceStatisticsObject value) => value.Value;

    public static explicit operator PriceStatisticsObject(string value) => new(value);

    /// <summary>
    /// Constant strings for enum values
    /// </summary>
    [Serializable]
    public static class Values
    {
        public const string PriceStatistics = "price_statistics";
    }
}
