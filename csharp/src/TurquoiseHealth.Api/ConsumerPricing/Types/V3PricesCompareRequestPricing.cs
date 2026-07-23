// ReSharper disable NullableWarningSuppressionIsUsed
// ReSharper disable InconsistentNaming

using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using TurquoiseHealth.Api.Core;

namespace TurquoiseHealth.Api;

[JsonConverter(typeof(V3PricesCompareRequestPricing.JsonConverter))]
[Serializable]
public record V3PricesCompareRequestPricing
{
    internal V3PricesCompareRequestPricing(string type, object? value)
    {
        Type = type;
        Value = value;
    }

    /// <summary>
    /// Create an instance of V3PricesCompareRequestPricing with <see cref="V3PricesCompareRequestPricing.Cash"/>.
    /// </summary>
    public V3PricesCompareRequestPricing(V3PricesCompareRequestPricing.Cash value)
    {
        Type = "cash";
        Value = value.Value;
    }

    /// <summary>
    /// Create an instance of V3PricesCompareRequestPricing with <see cref="V3PricesCompareRequestPricing.Negotiated"/>.
    /// </summary>
    public V3PricesCompareRequestPricing(V3PricesCompareRequestPricing.Negotiated value)
    {
        Type = "negotiated";
        Value = value.Value;
    }

    /// <summary>
    /// Discriminant value
    /// </summary>
    [JsonPropertyName("type")]
    public string Type { get; internal set; }

    /// <summary>
    /// Discriminated union value
    /// </summary>
    public object? Value { get; internal set; }

    /// <summary>
    /// Returns true if <see cref="Type"/> is "cash"
    /// </summary>
    public bool IsCash => Type == "cash";

    /// <summary>
    /// Returns true if <see cref="Type"/> is "negotiated"
    /// </summary>
    public bool IsNegotiated => Type == "negotiated";

    /// <summary>
    /// Returns the value as a <see cref="TurquoiseHealth.Api.V3PricingCash"/> if <see cref="Type"/> is 'cash', otherwise throws an exception.
    /// </summary>
    /// <exception cref="Exception">Thrown when <see cref="Type"/> is not 'cash'.</exception>
    public TurquoiseHealth.Api.V3PricingCash AsCash() =>
        IsCash
            ? (TurquoiseHealth.Api.V3PricingCash)Value!
            : throw new System.Exception("V3PricesCompareRequestPricing.Type is not 'cash'");

    /// <summary>
    /// Returns the value as a <see cref="TurquoiseHealth.Api.V3PricingNegotiated"/> if <see cref="Type"/> is 'negotiated', otherwise throws an exception.
    /// </summary>
    /// <exception cref="Exception">Thrown when <see cref="Type"/> is not 'negotiated'.</exception>
    public TurquoiseHealth.Api.V3PricingNegotiated AsNegotiated() =>
        IsNegotiated
            ? (TurquoiseHealth.Api.V3PricingNegotiated)Value!
            : throw new System.Exception("V3PricesCompareRequestPricing.Type is not 'negotiated'");

    public T Match<T>(
        Func<TurquoiseHealth.Api.V3PricingCash, T> onCash,
        Func<TurquoiseHealth.Api.V3PricingNegotiated, T> onNegotiated,
        Func<string, object?, T> onUnknown_
    )
    {
        return Type switch
        {
            "cash" => onCash(AsCash()),
            "negotiated" => onNegotiated(AsNegotiated()),
            _ => onUnknown_(Type, Value),
        };
    }

    public void Visit(
        Action<TurquoiseHealth.Api.V3PricingCash> onCash,
        Action<TurquoiseHealth.Api.V3PricingNegotiated> onNegotiated,
        Action<string, object?> onUnknown_
    )
    {
        switch (Type)
        {
            case "cash":
                onCash(AsCash());
                break;
            case "negotiated":
                onNegotiated(AsNegotiated());
                break;
            default:
                onUnknown_(Type, Value);
                break;
        }
    }

    /// <summary>
    /// Attempts to cast the value to a <see cref="TurquoiseHealth.Api.V3PricingCash"/> and returns true if successful.
    /// </summary>
    public bool TryAsCash(out TurquoiseHealth.Api.V3PricingCash? value)
    {
        if (Type == "cash")
        {
            value = (TurquoiseHealth.Api.V3PricingCash)Value!;
            return true;
        }
        value = null;
        return false;
    }

    /// <summary>
    /// Attempts to cast the value to a <see cref="TurquoiseHealth.Api.V3PricingNegotiated"/> and returns true if successful.
    /// </summary>
    public bool TryAsNegotiated(out TurquoiseHealth.Api.V3PricingNegotiated? value)
    {
        if (Type == "negotiated")
        {
            value = (TurquoiseHealth.Api.V3PricingNegotiated)Value!;
            return true;
        }
        value = null;
        return false;
    }

    public override string ToString() => JsonUtils.Serialize(this);

    public static implicit operator V3PricesCompareRequestPricing(
        V3PricesCompareRequestPricing.Cash value
    ) => new(value);

    public static implicit operator V3PricesCompareRequestPricing(
        V3PricesCompareRequestPricing.Negotiated value
    ) => new(value);

    [Serializable]
    internal sealed class JsonConverter : JsonConverter<V3PricesCompareRequestPricing>
    {
        public override bool CanConvert(System.Type typeToConvert) =>
            typeof(V3PricesCompareRequestPricing).IsAssignableFrom(typeToConvert);

        public override V3PricesCompareRequestPricing Read(
            ref Utf8JsonReader reader,
            System.Type typeToConvert,
            JsonSerializerOptions options
        )
        {
            var json = JsonElement.ParseValue(ref reader);
            if (!json.TryGetProperty("type", out var discriminatorElement))
            {
                throw new JsonException("Missing discriminator property 'type'");
            }
            if (discriminatorElement.ValueKind != JsonValueKind.String)
            {
                if (discriminatorElement.ValueKind == JsonValueKind.Null)
                {
                    throw new JsonException("Discriminator property 'type' is null");
                }

                throw new JsonException(
                    $"Discriminator property 'type' is not a string, instead is {discriminatorElement.ToString()}"
                );
            }

            var discriminator =
                discriminatorElement.GetString()
                ?? throw new JsonException("Discriminator property 'type' is null");

            var value = discriminator switch
            {
                "cash" => json.Deserialize<TurquoiseHealth.Api.V3PricingCash?>(options)
                    ?? throw new JsonException(
                        "Failed to deserialize TurquoiseHealth.Api.V3PricingCash"
                    ),
                "negotiated" => json.Deserialize<TurquoiseHealth.Api.V3PricingNegotiated?>(options)
                    ?? throw new JsonException(
                        "Failed to deserialize TurquoiseHealth.Api.V3PricingNegotiated"
                    ),
                _ => json.Deserialize<object?>(options),
            };
            return new V3PricesCompareRequestPricing(discriminator, value);
        }

        public override void Write(
            Utf8JsonWriter writer,
            V3PricesCompareRequestPricing value,
            JsonSerializerOptions options
        )
        {
            JsonNode json =
                value.Type switch
                {
                    "cash" => JsonSerializer.SerializeToNode(value.Value, options),
                    "negotiated" => JsonSerializer.SerializeToNode(value.Value, options),
                    _ => JsonSerializer.SerializeToNode(value.Value, options),
                } ?? new JsonObject();
            json["type"] = value.Type;
            json.WriteTo(writer, options);
        }
    }

    /// <summary>
    /// Discriminated union type for cash
    /// </summary>
    [Serializable]
    public struct Cash
    {
        public Cash(TurquoiseHealth.Api.V3PricingCash value)
        {
            Value = value;
        }

        internal TurquoiseHealth.Api.V3PricingCash Value { get; set; }

        public override string ToString() => Value.ToString() ?? "null";

        public static implicit operator V3PricesCompareRequestPricing.Cash(
            TurquoiseHealth.Api.V3PricingCash value
        ) => new(value);
    }

    /// <summary>
    /// Discriminated union type for negotiated
    /// </summary>
    [Serializable]
    public struct Negotiated
    {
        public Negotiated(TurquoiseHealth.Api.V3PricingNegotiated value)
        {
            Value = value;
        }

        internal TurquoiseHealth.Api.V3PricingNegotiated Value { get; set; }

        public override string ToString() => Value.ToString() ?? "null";

        public static implicit operator V3PricesCompareRequestPricing.Negotiated(
            TurquoiseHealth.Api.V3PricingNegotiated value
        ) => new(value);
    }
}
