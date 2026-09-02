using System.Text.Json.Serialization;
using TurquoiseHealth.Api.Core;

namespace TurquoiseHealth.Api;

[JsonConverter(typeof(StringEnumSerializer<ProviderObject>))]
[Serializable]
public readonly record struct ProviderObject : IStringEnum
{
    public static readonly ProviderObject Provider = new(Values.Provider);

    public ProviderObject(string value)
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
    public static ProviderObject FromCustom(string value)
    {
        return new ProviderObject(value);
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

    public static bool operator ==(ProviderObject value1, string value2) =>
        value1.Value.Equals(value2);

    public static bool operator !=(ProviderObject value1, string value2) =>
        !value1.Value.Equals(value2);

    public static explicit operator string(ProviderObject value) => value.Value;

    public static explicit operator ProviderObject(string value) => new(value);

    /// <summary>
    /// Constant strings for enum values
    /// </summary>
    [Serializable]
    public static class Values
    {
        public const string Provider = "provider";
    }
}
