using System.Text.Json.Serialization;
using TurquoiseHealth.Api.Core;

namespace TurquoiseHealth.Api;

[JsonConverter(typeof(StringEnumSerializer<V3PricesQueryRequestSortDirection>))]
[Serializable]
public readonly record struct V3PricesQueryRequestSortDirection : IStringEnum
{
    public static readonly V3PricesQueryRequestSortDirection Asc = new(Values.Asc);

    public static readonly V3PricesQueryRequestSortDirection Desc = new(Values.Desc);

    public V3PricesQueryRequestSortDirection(string value)
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
    public static V3PricesQueryRequestSortDirection FromCustom(string value)
    {
        return new V3PricesQueryRequestSortDirection(value);
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

    public static bool operator ==(V3PricesQueryRequestSortDirection value1, string value2) =>
        value1.Value.Equals(value2);

    public static bool operator !=(V3PricesQueryRequestSortDirection value1, string value2) =>
        !value1.Value.Equals(value2);

    public static explicit operator string(V3PricesQueryRequestSortDirection value) => value.Value;

    public static explicit operator V3PricesQueryRequestSortDirection(string value) => new(value);

    /// <summary>
    /// Constant strings for enum values
    /// </summary>
    [Serializable]
    public static class Values
    {
        public const string Asc = "asc";

        public const string Desc = "desc";
    }
}
