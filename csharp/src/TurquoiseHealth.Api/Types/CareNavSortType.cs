using System.Text.Json.Serialization;
using TurquoiseHealth.Api.Core;

namespace TurquoiseHealth.Api;

[JsonConverter(typeof(StringEnumSerializer<CareNavSortType>))]
[Serializable]
public readonly record struct CareNavSortType : IStringEnum
{
    public static readonly CareNavSortType PriceAsc = new(Values.PriceAsc);

    public static readonly CareNavSortType RatingDesc = new(Values.RatingDesc);

    public CareNavSortType(string value)
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
    public static CareNavSortType FromCustom(string value)
    {
        return new CareNavSortType(value);
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

    public static bool operator ==(CareNavSortType value1, string value2) =>
        value1.Value.Equals(value2);

    public static bool operator !=(CareNavSortType value1, string value2) =>
        !value1.Value.Equals(value2);

    public static explicit operator string(CareNavSortType value) => value.Value;

    public static explicit operator CareNavSortType(string value) => new(value);

    /// <summary>
    /// Constant strings for enum values
    /// </summary>
    [Serializable]
    public static class Values
    {
        public const string PriceAsc = "price-asc";

        public const string RatingDesc = "rating-desc";
    }
}
