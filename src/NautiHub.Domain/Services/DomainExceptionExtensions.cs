using NautiHub.Core.Resources;
using NautiHub.Core.DomainObjects;
using NautiHub.Domain.Exceptions;

namespace NautiHub.Domain.Services;

/// <summary>
/// Extensões para resolver mensagens de exceções de domínio
/// </summary>
public static class DomainExceptionExtensions
{
    /// <summary>
    /// Obtém a mensagem localizada de uma exceção de domínio
    /// </summary>
    /// <param name="exception">Exceção de domínio</param>
    /// <param name="messagesService">Serviço de mensagens</param>
    /// <returns>Mensagem localizada ou mensagem padrão</returns>
    public static string GetLocalizedMessage(this DomainException exception, MessagesService messagesService)
    {
        if (exception is null || messagesService is null)
            return exception?.Message ?? string.Empty;

        // Verificar se é uma exceção de domínio específica com MessageKey
        var exceptionType = exception.GetType();
        var messageKeyProperty = exceptionType.GetProperty("MessageKey");
        
        if (messageKeyProperty?.GetValue(exception) is string messageKey && !string.IsNullOrEmpty(messageKey))
        {
            try
            {
                var messageProperty = messagesService.GetType().GetProperty(messageKey);
                if (messageProperty?.GetValue(messagesService) is string localizedMessage)
                {
                    return localizedMessage;
                }
            }
            catch
            {
                // Em caso de erro, retorna a mensagem padrão
            }
        }

        return exception.Message;
    }

    /// <summary>
    /// Obtém a chave da mensagem de uma exceção de domínio
    /// </summary>
    /// <param name="exception">Exceção de domínio</param>
    /// <returns>Chave da mensagem ou null</returns>
    public static string? GetMessageKey(this DomainException exception)
    {
        if (exception is null)
            return null;

        var exceptionType = exception.GetType();
        var messageKeyProperty = exceptionType.GetProperty("MessageKey");
        
        return messageKeyProperty?.GetValue(exception) as string;
    }
}