using Newtonsoft.Json;

namespace Avvo.Core.Commons.Utils;

public class DateTimeJsonConverter : JsonConverter
{
    public override void WriteJson(
        JsonWriter writer,
        object? value,
        JsonSerializer serializer
    )
    {
        if (value is DateTime dateTime)
        {
            // Converte para UTC e formata com o sufixo "Z"
            writer.WriteValue(dateTime.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ss.fffZ"));
        }
        else
        {
            writer.WriteNull();
        }
    }

    public override object? ReadJson(
        JsonReader reader,
        Type objectType,
        object? existingValue,
        Newtonsoft.Json.JsonSerializer serializer
    )
    {
        var dateString = reader.Value?.ToString();
        return dateString != null
            ? DateTime.Parse(dateString).ToUniversalTime()
            : (DateTime?)null;
    }

    public override bool CanConvert(Type objectType)
    {
        return objectType == typeof(DateTime) || objectType == typeof(DateTime?);
    }
}
