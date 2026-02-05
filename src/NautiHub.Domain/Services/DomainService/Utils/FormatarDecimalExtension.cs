using System.Globalization;

namespace NautiHub.Domain.Services.DomainService.Utilitarios;

public static class FormatarDecimalExtension
{
    public static string ConverterParaStringFormatado(this decimal value)
    {
        return value
            .ToString("N2", CultureInfo.InvariantCulture)
            .Replace(",", "#")
            .Replace(".", ",")
            .Replace("#", ".");
    }
}
