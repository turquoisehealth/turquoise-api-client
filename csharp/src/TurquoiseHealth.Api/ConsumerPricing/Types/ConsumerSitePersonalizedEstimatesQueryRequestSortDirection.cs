using System.Text.Json.Serialization;
using TurquoiseHealth.Api.Core;

namespace TurquoiseHealth.Api;

[JsonConverter(
    typeof(StringEnumSerializer<ConsumerSitePersonalizedEstimatesQueryRequestSortDirection>)
)]
[Serializable]
public readonly record struct ConsumerSitePersonalizedEstimatesQueryRequestSortDirection
    : IStringEnum
{
    public static readonly ConsumerSitePersonalizedEstimatesQueryRequestSortDirection Asc = new(
        Values.Asc
    );

    public static readonly ConsumerSitePersonalizedEstimatesQueryRequestSortDirection Desc = new(
        Values.Desc
    );

    public ConsumerSitePersonalizedEstimatesQueryRequestSortDirection(string value)
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
    public static ConsumerSitePersonalizedEstimatesQueryRequestSortDirection FromCustom(
        string value
    )
    {
        return new ConsumerSitePersonalizedEstimatesQueryRequestSortDirection(value);
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

    public static bool operator ==(
        ConsumerSitePersonalizedEstimatesQueryRequestSortDirection value1,
        string value2
    ) => value1.Value.Equals(value2);

    public static bool operator !=(
        ConsumerSitePersonalizedEstimatesQueryRequestSortDirection value1,
        string value2
    ) => !value1.Value.Equals(value2);

    public static explicit operator string(
        ConsumerSitePersonalizedEstimatesQueryRequestSortDirection value
    ) => value.Value;

    public static explicit operator ConsumerSitePersonalizedEstimatesQueryRequestSortDirection(
        string value
    ) => new(value);

    /// <summary>
    /// Constant strings for enum values
    /// </summary>
    [Serializable]
    public static class Values
    {
        public const string Asc = "asc";

        public const string Desc = "desc";
    }
}
