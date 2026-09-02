using System.Text.Json.Serialization;
using TurquoiseHealth.Api.Core;

namespace TurquoiseHealth.Api;

[JsonConverter(typeof(StringEnumSerializer<ListEnvelopePayerObject>))]
[Serializable]
public readonly record struct ListEnvelopePayerObject : IStringEnum
{
    public static readonly ListEnvelopePayerObject List = new(Values.List);

    public ListEnvelopePayerObject(string value)
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
    public static ListEnvelopePayerObject FromCustom(string value)
    {
        return new ListEnvelopePayerObject(value);
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

    public static bool operator ==(ListEnvelopePayerObject value1, string value2) =>
        value1.Value.Equals(value2);

    public static bool operator !=(ListEnvelopePayerObject value1, string value2) =>
        !value1.Value.Equals(value2);

    public static explicit operator string(ListEnvelopePayerObject value) => value.Value;

    public static explicit operator ListEnvelopePayerObject(string value) => new(value);

    /// <summary>
    /// Constant strings for enum values
    /// </summary>
    [Serializable]
    public static class Values
    {
        public const string List = "list";
    }
}
