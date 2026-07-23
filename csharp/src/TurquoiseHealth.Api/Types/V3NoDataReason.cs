using System.Text.Json.Serialization;
using TurquoiseHealth.Api.Core;

namespace TurquoiseHealth.Api;

[JsonConverter(typeof(StringEnumSerializer<V3NoDataReason>))]
[Serializable]
public readonly record struct V3NoDataReason : IStringEnum
{
    public static readonly V3NoDataReason NoData = new(Values.NoData);

    public static readonly V3NoDataReason PermissionDenied = new(Values.PermissionDenied);

    public V3NoDataReason(string value)
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
    public static V3NoDataReason FromCustom(string value)
    {
        return new V3NoDataReason(value);
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

    public static bool operator ==(V3NoDataReason value1, string value2) =>
        value1.Value.Equals(value2);

    public static bool operator !=(V3NoDataReason value1, string value2) =>
        !value1.Value.Equals(value2);

    public static explicit operator string(V3NoDataReason value) => value.Value;

    public static explicit operator V3NoDataReason(string value) => new(value);

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
