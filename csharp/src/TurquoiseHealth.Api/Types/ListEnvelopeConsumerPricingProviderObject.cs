using System.Text.Json.Serialization;
using TurquoiseHealth.Api.Core;

namespace TurquoiseHealth.Api;

[JsonConverter(typeof(StringEnumSerializer<ListEnvelopeConsumerPricingProviderObject>))]
[Serializable]
public readonly record struct ListEnvelopeConsumerPricingProviderObject : IStringEnum
{
    public static readonly ListEnvelopeConsumerPricingProviderObject List = new(Values.List);

    public ListEnvelopeConsumerPricingProviderObject(string value)
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
    public static ListEnvelopeConsumerPricingProviderObject FromCustom(string value)
    {
        return new ListEnvelopeConsumerPricingProviderObject(value);
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

    public static bool operator ==(
        ListEnvelopeConsumerPricingProviderObject value1,
        string value2
    ) => value1.Value.Equals(value2);

    public static bool operator !=(
        ListEnvelopeConsumerPricingProviderObject value1,
        string value2
    ) => !value1.Value.Equals(value2);

    public static explicit operator string(ListEnvelopeConsumerPricingProviderObject value) =>
        value.Value;

    public static explicit operator ListEnvelopeConsumerPricingProviderObject(string value) =>
        new(value);

    /// <summary>
    /// Constant strings for enum values
    /// </summary>
    [Serializable]
    public static class Values
    {
        public const string List = "list";
    }
}
