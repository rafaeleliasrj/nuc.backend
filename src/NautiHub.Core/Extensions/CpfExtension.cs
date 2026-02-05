namespace NautiHub.Core.Extensions;

public static class CpfExtension
{
    public static bool CpfIsValid(this string cpf)
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

        var rest = sum % 11;
        if (rest < 2)
            rest = 0;
        else
            rest = 11 - rest;

        var digit = rest.ToString();

        tempCpf = tempCpf + digit;

        sum = 0;
        for (var i = 0; i < 10; i++)
            sum += int.Parse(tempCpf[i].ToString()) * multiplier2[i];

        rest = sum % 11;
        if (rest < 2)
            rest = 0;
        else
            rest = 11 - rest;

        digit = digit + rest;

        return cpf.EndsWith(digit);
    }

    public static string FormatCPF(this string cpf)
    {
        if (cpf.Length < 11)
        {
            return "";
        }

        return $"{cpf.Substring(0, 3)}.{cpf.Substring(3, 3)}.{cpf.Substring(6, 3)}-{cpf.Substring(9, 2)}";
    }
}
