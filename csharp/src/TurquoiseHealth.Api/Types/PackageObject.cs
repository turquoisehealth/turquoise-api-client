using System.Text.Json.Serialization;
using TurquoiseHealth.Api.Core;

namespace TurquoiseHealth.Api;

[JsonConverter(typeof(StringEnumSerializer<PackageObject>))]
[Serializable]
public readonly record struct PackageObject : IStringEnum
{
    public static readonly PackageObject Package = new(Values.Package);

    public PackageObject(string value)
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
    public static PackageObject FromCustom(string value)
    {
        return new PackageObject(value);
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

    public static bool operator ==(PackageObject value1, string value2) =>
        value1.Value.Equals(value2);

    public static bool operator !=(PackageObject value1, string value2) =>
        !value1.Value.Equals(value2);

    public static explicit operator string(PackageObject value) => value.Value;

    public static explicit operator PackageObject(string value) => new(value);

    /// <summary>
    /// Constant strings for enum values
    /// </summary>
    [Serializable]
    public static class Values
    {
        public const string Package = "package";
    }
}
