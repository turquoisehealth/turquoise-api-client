using System.Text.Json.Serialization;
using TurquoiseHealth.Api.Core;

namespace TurquoiseHealth.Api;

[JsonConverter(typeof(StringEnumSerializer<V3ListEnvelopePackageObject>))]
[Serializable]
public readonly record struct V3ListEnvelopePackageObject : IStringEnum
{
    public static readonly V3ListEnvelopePackageObject List = new(Values.List);

    public V3ListEnvelopePackageObject(string value)
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
    public static V3ListEnvelopePackageObject FromCustom(string value)
    {
        return new V3ListEnvelopePackageObject(value);
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

    public static bool operator ==(V3ListEnvelopePackageObject value1, string value2) =>
        value1.Value.Equals(value2);

    public static bool operator !=(V3ListEnvelopePackageObject value1, string value2) =>
        !value1.Value.Equals(value2);

    public static explicit operator string(V3ListEnvelopePackageObject value) => value.Value;

    public static explicit operator V3ListEnvelopePackageObject(string value) => new(value);

    /// <summary>
    /// Constant strings for enum values
    /// </summary>
    [Serializable]
    public static class Values
    {
        public const string List = "list";
    }
}
