using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using NautiHub.Core.Resources;
using Refit;

namespace NautiHub.Infrastructure.Gateways.Asaas;

/// <summary>
/// Extensões para facilitar o uso do ApiResponse do Refit
/// </summary>
public static class ApiResponseExtensions
{
    /// <summary>
    /// Verifica se o ApiResponse foi bem sucedido e contém dados
    /// </summary>
    public static bool IsSuccess<T>(this ApiResponse<T> response)
    {
        return response.IsSuccessStatusCode && response.Content != null;
    }

    /// <summary>
    /// Obtém os dados do Content do ApiResponse
    /// </summary>
    public static T GetData<T>(this ApiResponse<T> response)
    {
        return response.Content;
    }

    /// <summary>
    /// Obtém as mensagens de erro do ApiResponse
    /// </summary>
    public static List<string> GetErrors<T>(this ApiResponse<T> response, IServiceProvider serviceProvider)
    {
        var messagesService = serviceProvider.GetRequiredService<MessagesService>();
        
        if (response.Error?.Content != null)
        {
            // Tenta fazer parse do erro JSON se existir
            try
            {
                var json = response.Error.Content.ToString();
                if (json.Contains("\"errors\""))
                {
                    // Parse simples para extrair erros
                    var errors = new List<string>();
                    var lines = json.Split(',');
                    foreach (var line in lines)
                    {
                        if (line.Contains("\"description\""))
                        {
                            var description = line.Split(':')[1]?.Trim().Trim('"', '}', ']');
                            if (!string.IsNullOrEmpty(description))
                                errors.Add(description);
                        }
                    }
                    return errors;
                }
            }
            catch
            {
                // Em caso de erro no parse, retorna mensagem genérica
            }
        }

        return new List<string> { response.Error?.Message ?? messagesService.System_Unknown_Error };
    }

    /// <summary>
    /// Obtém as mensagens de erro do ApiResponse (sobrecarga sem ServiceProvider para compatibilidade)
    /// </summary>
    public static List<string> GetErrors<T>(this ApiResponse<T> response)
    {
        if (response.Error?.Content != null)
        {
            // Tenta fazer parse do erro JSON se existir
            try
            {
                var json = response.Error.Content.ToString();
                if (json.Contains("\"errors\""))
                {
                    // Parse simples para extrair erros
                    var errors = new List<string>();
                    var lines = json.Split(',');
                    foreach (var line in lines)
                    {
                        if (line.Contains("\"description\""))
                        {
                            var description = line.Split(':')[1]?.Trim().Trim('"', '}', ']');
                            if (!string.IsNullOrEmpty(description))
                                errors.Add(description);
                        }
                    }
                    return errors;
                }
            }
            catch
            {
                // Em caso de erro no parse, retorna mensagem genérica
            }
        }

        return new List<string> { response.Error?.Message ?? "Erro desconhecido" };
    }
}