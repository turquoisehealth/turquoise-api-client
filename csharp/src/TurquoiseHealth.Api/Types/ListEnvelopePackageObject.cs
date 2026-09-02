using System.Text.Json.Serialization;
using TurquoiseHealth.Api.Core;

namespace TurquoiseHealth.Api;

[JsonConverter(typeof(StringEnumSerializer<ListEnvelopePackageObject>))]
[Serializable]
public readonly record struct ListEnvelopePackageObject : IStringEnum
{
    public static readonly ListEnvelopePackageObject List = new(Values.List);

    public ListEnvelopePackageObject(string value)
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
    public static ListEnvelopePackageObject FromCustom(string value)
    {
        return new ListEnvelopePackageObject(value);
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

    public static bool operator ==(ListEnvelopePackageObject value1, string value2) =>
        value1.Value.Equals(value2);

    public static bool operator !=(ListEnvelopePackageObject value1, string value2) =>
        !value1.Value.Equals(value2);

    public static explicit operator string(ListEnvelopePackageObject value) => value.Value;

    public static explicit operator ListEnvelopePackageObject(string value) => new(value);

    /// <summary>
    /// Constant strings for enum values
    /// </summary>
    [Serializable]
    public static class Values
    {
        public const string List = "list";
    }
}
