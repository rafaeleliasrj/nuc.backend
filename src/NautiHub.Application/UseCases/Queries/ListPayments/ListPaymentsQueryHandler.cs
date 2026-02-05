using MediatR;
using Microsoft.Extensions.Logging;
using FluentValidation.Results;
using NautiHub.Application.UseCases.Queries.ListPayments;
using NautiHub.Application.UseCases.Models.Responses;
using NautiHub.Core.Messages.Queries;
using NautiHub.Core.Messages.Models;
using NautiHub.Core.Resources;
using NautiHub.Domain.Entities;
using NautiHub.Domain.Enums;
using NautiHub.Infrastructure.Repositories;
using NautiHub.Domain.Repositories;

namespace NautiHub.Application.UseCases.Queries.ListPayments;

/// <summary>
/// Handler para listar pagamentos com filtros
/// </summary>
public class ListPaymentsQueryHandler(
    IPaymentRepository paymentRepository,
    ILogger<ListPaymentsQueryHandler> logger,
    MessagesService messagesService) : QueryHandler, IRequestHandler<ListPaymentsQuery, QueryResponse<ListPaymentsResponse>>
{
    private readonly IPaymentRepository _paymentRepository = paymentRepository;
    private readonly ILogger<ListPaymentsQueryHandler> _logger = logger;
    private readonly MessagesService _messagesService = messagesService;

    public async Task<QueryResponse<ListPaymentsResponse>> Handle(ListPaymentsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            // Buscar pagamentos com filtros
            IEnumerable<Payment> payments;
            
            if (request.BookingId.HasValue)
            {
                var bookingPayments = await _paymentRepository.GetByBookingIdAsync(request.BookingId.Value);
                payments = bookingPayments.AsEnumerable();
            }
            else
            {
                // Se não tiver filtros específicos, buscar todos (implementação futura)
                payments = new List<Payment>();
            }

            // Aplicar filtros adicionais
            if (!string.IsNullOrEmpty(request.Status))
            {
                if (Enum.TryParse<PaymentStatus>(request.Status, true, out var status))
                {
                    payments = payments.Where(p => p.Status == status);
                }
            }

            if (!string.IsNullOrEmpty(request.Method))
            {
                if (Enum.TryParse<PaymentMethod>(request.Method, true, out var method))
                {
                    payments = payments.Where(p => p.Method == method);
                }
            }

            // Paginação
            var totalCount = payments.Count();
            var pagedPayments = payments
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToList();

            // Montar response
            var paymentItems = pagedPayments.Select(p => new PaymentListItem
            {
                Id = p.Id,
                BookingId = p.BookingId,
                Value = p.Value,
                Method = p.Method.ToString(),
                Status = p.Status.ToString(),
                CreatedAt = p.CreatedAt ?? DateTime.Now,
                DueDate = p.DueDate,
                Description = p.Description
            }).ToList();

            var response = new ListPaymentsResponse
            {
                Payments = paymentItems,
                Pagination = new PaginationInfo
                {
                    Page = request.Page,
                    PageSize = request.PageSize,
                    TotalCount = totalCount,
                    TotalPages = (int)Math.Ceiling((double)totalCount / request.PageSize)
                }
            };

            return new QueryResponse<ListPaymentsResponse>(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao listar pagamentos");
            AddError(_messagesService.Payment_List_Error);
            return new QueryResponse<ListPaymentsResponse>(ValidationResult);
        }
    }
}