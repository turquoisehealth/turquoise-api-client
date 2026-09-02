using System.Text.Json.Serialization;
using TurquoiseHealth.Api.Core;

namespace TurquoiseHealth.Api;

[JsonConverter(typeof(StringEnumSerializer<ConsumerSiteNoDataReason>))]
[Serializable]
public readonly record struct ConsumerSiteNoDataReason : IStringEnum
{
    public static readonly ConsumerSiteNoDataReason NoData = new(Values.NoData);

    public static readonly ConsumerSiteNoDataReason PermissionDenied = new(Values.PermissionDenied);

    public ConsumerSiteNoDataReason(string value)
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
    public static ConsumerSiteNoDataReason FromCustom(string value)
    {
        return new ConsumerSiteNoDataReason(value);
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

    public static bool operator ==(ConsumerSiteNoDataReason value1, string value2) =>
        value1.Value.Equals(value2);

    public static bool operator !=(ConsumerSiteNoDataReason value1, string value2) =>
        !value1.Value.Equals(value2);

    public static explicit operator string(ConsumerSiteNoDataReason value) => value.Value;

    public static explicit operator ConsumerSiteNoDataReason(string value) => new(value);

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
