using System.Text.Json.Serialization;

namespace TurquoiseHealth.Api.Core;

public interface IStringEnum : IEquatable<string>
{
    public string Value { get; }
}
