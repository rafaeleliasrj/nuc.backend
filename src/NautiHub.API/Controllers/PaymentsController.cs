using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NautiHub.Application.UseCases.Queries.PaymentById;
using NautiHub.Application.UseCases.Queries.ListPayments;
using NautiHub.Application.UseCases.Features.GeneratePixQrCode;
using NautiHub.Application.UseCases.Features.GenerateBoleto;
using NautiHub.Application.UseCases.Features.RefundPayment;
using NautiHub.Application.UseCases.Features.PaymentCreate;
using NautiHub.Application.UseCases.Models.Responses;
using NautiHub.Core.Controllers;
using NautiHub.Core.Mediator;
using NautiHub.Application.UseCases.Models.Requests;
using NautiHub.Core.Resources;

namespace NautiHub.API.Controllers;

/// <summary>
/// Controller para gerenciamento de pagamentos
/// </summary>
[ApiController]
[Route("api/v1/[controller]")]
[Authorize]
public class PaymentsController : MainController
{
    private readonly IMediatorHandler _mediator;
    private readonly ILogger<PaymentsController> _logger;

    public PaymentsController(IMediatorHandler mediator, ILogger<PaymentsController> logger, MessagesService messagesService) : base(mediator, messagesService)
    {
        _mediator = mediator;
        _logger = logger;
    }

    /// <summary>
    /// Criar um novo pagamento
    /// </summary>
    [HttpPost("create")]
    public async Task<IActionResult> CreateAsync([FromBody] CreatePaymentRequest request)
    {
        if (!ModelState.IsValid)
            return CustomResponse(ModelState);

        var result = await _mediator.ExecuteFeature(new CreatePaymentFeature
        {
            Data = request
        });

        if (result.ValidationResult.IsValid)
            return CustomResponse(result.ValidationResult, result.StatusCode);

        return CustomResponse(result.ValidationResult);
    }

    /// <summary>
    /// Buscar pagamento por ID
    /// </summary>
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetByIdAsync(Guid id)
    {
        var query = new GetPaymentByIdQuery(id);
        var result = await _mediator.ExecuteQuery<PaymentByIdResponse>(query);

        if (result.ValidationResult.IsValid)
            return CustomResponse(result.Response);

        return BadRequest(result.ValidationResult);
    }

    /// <summary>
    /// Listar pagamentos
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> ListAsync([FromQuery] Guid? bookingId = null, [FromQuery] string status = null, [FromQuery] string method = null, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        var query = new ListPaymentsQuery
        {
            BookingId = bookingId,
            Status = status,
            Method = method,
            Page = page,
            PageSize = pageSize
        };

        var result = await _mediator.ExecuteQuery(query);

        if (result.ValidationResult.IsValid)
            return CustomResponse(result.Response);

        return CustomResponse(result.ValidationResult);
    }

    /// <summary>
    /// Obter status de um pagamento
    /// </summary>
    [HttpGet("{id:guid}/status")]
    public async Task<IActionResult> GetStatusAsync(Guid id)
    {
        var query = new GetPaymentStatusQuery(id);
        var result = await _mediator.ExecuteQuery(query);

        if (result.ValidationResult.IsValid)
            return CustomResponse(result.Response);

        return CustomResponse(result.ValidationResult);
    }

    /// <summary>
    /// Gerar QR Code para Pix
    /// </summary>
    [HttpPost("{id:guid}/pix/qrcode")]
    public async Task<IActionResult> GeneratePixQrCodeAsync(Guid id)
    {
        var feature = new GeneratePixQrCodeFeature(id);
        var result = await _mediator.ExecuteFeature(feature);

        if (result.ValidationResult.IsValid)
            return CustomResponse(result.Response);

        return CustomResponse(result.ValidationResult);
    }

    /// <summary>
    /// Gerar PDF do boleto
    /// </summary>
    [HttpPost("{id:guid}/boleto")]
    public async Task<IActionResult> GenerateBoletoAsync(Guid id)
    {
        var feature = new GenerateBoletoFeature(id);
        var result = await _mediator.ExecuteFeature(feature);

        if (result.ValidationResult.IsValid)
            return CustomResponse(result.Response);

        return CustomResponse(result.ValidationResult);
    }

    /// <summary>
    /// Estornar pagamento
    /// </summary>
    [HttpPost("{id:guid}/refund")]
    public async Task<IActionResult> RefundAsync(Guid id, [FromBody] RefundRequest request)
    {
        var feature = new RefundPaymentFeature
        {
            PaymentId = id,
            Value = request.Value,
            Reason = request.Reason
        };
        var result = await _mediator.ExecuteFeature(feature);

        if (result.ValidationResult.IsValid)
            return CustomResponse(result.Response);

        return CustomResponse(result.ValidationResult);
    }
}