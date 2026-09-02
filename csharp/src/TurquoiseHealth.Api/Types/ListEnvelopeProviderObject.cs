using System.Text.Json.Serialization;
using TurquoiseHealth.Api.Core;

namespace TurquoiseHealth.Api;

[JsonConverter(typeof(StringEnumSerializer<ListEnvelopeProviderObject>))]
[Serializable]
public readonly record struct ListEnvelopeProviderObject : IStringEnum
{
    public static readonly ListEnvelopeProviderObject List = new(Values.List);

    public ListEnvelopeProviderObject(string value)
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
    public static ListEnvelopeProviderObject FromCustom(string value)
    {
        return new ListEnvelopeProviderObject(value);
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

    public static bool operator ==(ListEnvelopeProviderObject value1, string value2) =>
        value1.Value.Equals(value2);

    public static bool operator !=(ListEnvelopeProviderObject value1, string value2) =>
        !value1.Value.Equals(value2);

    public static explicit operator string(ListEnvelopeProviderObject value) => value.Value;

    public static explicit operator ListEnvelopeProviderObject(string value) => new(value);

    /// <summary>
    /// Constant strings for enum values
    /// </summary>
    [Serializable]
    public static class Values
    {
        public const string List = "list";
    }
}
