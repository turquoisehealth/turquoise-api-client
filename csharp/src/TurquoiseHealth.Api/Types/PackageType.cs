using System.Text.Json.Serialization;
using TurquoiseHealth.Api.Core;

namespace TurquoiseHealth.Api;

[JsonConverter(typeof(StringEnumSerializer<PackageType>))]
[Serializable]
public readonly record struct PackageType : IStringEnum
{
    public static readonly PackageType Ssp = new(Values.Ssp);

    public PackageType(string value)
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
    public static PackageType FromCustom(string value)
    {
        return new PackageType(value);
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

    public static bool operator ==(PackageType value1, string value2) =>
        value1.Value.Equals(value2);

    public static bool operator !=(PackageType value1, string value2) =>
        !value1.Value.Equals(value2);

    public static explicit operator string(PackageType value) => value.Value;

    public static explicit operator PackageType(string value) => new(value);

    /// <summary>
    /// Constant strings for enum values
    /// </summary>
    [Serializable]
    public static class Values
    {
        public const string Ssp = "ssp";
    }
}
