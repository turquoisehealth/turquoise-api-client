using System.Text.Json.Serialization;
using TurquoiseHealth.Api.Core;

namespace TurquoiseHealth.Api;

[JsonConverter(typeof(StringEnumSerializer<ConsumerPricingProviderObject>))]
[Serializable]
public readonly record struct ConsumerPricingProviderObject : IStringEnum
{
    public static readonly ConsumerPricingProviderObject Provider = new(Values.Provider);

    public ConsumerPricingProviderObject(string value)
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
    public static ConsumerPricingProviderObject FromCustom(string value)
    {
        return new ConsumerPricingProviderObject(value);
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

    public static bool operator ==(ConsumerPricingProviderObject value1, string value2) =>
        value1.Value.Equals(value2);

    public static bool operator !=(ConsumerPricingProviderObject value1, string value2) =>
        !value1.Value.Equals(value2);

    public static explicit operator string(ConsumerPricingProviderObject value) => value.Value;

    public static explicit operator ConsumerPricingProviderObject(string value) => new(value);

    /// <summary>
    /// Constant strings for enum values
    /// </summary>
    [Serializable]
    public static class Values
    {
        public const string Provider = "provider";
    }
}
