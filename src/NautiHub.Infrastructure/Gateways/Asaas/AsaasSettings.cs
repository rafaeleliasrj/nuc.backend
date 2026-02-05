namespace NautiHub.Infrastructure.Gateways.Asaas;

/// <summary>
/// Configurações do gateway de pagamentos Asaas
/// </summary>
public class AsaasSettings
{
    /// <summary>
    /// URL da API do Asaas
    /// </summary>
    public string BaseUrl { get; set; }

    /// <summary>
    /// Chave de API para autenticação
    /// </summary>
    public string ApiKey { get; set; }

    /// <summary>
    /// Ambiente de execução (Production ou Sandbox)
    /// </summary>
    public string Environment { get; set; }

    /// <summary>
    /// URL para webhook de pagamentos
    /// </summary>
    public string WebhookUrl { get; set; }

    /// <summary>
    /// Token para validação de webhooks
    /// </summary>
    public string WebhookToken { get; set; }

    /// <summary>
    /// Timeout em segundos para chamadas à API
    /// </summary>
    public int TimeoutInSeconds { get; set; } = 30;
}