using Microsoft.Extensions.Logging;
using NautiHub.Domain.Entities;
using NautiHub.Domain.Services;
using NautiHub.Domain.Services.InfrastructureService.Message.Models.Responses;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Twilio;
using Twilio.Exceptions;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace NautiHub.Infrastructure.Services.Message.Servers.Twilio;

public class TwilioStrategy : IMessageStrategy
{
    private readonly HttpClient _httpClient;
    private readonly string _sid;
    private readonly string _token;
    private readonly string _from;
    private readonly ILogger<TwilioStrategy> _logger;


    public TwilioStrategy(ILogger<TwilioStrategy> logger, HttpClient httpClient)
    {
        _httpClient = httpClient;
        _sid = Environment.GetEnvironmentVariable("TWILIO_ACCOUNT_SID") ?? throw new ArgumentNullException("TWILIO_ACCOUNT_SID não configurado");
        _token = Environment.GetEnvironmentVariable("TWILIO_AUTHTOKEN") ?? throw new ArgumentNullException("TWILIO_AUTHTOKEN não configurado");
        _from = Environment.GetEnvironmentVariable("TWILIO_FROM_NUMBER") ?? throw new ArgumentNullException("TWILIO_FROM_NUMBER não configurado");
        _logger = logger;
        TwilioClient.Init(_sid, _token);
        var byteArray = Encoding.ASCII.GetBytes($"{_sid}:{_token}");
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
    }


    public async Task<NotificationResposta> SendMessageAsync(string phone, string? body = null, Uri? documentUrl = null, string? documentName = null, CancellationToken cancellationToken = default)
    {
        try
        {
            var to = new PhoneNumber("whatsapp:+" + phone);
            var from = new PhoneNumber("whatsapp:+" + _from);
            var options = new CreateMessageOptions(to)
            {
                From = from,
                Body = body
            };

            if (documentUrl is not null)
                options.MediaUrl = new List<Uri> { documentUrl };

            var message = await MessageResource.CreateAsync(options);

            return new NotificationResposta(true, message.Sid);
        }
        catch (ApiException apiEx)
        {
            _logger.LogWarning(apiEx, "Twilio API error ao enviar WhatsApp para {To}", phone);
            return new NotificationResposta(false, null, apiEx.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro inesperado ao enviar WhatsApp para {To}", phone);
            return new NotificationResposta(false, null, ex.Message);
        }
    }

    public async Task<NotificationResposta> SendConsentAsync(User user, string telefone, CancellationToken cancellationToken = default)
    {
        try
        {
            var url = $"https://api.twilio.com/2010-04-01/Accounts/{_sid}/Messages.json";

            var variables = new Dictionary<string, string>
                {
                    { "1", user.FullName }
                };

            var payload = new Dictionary<string, string>
                {
                    { "To", $"whatsapp:+{telefone}" },
                    { "From", $"whatsapp:+{_from}" },
                    { "ContentSid", Environment.GetEnvironmentVariable("TWILIO_OPTIN_CONTENTSID") ?? "HXb3fb2835b483e6b965b7b338fcc5abfc" },
                    { "ContentVariables", JsonSerializer.Serialize(variables) }
                };

            var content = new FormUrlEncodedContent(payload);
            var response = await _httpClient.PostAsync(url, content, cancellationToken);

            var result = await response.Content.ReadAsStringAsync(cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Erro ao enviar mensagem: {response.StatusCode} - {result}");
            }

            return new NotificationResposta(true, result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro inesperado ao enviar mensagem de opt-in para {To}", telefone);
            return new NotificationResposta(false, null, ex.Message);
        }
    }
}
