using System.Linq.Expressions;
using System.Security.Cryptography;
using System.Text;

namespace NautiHub.Core.Extensions;

public static class StringExtension
{
    public static string RemoveSpecialCharacters(this string text)
    {
        string ret = text;

        if (string.IsNullOrEmpty(ret))
            return ret;

        ret = RemoveAccents(ret);
        ret = System.Text.RegularExpressions.Regex.Replace(ret, @"\t", " ");
        ret = System.Text.RegularExpressions.Regex.Replace(
            ret,
            string.Format(@"[^0-9a-zA-ZéúíóáÉÚÍÓÁèùìòàÈÙÌÒÀõãñÕÃÑêûîôâÊÛÎÔÂëÿüïöäËYÜÏÖÄçÇ{0}]+?"),
            string.Empty
        );
        ret = ret.Trim();

        ret = ret.Replace("  ", " ");

        return ret;
    }

    public static string RemoveAccents(this string text)
    {
        if (string.IsNullOrEmpty(text))
            return text;

        var sb = new System.Text.StringBuilder();
        for (int i = 0; i < text.Length; i++)
        {
            if (text[i] > 255)
                sb.Append(text[i]);
            else
                sb.Append(textoComAcentoRemovido[text[i]]);
        }

        return sb.ToString();
    }

    public static string OnlyNumbers(this string text)
    {
        if (string.IsNullOrEmpty(text))
            return text;

        string ret = System.Text.RegularExpressions.Regex.Replace(text, @"[^0-9]+?", string.Empty);

        return ret != null ? ret.Trim() : ret!;
    }

    public static string RemoveHtmlTags(this string text)
    {
        if (text == null)
            return null!;

        text = System.Text.RegularExpressions.Regex.Replace(text, @"<[^>]*>", String.Empty);
        return text;
    }

    private static readonly char[] textoComAcentoRemovido = RemoveAccents();

    private static char[] RemoveAccents()
    {
        char[] accents = new char[256];

        for (int i = 0; i < 256; i++)
            accents[i] = (char)i;

        accents[(byte)'á'] =
            accents[(byte)'à'] =
            accents[(byte)'ã'] =
            accents[(byte)'â'] =
            accents[(byte)'ä'] =
                'a';
        accents[(byte)'Á'] =
            accents[(byte)'À'] =
            accents[(byte)'Ã'] =
            accents[(byte)'Â'] =
            accents[(byte)'Ä'] =
                'A';

        accents[(byte)'é'] = accents[(byte)'è'] = accents[(byte)'ê'] = accents[(byte)'ë'] = 'e';
        accents[(byte)'É'] = accents[(byte)'È'] = accents[(byte)'Ê'] = accents[(byte)'Ë'] = 'E';

        accents[(byte)'í'] = accents[(byte)'ì'] = accents[(byte)'î'] = accents[(byte)'ï'] = 'i';
        accents[(byte)'Í'] = accents[(byte)'Ì'] = accents[(byte)'Î'] = accents[(byte)'Ï'] = 'I';

        accents[(byte)'ó'] =
            accents[(byte)'ò'] =
            accents[(byte)'ô'] =
            accents[(byte)'õ'] =
            accents[(byte)'ö'] =
                'o';
        accents[(byte)'Ó'] =
            accents[(byte)'Ò'] =
            accents[(byte)'Ô'] =
            accents[(byte)'Õ'] =
            accents[(byte)'Ö'] =
                'O';

        accents[(byte)'ú'] = accents[(byte)'ù'] = accents[(byte)'û'] = accents[(byte)'ü'] = 'u';
        accents[(byte)'Ú'] = accents[(byte)'Ù'] = accents[(byte)'Û'] = accents[(byte)'Ü'] = 'U';

        accents[(byte)'ç'] = 'c';
        accents[(byte)'Ç'] = 'C';

        accents[(byte)'ñ'] = 'n';
        accents[(byte)'Ñ'] = 'N';

        accents[(byte)'ÿ'] = accents[(byte)'ý'] = 'y';
        accents[(byte)'Ý'] = 'Y';

        return accents;
    }

    public static string PrimeiroCaracterMaiusculo(this string entrada)
    {
        if (string.IsNullOrEmpty(entrada))
            return entrada;

        return entrada.First().ToString().ToUpper() + entrada.Substring(1);
    }

    public static Dictionary<Expression<Func<T, object>>, string>? OrdernarParaDicionario<T>(
        this string entrada
    )
    {
        if (string.IsNullOrEmpty(entrada))
            return null;

        var dicionario = new Dictionary<Expression<Func<T, object>>, string>();
        var matriz = entrada.Split(',');
        foreach (var objeto in matriz)
        {
            var propriedade = objeto.Split('.')[0].PrimeiroCaracterMaiusculo();
            var texto = objeto.Split('.')[1].ToUpper();
            if (texto == "ASC" || texto == "DESC")
                dicionario.Add(ObterLambda<T>(propriedade), texto);
        }

        return dicionario;
    }

    public static Expression<Func<T, object>> ObterLambda<T>(this string propriedade)
    {
        ParameterExpression parametro = Expression.Parameter(typeof(T), "p");
        Expression expressao = parametro;
        var matriz = propriedade.Split('.');
        foreach (var nomeDoCampo in matriz)
        {
            expressao = Expression.PropertyOrField(expressao, nomeDoCampo);
        }

        return (Expression<Func<T, object>>)
            Expression.Lambda(Expression.Convert(expressao, typeof(object)), parametro);
    }

    public static string FormatarDocumento(this string documento)
    {
        if (string.IsNullOrEmpty(documento))
            return "";

        documento = new string(documento.Where(char.IsDigit).ToArray());

        if (documento.Length == 11)
            return $"{documento.Substring(0, 3)}.{documento.Substring(3, 3)}.{documento.Substring(6, 3)}-{documento.Substring(9, 2)}";
        else if (documento.Length == 14)
            return $"{documento.Substring(0, 2)}.{documento.Substring(2, 3)}.{documento.Substring(5, 3)}/{documento.Substring(8, 4)}-{documento.Substring(12, 2)}";

        return documento;
    }

    public static string FormatarCep(this string cep)
    {
        if (string.IsNullOrEmpty(cep))
            return "";

        cep = new string(cep.Where(char.IsDigit).ToArray());

        if (cep.Length == 8)
            return $"{cep.Substring(0, 5)}-{cep.Substring(5, 3)}";

        return cep;
    }

    public static string FormatarChaveAcesso(this string chave)
    {
        if (string.IsNullOrEmpty(chave))
            return "";

        chave = new string(chave.Where(char.IsDigit).ToArray());

        if (chave.Length == 44)
        {
            string chaveFormatada = "";
            for (int i = 0; i < chave.Length; i += 4)
            {
                if (i + 4 <= chave.Length)
                    chaveFormatada += chave.Substring(i, 4) + " ";
                else
                    chaveFormatada += chave.Substring(i);
            }
            return chaveFormatada.Trim();
        }

        return chave;
    }

    public static string ZerosAEsquerda(this string valor, int tamanhoTotal)
    {
        if (valor == null)
            return null;

        return valor.PadLeft(tamanhoTotal, '0');
    }

    public static string ZerosAEsquerda(this int valor, int tamanhoTotal)
    {
        return valor.ToString().PadLeft(tamanhoTotal, '0');
    }

    public static string GerarHash(this string texto)
    {
        using var sha256 = SHA256.Create();
        var bytes = Encoding.UTF8.GetBytes(texto);
        var hash = sha256.ComputeHash(bytes);
        return Convert.ToBase64String(hash);
    }
}
