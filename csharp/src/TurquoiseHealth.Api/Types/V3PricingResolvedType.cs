using System.Text.Json.Serialization;
using TurquoiseHealth.Api.Core;

namespace TurquoiseHealth.Api;

[JsonConverter(typeof(StringEnumSerializer<V3PricingResolvedType>))]
[Serializable]
public readonly record struct V3PricingResolvedType : IStringEnum
{
    public static readonly V3PricingResolvedType Cash = new(Values.Cash);

    public static readonly V3PricingResolvedType Negotiated = new(Values.Negotiated);

    public V3PricingResolvedType(string value)
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
    public static V3PricingResolvedType FromCustom(string value)
    {
        return new V3PricingResolvedType(value);
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

    public static bool operator ==(V3PricingResolvedType value1, string value2) =>
        value1.Value.Equals(value2);

    public static bool operator !=(V3PricingResolvedType value1, string value2) =>
        !value1.Value.Equals(value2);

    public static explicit operator string(V3PricingResolvedType value) => value.Value;

    public static explicit operator V3PricingResolvedType(string value) => new(value);

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
