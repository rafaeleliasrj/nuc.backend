namespace NautiHub.Core.Extensions;

using System.Text.RegularExpressions;

public static class CnpjExtension
{
    private const int LengthCNPJWithoutDV = 12;
    private static readonly Regex RegexCNPJWithoutDV = new(@"^[A-Z\d]{12}$", RegexOptions.Compiled);
    private static readonly Regex RegexCNPJ = new(@"^[A-Z\d]{12}(\d){2}$", RegexOptions.Compiled);
    private static readonly Regex RegexCaracteresNaoPermitidos = new(@"[^A-Z\d./-]", RegexOptions.IgnoreCase | RegexOptions.Compiled);
    private static readonly int[] WeightDV = { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };

    public static bool CnpjIsValid(this string cnpj)
    {
        if (string.IsNullOrWhiteSpace(cnpj) || RegexCaracteresNaoPermitidos.IsMatch(cnpj))
            return false;

        var cnpjWithoutMask = RemoveMask(cnpj);

        if (!RegexCNPJ.IsMatch(cnpjWithoutMask) || AllCharactersAlike(cnpjWithoutMask))
            return false;

        var calculatedDV = CalculateDV(cnpjWithoutMask[..LengthCNPJWithoutDV]);
        var InformedDV = cnpjWithoutMask[LengthCNPJWithoutDV..];

        return InformedDV == calculatedDV;
    }

    private static string CalculateDV(string cnpj)
    {
        if (!RegexCNPJWithoutDV.IsMatch(cnpj) || AllCharactersAlike(cnpj))
            throw new ArgumentException("CNPJ base inválido para cálculo do DV");

        int sum1 = 0;
        int sum2 = 0;

        for (int i = 0; i < LengthCNPJWithoutDV; i++)
        {
            int digit = cnpj[i] - '0';
            sum1 += digit * WeightDV[i + 1];
            sum2 += digit * WeightDV[i];
        }

        int dv1 = sum1 % 11 < 2 ? 0 : 11 - (sum1 % 11);
        sum2 += dv1 * WeightDV[LengthCNPJWithoutDV];
        int dv2 = sum2 % 11 < 2 ? 0 : 11 - (sum2 % 11);

        return $"{dv1}{dv2}";
    }

    private static string RemoveMask(string cnpj)
    {
        return cnpj.Replace(".", "").Replace("-", "").Replace("/", "");
    }

    private static bool AllCharactersAlike(string input)
    {
        return input.All(c => c == input[0]);
    }
}
