namespace NautiHub.Infrastructure.Services.Utilitarios;

public static class GtinExtensions
{
    public static bool GtinValido(this string gtin)
    {
        if (string.IsNullOrWhiteSpace(gtin) || !gtin.All(char.IsDigit))
            return false;

        if (!(gtin.Length is 8 or 12 or 13 or 14))
            return false;

        int[] digitos = gtin.Select(c => c - '0').ToArray();
        int digitoVerificador = digitos[^1];
        int calculado = CalcularDigitoVerificador(digitos[..^1]);

        return digitoVerificador == calculado;
    }

    public static string AdicionarDigito(string gtinParcial)
    {
        if (string.IsNullOrWhiteSpace(gtinParcial) || !gtinParcial.All(char.IsDigit))
            throw new ArgumentException("GTIN deve conter apenas dígitos.");

        if (!(gtinParcial.Length is 7 or 11 or 12 or 13))
            throw new ArgumentException("Tamanho inválido. Deve conter 7, 11, 12 ou 13 dígitos.");

        int[] digits = gtinParcial.Select(c => c - '0').ToArray();
        int digito = CalcularDigitoVerificador(digits);
        return gtinParcial + digito;
    }

    private static int CalcularDigitoVerificador(int[] digitos)
    {
        int soma = 0;
        for (int i = digitos.Length - 1, pos = 0; i >= 0; i--, pos++)
        {
            int multiplicadores = (pos % 2 == 0) ? 3 : 1;
            soma += digitos[i] * multiplicadores;
        }

        return (10 - (soma % 10)) % 10;
    }
}
