using System.Text.Json;
using Avvo.Core.Commons.Exceptions;

namespace Avvo.Core.Commons.Extensions;

/// <summary>
/// Fornece métodos de extensão para manipulação de objetos.
/// </summary>
public static class ObjectExtensions
{
    /// <summary>
    /// Realiza uma cópia profunda do objeto usando serialização JSON.
    /// Nota: Membros privados não são clonados por este método.
    /// </summary>
    /// <typeparam name="T">O tipo do objeto a ser copiado.</typeparam>
    /// <param name="source">A instância do objeto a ser copiada.</param>
    /// <returns>Um resultado com a cópia profunda do objeto ou um erro se a operação falhar.</returns>
    public static T Clone<T>(this T source)
    {
        if (source == null)
            return default!;

        try
        {
            var options = new JsonSerializerOptions
            {
                // Substitui objetos existentes para evitar valores padrão do construtor
                PropertyNameCaseInsensitive = true
            };

            var serialized = JsonSerializer.Serialize(source, options);
            var deserialized = JsonSerializer.Deserialize<T>(serialized, options);

            if (deserialized == null)
                throw new ServiceException("Falha ao desserializar o objeto clonado.");

            return deserialized;
        }
        catch (Exception ex)
        {
            throw new ServiceException("Erro ao clonar o objeto.", ex);
        }
    }
}
