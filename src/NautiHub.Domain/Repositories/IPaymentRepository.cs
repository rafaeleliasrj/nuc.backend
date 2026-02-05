using NautiHub.Core.Data;
using NautiHub.Domain.Entities;
using NautiHub.Domain.Enums;

namespace NautiHub.Domain.Repositories;

/// <summary>
/// Interface de repositório para pagamentos.
/// </summary>
public interface IPaymentRepository : IRepository<Payment>
{
    /// <summary>
    /// Obtém pagamentos por ID externo do Asaas.
    /// </summary>
    Task<Payment?> GetByIdAsync(Guid id);

    /// <summary>
    /// Obtém pagamentos por ID da reserva.
    /// </summary>
    Task<IEnumerable<Payment>> GetByBookingIdAsync(Guid bookingId);
    
    /// <summary>
    /// Obtém pagamentos por ID externo do Asaas.
    /// </summary>
    Task<Payment?> GetByAsaasPaymentIdAsync(string asaasPaymentId);
    
    /// <summary>
    /// Obtém pagamentos por status.
    /// </summary>
    Task<IEnumerable<Payment>> GetByStatusAsync(PaymentStatus status);
    
    /// <summary>
    /// Obtém pagamentos por método de pagamento.
    /// </summary>
    Task<IEnumerable<Payment>> GetByPaymentMethodAsync(PaymentMethod paymentMethod);
    
    /// <summary>
    /// Obtém pagamentos por período de data.
    /// </summary>
    Task<IEnumerable<Payment>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);
    
    /// <summary>
    /// Obtém pagamentos vencidos.
    /// </summary>
    Task<IEnumerable<Payment>> GetOverduePaymentsAsync();
    
    /// <summary>
    /// Obtém pagamentos pendentes.
    /// </summary>
    Task<IEnumerable<Payment>> GetPendingPaymentsAsync();
    
    /// <summary>
    /// Lista pagamentos com paginação e filtros.
    /// </summary>
    Task<(IEnumerable<Payment> items, int total)> ListAsync(
        int page = 1,
        int perPage = 10,
        string? search = null,
        Guid? bookingId = null,
        PaymentStatus? status = null,
        PaymentMethod? paymentMethod = null,
        DateTime? dueDateStart = null,
        DateTime? dueDateEnd = null,
        DateTime? createdAtStart = null,
        DateTime? createdAtEnd = null,
        string? orderBy = null);
}