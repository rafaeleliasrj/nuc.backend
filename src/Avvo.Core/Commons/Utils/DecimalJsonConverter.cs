using Newtonsoft.Json;

namespace Avvo.Core.Commons.Utils;

public class DecimalJsonConverter : JsonConverter
{
    private const int NUMERO_CASAS_DECIMAIS = 2;

    public override void WriteJson(
        JsonWriter writer,
        object? value,
        JsonSerializer serializer
    )
    {
        if (value is decimal decimalValue)
            writer.WriteValue(decimal.Round(decimalValue, NUMERO_CASAS_DECIMAIS));
        else if (value is float floatValue)
            writer.WriteValue(float.Round(floatValue, NUMERO_CASAS_DECIMAIS));
        else if (value is double doubleValue)
            writer.WriteValue(double.Round(doubleValue, NUMERO_CASAS_DECIMAIS));
        else
            throw new JsonSerializationException("Expected decimal value.");
    }

    public override object ReadJson(
        JsonReader reader,
        Type objectType,
        object? existingValue,
        Newtonsoft.Json.JsonSerializer serializer
    )
    {
        if (reader.TokenType == JsonToken.Null)
            return null!;

        if (reader.TokenType == JsonToken.Float || reader.TokenType == JsonToken.Integer)
        {
            var value = Convert.ToDecimal(reader.Value);
            return decimal.Round(value, NUMERO_CASAS_DECIMAIS);
        }

        throw new JsonSerializationException($"Unexpected token type: {reader.TokenType}");
    }

    public override bool CanConvert(Type objectType)
    {
        return objectType == typeof(decimal)
            || objectType == typeof(decimal?)
            || objectType == typeof(float)
            || objectType == typeof(float?)
            || objectType == typeof(double)
            || objectType == typeof(double?);
    }
}
