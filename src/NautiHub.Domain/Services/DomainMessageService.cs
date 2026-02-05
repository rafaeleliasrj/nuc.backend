using NautiHub.Core.Resources;

namespace NautiHub.Domain.Services;

/// <summary>
/// Serviço de domínio para resolução de mensagens localizadas
/// Mantém a pureza do domínio enquanto permite internacionalização
/// </summary>
public interface IDomainMessageService
{
    /// <summary>
    /// Resolve a mensagem localizada para uma chave específica
    /// </summary>
    /// <param name="messageKey">Chave da mensagem</param>
    /// <param name="args">Argumentos opcionais para formatação da mensagem</param>
    /// <returns>Mensagem localizada</returns>
    string GetMessage(string messageKey, params object[] args);

    /// <summary>
    /// Resolve a mensagem de uma exceção de domínio
    /// </summary>
    /// <param name="messageKey">Chave da mensagem na exceção</param>
    /// <param name="defaultMessage">Mensagem padrão caso não encontre</param>
    /// <param name="args">Argumentos opcionais para formatação</param>
    /// <returns>Mensagem localizada</returns>
    string GetExceptionMessage(string messageKey, string defaultMessage, params object[] args);
}

/// <summary>
/// Implementação do serviço de mensagens de domínio
/// Esta implementação usa dependência estática para manter o domínio puro
/// </summary>
public class DomainMessageService : IDomainMessageService
{
    private static MessagesService _messagesService;

    /// <summary>
    /// Configura o serviço de mensagens estaticamente
    /// Deve ser chamado na inicialização da aplicação
    /// </summary>
    /// <param name="messagesService">Instância do MessagesService</param>
    public static void Configure(MessagesService messagesService)
    {
        _messagesService = messagesService ?? throw new ArgumentNullException(nameof(messagesService));
    }

    public string GetMessage(string messageKey, params object[] args)
    {
        if (_messagesService == null)
            return messageKey;

        try
        {
            var message = _messagesService.GetType().GetProperty(messageKey)?.GetValue(_messagesService)?.ToString();
            
            if (string.IsNullOrEmpty(message))
                return messageKey;

            return args.Length > 0 ? string.Format(message, args) : message;
        }
        catch
        {
            return messageKey;
        }
    }

    public string GetExceptionMessage(string messageKey, string defaultMessage, params object[] args)
    {
        if (_messagesService == null)
            return args.Length > 0 ? string.Format(defaultMessage, args) : defaultMessage;

        try
        {
            var message = _messagesService.GetType().GetProperty(messageKey)?.GetValue(_messagesService)?.ToString();
            
            if (string.IsNullOrEmpty(message))
                return args.Length > 0 ? string.Format(defaultMessage, args) : defaultMessage;

            return args.Length > 0 ? string.Format(message, args) : message;
        }
        catch
        {
            return args.Length > 0 ? string.Format(defaultMessage, args) : defaultMessage;
        }
    }
}