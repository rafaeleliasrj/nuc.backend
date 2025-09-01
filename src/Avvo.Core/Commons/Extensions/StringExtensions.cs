using System.Linq.Expressions;
using System.Net;
using Avvo.Core.Commons.Exceptions;


namespace Avvo.Core.Commons.Extensions;

/// <summary>
/// Fornece métodos de extensão para manipulação de strings.
/// </summary>
public static class StringExtensions
{
    /// <summary>
    /// Capitaliza a primeira letra da string, mantendo o restante inalterado.
    /// </summary>
    /// <param name="input">A string de entrada.</param>
    /// <returns>A string com a primeira letra em maiúscula ou a string original se nula ou vazia.</returns>
    public static string FirstCharToUpper(this string? input)
    {
        if (string.IsNullOrEmpty(input))
            return input ?? string.Empty;

        return char.ToUpper(input[0]) + input[1..];
    }

    /// <summary>
    /// Converte uma string de ordenação em um dicionário de expressões lambda para ordenação dinâmica.
    /// Exemplo: "Name.ASC,Age.DESC" gera um dicionário com expressões para "Name" (ascendente) e "Age" (descendente).
    /// </summary>
    /// <typeparam name="T">O tipo da entidade.</typeparam>
    /// <param name="input">A string de ordenação no formato "Propriedade.Direção,Propriedade.Direção".</param>
    /// <returns>Um resultado com um dicionário de expressões lambda e direções de ordenação.</returns>
    public static IReadOnlyDictionary<Expression<Func<T, object>>, string> ToOrderByDictionary<T>(this string? input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return new Dictionary<Expression<Func<T, object>>, string>().AsReadOnly();

        try
        {
            var values = new Dictionary<Expression<Func<T, object>>, string>();
            foreach (var param in input.Split(',', StringSplitOptions.RemoveEmptyEntries))
            {
                var parts = param.Split('.');
                if (parts.Length != 2)
                    throw new ArgumentException($"Formato inválido para ordenação: {param}. Esperado: Propriedade.Direção.", nameof(input));

                var property = parts[0].FirstCharToUpper();
                var sort = parts[1].ToUpper();

                if (sort != "ASC" && sort != "DESC")
                    throw new ArgumentException($"Direção inválida: {sort}. Deve ser 'ASC' ou 'DESC'.", nameof(input));

                values.Add(GetLambda<T>(property), sort);
            }

            return values.AsReadOnly();
        }
        catch (ArgumentException ex)
        {
            throw new HttpStatusException(HttpStatusCode.BadRequest, ex.Message, "E400");
        }
        catch (Exception ex)
        {
            throw new ServiceException("Erro ao processar string de ordenação.", ex);
        }
    }

    private static Expression<Func<T, object>> GetLambda<T>(string property)
    {
        var param = Expression.Parameter(typeof(T), "p");
        Expression body = param;
        foreach (var member in property.Split('.'))
        {
            body = Expression.PropertyOrField(body, member);
        }
        var convert = Expression.Convert(body, typeof(object));
        return Expression.Lambda<Func<T, object>>(convert, param);
    }

    /// <summary>
    /// Verifica se a string é nula ou vazia.
    /// </summary>
    /// <param name="value">A string a ser verificada.</param>
    /// <returns><c>true</c> se a string é nula ou vazia; caso contrário, <c>false</c>.</returns>
    public static bool IsNullOrEmpty(this string? value) => string.IsNullOrEmpty(value);

    /// <summary>
    /// Verifica se a string é nula, vazia ou contém apenas espaços em branco.
    /// </summary>
    /// <param name="value">A string a ser verificada.</param>
    /// <returns><c>true</c> se a string é nula, vazia ou contém apenas espaços; caso contrário, <c>false</c>.</returns>
    public static bool IsNullOrWhiteSpace(this string? value) => string.IsNullOrWhiteSpace(value);

    /// <summary>
    /// Verifica se a string contém um valor válido (não nulo, não vazio e não apenas espaços).
    /// </summary>
    /// <param name="value">A string a ser verificada.</param>
    /// <returns><c>true</c> se a string contém um valor válido; caso contrário, <c>false</c>.</returns>
    public static bool HasValue(this string? value) => !string.IsNullOrWhiteSpace(value);

    /// <summary>
    /// Extrai uma substring de forma segura, ajustando índices fora dos limites.
    /// </summary>
    /// <param name="value">A string de entrada.</param>
    /// <param name="startIndex">O índice inicial da substring.</param>
    /// <param name="length">O comprimento da substring.</param>
    /// <returns>A substring extraída ou uma string vazia se a entrada for inválida.</returns>
    public static string SubstringSafe(this string? value, int startIndex, int length)
    {
        if (string.IsNullOrEmpty(value))
            return string.Empty;

        if (startIndex >= value.Length)
            return string.Empty;

        if (startIndex < 0)
            startIndex = 0;

        if (startIndex + length > value.Length)
            length = value.Length - startIndex;

        return value.Substring(startIndex, length);
    }

    /// <summary>
    /// Limita o tamanho da string a um número máximo de caracteres.
    /// </summary>
    /// <param name="value">A string de entrada.</param>
    /// <param name="length">O tamanho máximo da string resultante.</param>
    /// <returns>A string limitada ao tamanho especificado.</returns>
    public static string Truncate(this string? value, int length)
    {
        if (length < 0)
            throw new ArgumentException("O comprimento não pode ser negativo.", nameof(length));

        return value.SubstringSafe(0, length);
    }
}
