namespace Avvo.Core.Commons.Extensions;

using System.Text.RegularExpressions;

public static class DocumentExtension
{
    private const int CnpjBaseLength = 12;
    private static readonly Regex RegexCnpjBase = new(@"^[A-Z\d]{12}$", RegexOptions.Compiled);
    private static readonly Regex RegexCnpj = new(@"^[A-Z\d]{12}(\d){2}$", RegexOptions.Compiled);
    private static readonly Regex RegexInvalidCharacters = new(@"[^A-Z\d./-]", RegexOptions.IgnoreCase | RegexOptions.Compiled);
    private static readonly int[] CheckDigitWeights = { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };

    public static bool IsValidCnpj(this string cnpj)
    {
        if (string.IsNullOrWhiteSpace(cnpj) || RegexInvalidCharacters.IsMatch(cnpj))
            return false;

        var unmaskedCnpj = RemoveMask(cnpj);

        if (!RegexCnpj.IsMatch(unmaskedCnpj) || AllCharactersEqual(unmaskedCnpj))
            return false;

        var calculatedCheckDigits = CalculateCheckDigits(unmaskedCnpj[..CnpjBaseLength]);
        var providedCheckDigits = unmaskedCnpj[CnpjBaseLength..];

        return providedCheckDigits == calculatedCheckDigits;
    }

    public static bool IsValidCpf(this string cpf)
    {
        if (cpf == null)
        {
            return false;
        }

        cpf = cpf.Trim();
        cpf = cpf.Replace(".", "").Replace("-", "");

        if (
            cpf.Length != 11
            || cpf == "00000000000"
            || cpf == "11111111111"
            || cpf == "22222222222"
            || cpf == "33333333333"
            || cpf == "44444444444"
            || cpf == "55555555555"
            || cpf == "66666666666"
            || cpf == "77777777777"
            || cpf == "88888888888"
            || cpf == "99999999999"
        )
        {
            return false;
        }

        var multiplier1 = new[] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
        var multiplier2 = new[] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };

        var tempCpf = cpf.Substring(0, 9);
        var sum = 0;
        for (var i = 0; i < 9; i++)
            sum += int.Parse(tempCpf[i].ToString()) * multiplier1[i];

        var remainder = sum % 11;
        if (remainder < 2)
            remainder = 0;
        else
            remainder = 11 - remainder;

        var digit = remainder.ToString();

        tempCpf = tempCpf + digit;

        sum = 0;
        for (var i = 0; i < 10; i++)
            sum += int.Parse(tempCpf[i].ToString()) * multiplier2[i];

        remainder = sum % 11;
        if (remainder < 2)
            remainder = 0;
        else
            remainder = 11 - remainder;

        digit = digit + remainder;

        return cpf.EndsWith(digit);
    }

    private static string CalculateCheckDigits(string cnpj)
    {
        if (!RegexCnpjBase.IsMatch(cnpj) || AllCharactersEqual(cnpj))
            throw new ArgumentException("CNPJ base inválido para cálculo do DV");

        int sum1 = 0;
        int sum2 = 0;

        for (int i = 0; i < CnpjBaseLength; i++)
        {
            int digit = cnpj[i] - '0';
            sum1 += digit * CheckDigitWeights[i + 1];
            sum2 += digit * CheckDigitWeights[i];
        }

        int checkDigit1 = sum1 % 11 < 2 ? 0 : 11 - (sum1 % 11);
        sum2 += checkDigit1 * CheckDigitWeights[CnpjBaseLength];
        int checkDigit2 = sum2 % 11 < 2 ? 0 : 11 - (sum2 % 11);

        return $"{checkDigit1}{checkDigit2}";
    }

    private static string RemoveMask(string cnpj)
    {
        return cnpj.Replace(".", "").Replace("-", "").Replace("/", "");
    }

    private static bool AllCharactersEqual(string input)
    {
        return input.All(c => c == input[0]);
    }

    public static string FormatDocument(this string document)
    {
        if (string.IsNullOrWhiteSpace(document))
            return string.Empty;

        var raw = new string(document.Where(c => !char.IsWhiteSpace(c)).ToArray());

        if (raw.Length == 11)
            return $"{raw.Substring(0, 3)}.{raw.Substring(3, 3)}.{raw.Substring(6, 3)}-{raw.Substring(9, 2)}";

        if (raw.Length == 14)
            return $"{raw.Substring(0, 2)}.{raw.Substring(2, 3)}.{raw.Substring(5, 3)}/{raw.Substring(8, 4)}-{raw.Substring(12, 2)}";

        return string.Empty;
    }
}
