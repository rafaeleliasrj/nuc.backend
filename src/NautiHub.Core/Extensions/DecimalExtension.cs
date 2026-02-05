using System.Globalization;

namespace NautiHub.Core.Extensions;

public static class DecimalExtension
{
    public static decimal Truncate(this decimal value, int precision)
    {
        string valueString = Math.Round(value, precision + 1).ToString($"N{precision + 1}");
        valueString = valueString[..^1];
        return decimal.Parse(valueString);
    }

    public static string FormatMoney(this decimal value)
    {
        NumberFormatInfo numberFormatInfo = new NumberFormatInfo
        {
            NumberDecimalSeparator = ",",
            NumberGroupSeparator = ".",
            NumberGroupSizes = [3]
        };

        return value.ToString("#,##0.00", numberFormatInfo);
    }

    public static string Format(this decimal value, int precisao)
    {
        if (precisao <= 0)
        {
            return value.Truncate(0).ToString();
        }

        return value
            .ToString($"0.{"".PadLeft(precisao, '0')}", CultureInfo.InvariantCulture)
            .Replace(".", ",");
    }
}
