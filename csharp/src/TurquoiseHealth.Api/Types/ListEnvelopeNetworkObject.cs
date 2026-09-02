using System.Text.Json.Serialization;
using TurquoiseHealth.Api.Core;

namespace TurquoiseHealth.Api;

[JsonConverter(typeof(StringEnumSerializer<ListEnvelopeNetworkObject>))]
[Serializable]
public readonly record struct ListEnvelopeNetworkObject : IStringEnum
{
    public static readonly ListEnvelopeNetworkObject List = new(Values.List);

    public ListEnvelopeNetworkObject(string value)
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
    public static ListEnvelopeNetworkObject FromCustom(string value)
    {
        return new ListEnvelopeNetworkObject(value);
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

    public static bool operator ==(ListEnvelopeNetworkObject value1, string value2) =>
        value1.Value.Equals(value2);

    public static bool operator !=(ListEnvelopeNetworkObject value1, string value2) =>
        !value1.Value.Equals(value2);

    public static explicit operator string(ListEnvelopeNetworkObject value) => value.Value;

    public static explicit operator ListEnvelopeNetworkObject(string value) => new(value);

    /// <summary>
    /// Constant strings for enum values
    /// </summary>
    [Serializable]
    public static class Values
    {
        public const string List = "list";
    }
}
