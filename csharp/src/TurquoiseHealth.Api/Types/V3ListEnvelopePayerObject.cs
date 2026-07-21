using System.Text.Json.Serialization;
using TurquoiseHealth.Api.Core;

namespace TurquoiseHealth.Api;

[JsonConverter(typeof(StringEnumSerializer<V3ListEnvelopePayerObject>))]
[Serializable]
public readonly record struct V3ListEnvelopePayerObject : IStringEnum
{
    public static readonly V3ListEnvelopePayerObject List = new(Values.List);

    public V3ListEnvelopePayerObject(string value)
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
    public static V3ListEnvelopePayerObject FromCustom(string value)
    {
        return new V3ListEnvelopePayerObject(value);
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

    public static bool operator ==(V3ListEnvelopePayerObject value1, string value2) =>
        value1.Value.Equals(value2);

    public static bool operator !=(V3ListEnvelopePayerObject value1, string value2) =>
        !value1.Value.Equals(value2);

    public static explicit operator string(V3ListEnvelopePayerObject value) => value.Value;

    public static explicit operator V3ListEnvelopePayerObject(string value) => new(value);

    /// <summary>
    /// Constant strings for enum values
    /// </summary>
    [Serializable]
    public static class Values
    {
        public const string List = "list";
    }
}
