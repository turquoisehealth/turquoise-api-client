using System.Text.Json.Serialization;
using TurquoiseHealth.Api.Core;

namespace TurquoiseHealth.Api;

[JsonConverter(typeof(StringEnumSerializer<PricesQueryRequestSortDirection>))]
[Serializable]
public readonly record struct PricesQueryRequestSortDirection : IStringEnum
{
    public static readonly PricesQueryRequestSortDirection Asc = new(Values.Asc);

    public static readonly PricesQueryRequestSortDirection Desc = new(Values.Desc);

    public PricesQueryRequestSortDirection(string value)
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
    public static PricesQueryRequestSortDirection FromCustom(string value)
    {
        return new PricesQueryRequestSortDirection(value);
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

    public static bool operator ==(PricesQueryRequestSortDirection value1, string value2) =>
        value1.Value.Equals(value2);

    public static bool operator !=(PricesQueryRequestSortDirection value1, string value2) =>
        !value1.Value.Equals(value2);

    public static explicit operator string(PricesQueryRequestSortDirection value) => value.Value;

    public static explicit operator PricesQueryRequestSortDirection(string value) => new(value);

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
