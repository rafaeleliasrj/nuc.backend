using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NautiHub.Domain.Entities;
using NautiHub.Domain.Services;
using NautiHub.Domain.Services.InfrastructureService.Message.Models.Responses;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace NautiHub.Infrastructure.Services.Message.Servers.Meta;

public class MetaStrategy : IMessageStrategy
{
    private readonly HttpClient _httpClient;
    private readonly IHostEnvironment _env;
    private readonly string _phoneNumberId;
    private readonly string _accessToken;
    private readonly ILogger<MetaStrategy> _logger;
    private readonly string _apiVersion = "v23.0";
    private readonly string _baseUrl = "https://graph.facebook.com";

    public MetaStrategy(ILogger<MetaStrategy> logger, HttpClient httpClient, IHostEnvironment env)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        _env = env;
        _phoneNumberId = Environment.GetEnvironmentVariable("META_PHONE_ID");
        _accessToken = Environment.GetEnvironmentVariable("META_TOKEN");
        _logger = logger;
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);
        _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    }

    public async Task<NotificationResposta> SendMessageAsync(string phone, string? body = null, Uri? documentUrl = null, string? documentName = null, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("[EnviarMensagem] - [PhoneNumberId: {PhoneNumberId}] - Meta API: Enviando mensagem para {To}", _phoneNumberId, phone);

            var requestBody = new
            {
                messaging_product = "whatsapp",
                recipient_type = "individual",
                to = phone,
                type = documentUrl != null ? "document" : "text",
                text = body != null ? new { body } : null,
                document = documentUrl != null ? new { link = documentUrl.ToString(), caption = body, filename = documentName ?? $"Documento_{DateTime.Now:yyyyMMdd_HHmmss}" } : null
            };

            var jsonOptions = new JsonSerializerOptions
            {
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            };

            var jsonContent = JsonSerializer.Serialize(requestBody, jsonOptions);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var endpoint = $"{_baseUrl}/{_apiVersion}/{_phoneNumberId}/messages";
            var response = await _httpClient.PostAsync(endpoint, content, cancellationToken);
            var responseContent = await response.Content.ReadAsStringAsync(cancellationToken);

            if (response.IsSuccessStatusCode)
            {
                var responseJson = JsonDocument.Parse(responseContent);
                var messageId = responseJson.RootElement
                    .GetProperty("messages")[0]
                    .GetProperty("id").GetString();

                return new NotificationResposta(true, messageId);
            }

            _logger.LogWarning("Erro ao enviar mensagem para {Telefone}: {StatusCode} - {Response}", phone, response.StatusCode, responseContent);
            return new NotificationResposta(false, null, $"Erro: {response.StatusCode} - {responseContent}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro inesperado ao enviar WhatsApp para {Telefone}", phone);
            return new NotificationResposta(false, null, ex.Message);
        }
    }

    public async Task<NotificationResposta> SendConsentAsync(User user, string phone, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("[EnviarMensagem] - [PhoneNumberId: {PhoneNumberId}] - Meta API: Enviando mensagem para {To}", _phoneNumberId, phone);

            var requestBody = new
            {
                messaging_product = "whatsapp",
                recipient_type = "individual",
                to = phone,
                type = "template",
                template = new
                {
                    name = "consentimento_demonstrativo_fechamento_caixa",
                    language = new { code = "pt_BR" },
                    components = new[]
                    {
                        new
                        {
                            type = "body",
                            parameters = new[]
                            {
                                new { type = "text", text = user.FullName }
                            }
                        }
                    }
                }
            };

            var jsonContent = JsonSerializer.Serialize(requestBody);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var endpoint = $"{_baseUrl}/{_apiVersion}/{_phoneNumberId}/messages";
            var response = await _httpClient.PostAsync(endpoint, content, cancellationToken);
            var responseContent = await response.Content.ReadAsStringAsync(cancellationToken);

            if (response.IsSuccessStatusCode)
            {
                var responseJson = JsonDocument.Parse(responseContent);
                var messageId = responseJson.RootElement
                    .GetProperty("messages")[0]
                    .GetProperty("id").GetString();

                return new NotificationResposta(true, messageId);
            }

            _logger.LogWarning("Erro ao enviar mensagem de opt-in para {Telefone}: {StatusCode} - {Response}", phone, response.StatusCode, responseContent);
            return new NotificationResposta(false, null, $"Erro: {response.StatusCode} - {responseContent}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro inesperado ao enviar mensagem de opt-in para {Telefone}", phone);
            return new NotificationResposta(false, null, ex.Message);
        }
    }
}
