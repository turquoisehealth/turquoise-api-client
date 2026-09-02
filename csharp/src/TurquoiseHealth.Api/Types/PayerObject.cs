using System.Text.Json.Serialization;
using TurquoiseHealth.Api.Core;

namespace TurquoiseHealth.Api;

[JsonConverter(typeof(StringEnumSerializer<PayerObject>))]
[Serializable]
public readonly record struct PayerObject : IStringEnum
{
    public static readonly PayerObject Payer = new(Values.Payer);

    public PayerObject(string value)
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
    public static PayerObject FromCustom(string value)
    {
        return new PayerObject(value);
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

    public static bool operator ==(PayerObject value1, string value2) =>
        value1.Value.Equals(value2);

    public static bool operator !=(PayerObject value1, string value2) =>
        !value1.Value.Equals(value2);

    public static explicit operator string(PayerObject value) => value.Value;

    public static explicit operator PayerObject(string value) => new(value);

    /// <summary>
    /// Constant strings for enum values
    /// </summary>
    [Serializable]
    public static class Values
    {
        public const string Payer = "payer";
    }
}
