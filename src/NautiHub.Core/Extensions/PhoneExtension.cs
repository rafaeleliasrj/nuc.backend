using System.Text.RegularExpressions;

namespace NautiHub.Core.Extensions;

public static class PhoneExtensions
{
    public static string? GuaranteeCountryCode(this string phone)
    {
        if (string.IsNullOrWhiteSpace(phone))
            return null;

        phone = phone.Trim();

        if (phone.StartsWith("+"))
            return phone;

        return "55" + phone.OnlyNumbers();
    }

    public static bool ValidatePhone(this string? phone)
    {
        if (string.IsNullOrWhiteSpace(phone))
            return false;

        phone = phone.Trim();

        if (Regex.IsMatch(phone, @"^\+[1-9]\d{7,14}$"))
            return true;

        if (Regex.IsMatch(phone, @"^\d{8,15}$"))
        {
            if (phone.Length > 11 || phone.Length < 11)
                return false;

            return true;
        }

        return false;
    }
}
