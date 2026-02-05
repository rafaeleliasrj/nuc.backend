using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using NautiHub.Application.UseCases.Events.PaymentHook;
using NautiHub.Core.MessageEvents;
using NautiHub.Infrastructure.Gateways.Asaas;
using NautiHub.Infrastructure.Gateways.Asaas.DTOs;
using NautiHub.Core.Resources;

namespace NautiHub.API.Controllers;

/// <summary>
/// Controller para receber webhooks do Asaas
/// </summary>
[ApiController]
[Route("api/webhooks/[controller]")]
public class HookController : ControllerBase
{
    private readonly IEventPublish _publish;
    private readonly ILogger<HookController> _logger;
    private readonly AsaasSettings _settings;
    private readonly MessagesService _messagesService;

    public HookController(
        IEventPublish publish,
        ILogger<HookController> logger,
        IOptions<AsaasSettings> settings,
        MessagesService messagesService)
    {
        _publish = publish;
        _logger = logger;
        _settings = settings.Value;
        _messagesService = messagesService;
    }

    /// <summary>
    /// Receber eventos de pagamento do Asaas
    /// </summary>
    [HttpPost("asaas")]
    public async Task<IActionResult> PaymentEvent([FromBody] AsaasWebhookEvent webhookEvent)
    {
        try
        {
            // Validar token do webhook
            var providedToken = Request.Headers["asaas-access-token"].FirstOrDefault();
            if (string.IsNullOrEmpty(providedToken) || providedToken != _settings.WebhookToken)
            {
                _logger.LogWarning("Token de webhook inválido: {Token}", providedToken);
                return Unauthorized();
            }

            _logger.LogInformation("Webhook recebido: {Event} - {PaymentId}", 
                webhookEvent.Event, webhookEvent.Payment?.Id);

            // Processar evento
            var @event = new ProcessWebhookEvent(webhookEvent);
            await _publish.Publish(@event);

            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao processar webhook do Asaas");
            return StatusCode(500, _messagesService?.Webhook_Internal_Error ?? "Erro interno ao processar webhook");
        }
    }
}