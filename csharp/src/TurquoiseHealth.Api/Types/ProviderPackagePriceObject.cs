using System.Text.Json.Serialization;
using TurquoiseHealth.Api.Core;

namespace TurquoiseHealth.Api;

[JsonConverter(typeof(StringEnumSerializer<ProviderPackagePriceObject>))]
[Serializable]
public readonly record struct ProviderPackagePriceObject : IStringEnum
{
    public static readonly ProviderPackagePriceObject Price = new(Values.Price);

    public ProviderPackagePriceObject(string value)
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
    public static ProviderPackagePriceObject FromCustom(string value)
    {
        return new ProviderPackagePriceObject(value);
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

    public static bool operator ==(ProviderPackagePriceObject value1, string value2) =>
        value1.Value.Equals(value2);

    public static bool operator !=(ProviderPackagePriceObject value1, string value2) =>
        !value1.Value.Equals(value2);

    public static explicit operator string(ProviderPackagePriceObject value) => value.Value;

    public static explicit operator ProviderPackagePriceObject(string value) => new(value);

    /// <summary>
    /// Constant strings for enum values
    /// </summary>
    [Serializable]
    public static class Values
    {
        public const string Price = "price";
    }
}
