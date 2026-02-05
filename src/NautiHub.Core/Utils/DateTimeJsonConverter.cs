using Newtonsoft.Json;
using System.Globalization;
using System.Text.RegularExpressions;

namespace NautiHub.Core.Utils;

public class DateTimeJsonConverter : Newtonsoft.Json.JsonConverter
{
    public override void WriteJson(
        JsonWriter writer,
        object? value,
        Newtonsoft.Json.JsonSerializer serializer
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

        if (string.IsNullOrWhiteSpace(dateString))
            return null;

        var cleaned = Regex.Replace(dateString, @"\s*\(.*\)$", "");

        if (DateTime.TryParseExact(
                cleaned,
                "ddd MMM dd yyyy HH:mm:ss 'GMT'K",
                CultureInfo.InvariantCulture,
                DateTimeStyles.AdjustToUniversal,
                out var jsDate))
        {
            return jsDate;
        }

        return dateString != null
            ? DateTime.Parse(dateString).ToUniversalTime()
            : (DateTime?)null;
    }

    public override bool CanConvert(Type objectType)
    {
        return objectType == typeof(DateTime) || objectType == typeof(DateTime?);
    }
}
