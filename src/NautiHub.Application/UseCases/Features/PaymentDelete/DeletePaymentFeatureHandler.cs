using Microsoft.Extensions.Logging;
using FluentValidation.Results;
using NautiHub.Application.UseCases.Models.Responses;
using NautiHub.Core.Messages.Features;
using NautiHub.Core.Resources;
using NautiHub.Domain.Entities;
using NautiHub.Domain.Repositories;
using System.Net;
using MediatR;
using NautiHub.Infrastructure.Repositories;
using NautiHub.Infrastructure.DataContext;

namespace NautiHub.Application.UseCases.Features.PaymentDelete;

/// <summary>
/// Handler para exclusão de pagamento
/// </summary>
public class DeletePaymentFeatureHandler : FeatureHandler, IRequestHandler<DeletePaymentFeature, FeatureResponse<bool>>
{
    private readonly DatabaseContext _context;
    private readonly IPaymentRepository _paymentRepository;
    private readonly ILogger<DeletePaymentFeatureHandler> _logger;
    private readonly MessagesService _messagesService;

    public DeletePaymentFeatureHandler(
        DatabaseContext context,
        IPaymentRepository paymentRepository,
        ILogger<DeletePaymentFeatureHandler> logger,
        MessagesService messagesService) : base(context)
    {
        _context = context;
        _paymentRepository = paymentRepository;
        _logger = logger;
        _messagesService = messagesService;
    }

    public async Task<FeatureResponse<bool>> Handle(DeletePaymentFeature request, CancellationToken cancellationToken)
    {
        try
        {
            // Buscar pagamento
            var payment = await _context.Set<Payment>().FindAsync(request.PaymentId);
            if (payment == null)
            {
                _logger.LogWarning("Pagamento {PaymentId} não encontrado", request.PaymentId);
                AddError(_messagesService.Payment_Not_Found);
                return new FeatureResponse<bool>(ValidationResult, statusCode: HttpStatusCode.NotFound);
            }

            // Pagamentos não podem ser excluídos diretamente
            // Apenas estornados através da integração com Asaas
            AddError(_messagesService.Payment_Refund_Not_Allowed);
            return new FeatureResponse<bool>(ValidationResult, statusCode: HttpStatusCode.BadRequest);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao tentar excluir pagamento {PaymentId}", request.PaymentId);
            AddError(_messagesService.Error_Internal_Server_Generic);
            return new FeatureResponse<bool>(ValidationResult, statusCode: HttpStatusCode.InternalServerError);
        }
    }
}