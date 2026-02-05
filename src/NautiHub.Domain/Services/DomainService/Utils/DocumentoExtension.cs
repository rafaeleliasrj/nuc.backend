using System.Text.RegularExpressions;

namespace NautiHub.Domain.Services.DomainService.Utilitarios;

public static class DocumentoExtensions
{
    public static bool EhCpf(this string documento)
    {
        documento = SomenteDigitos(documento);
        if (documento.Length != 11 || DigitosRepetidos(documento))
            return false;

        int[] multiplicadores1 = { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
        int[] multiplicadores2 = { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };

        string baseCpf = documento[..9];
        string digito = CalcularDigito(baseCpf, multiplicadores1);
        digito += CalcularDigito(baseCpf + digito[0], multiplicadores2);

        return documento.EndsWith(digito);
    }

    public static bool EhCnpj(this string documento)
    {
        documento = SomenteDigitos(documento);
        if (documento.Length != 14 || DigitosRepetidos(documento))
            return false;

        int[] multiplicadores1 = { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
        int[] multiplicadores2 = { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };

        string baseCnpj = documento[..12];
        string digito = CalcularDigito(baseCnpj, multiplicadores1);
        digito += CalcularDigito(baseCnpj + digito[0], multiplicadores2);

        return documento.EndsWith(digito);
    }

    public static bool EhCpfOuCnpj(this string documento)
        => documento.EhCpf() || documento.EhCnpj();

    public static bool EhPessoaFisica(this string documento)
        => documento.EhCpf();

    public static bool EhPessoaJuridica(this string documento)
        => documento.EhCnpj();

    private static string SomenteDigitos(string texto)
        => Regex.Replace(texto ?? string.Empty, @"[^\d]", "");

    private static bool DigitosRepetidos(string texto)
        => new string(texto[0], texto.Length) == texto;

    private static string CalcularDigito(string baseNumero, int[] multiplicadores)
    {
        int soma = 0;
        for (int i = 0; i < multiplicadores.Length; i++)
            soma += int.Parse(baseNumero[i].ToString()) * multiplicadores[i];

        int resto = soma % 11;
        return (resto < 2 ? 0 : 11 - resto).ToString();
    }
}
