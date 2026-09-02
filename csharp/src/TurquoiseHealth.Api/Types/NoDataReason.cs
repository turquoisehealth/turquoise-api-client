using System.Text.Json.Serialization;
using TurquoiseHealth.Api.Core;

namespace TurquoiseHealth.Api;

[JsonConverter(typeof(StringEnumSerializer<NoDataReason>))]
[Serializable]
public readonly record struct NoDataReason : IStringEnum
{
    public static readonly NoDataReason NoData = new(Values.NoData);

    public static readonly NoDataReason PermissionDenied = new(Values.PermissionDenied);

    public NoDataReason(string value)
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
    public static NoDataReason FromCustom(string value)
    {
        return new NoDataReason(value);
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

    public static bool operator ==(NoDataReason value1, string value2) =>
        value1.Value.Equals(value2);

    public static bool operator !=(NoDataReason value1, string value2) =>
        !value1.Value.Equals(value2);

    public static explicit operator string(NoDataReason value) => value.Value;

    public static explicit operator NoDataReason(string value) => new(value);

    /// <summary>
    /// Constant strings for enum values
    /// </summary>
    [Serializable]
    public static class Values
    {
        public const string NoData = "no_data";

        public const string PermissionDenied = "permission_denied";
    }
}
