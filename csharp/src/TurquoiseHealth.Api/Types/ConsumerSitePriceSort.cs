using System.Text.Json.Serialization;
using TurquoiseHealth.Api.Core;

namespace TurquoiseHealth.Api;

[JsonConverter(typeof(StringEnumSerializer<ConsumerSitePriceSort>))]
[Serializable]
public readonly record struct ConsumerSitePriceSort : IStringEnum
{
    public static readonly ConsumerSitePriceSort Total = new(Values.Total);

    public static readonly ConsumerSitePriceSort Distance = new(Values.Distance);

    public ConsumerSitePriceSort(string value)
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
    public static ConsumerSitePriceSort FromCustom(string value)
    {
        return new ConsumerSitePriceSort(value);
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

    public static bool operator ==(ConsumerSitePriceSort value1, string value2) =>
        value1.Value.Equals(value2);

    public static bool operator !=(ConsumerSitePriceSort value1, string value2) =>
        !value1.Value.Equals(value2);

    public static explicit operator string(ConsumerSitePriceSort value) => value.Value;

    public static explicit operator ConsumerSitePriceSort(string value) => new(value);

    /// <summary>
    /// Constant strings for enum values
    /// </summary>
    [Serializable]
    public static class Values
    {
        public const string Total = "total";

        public const string Distance = "distance";
    }
}
