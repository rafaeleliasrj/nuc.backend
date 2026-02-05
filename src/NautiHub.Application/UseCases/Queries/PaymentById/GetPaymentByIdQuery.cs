using NautiHub.Core.Messages.Queries;
using NautiHub.Application.UseCases.Models.Responses;

namespace NautiHub.Application.UseCases.Queries.PaymentById;

/// <summary>
/// Query para buscar pagamento por ID
/// </summary>
public class GetPaymentByIdQuery(Guid id) : Query<QueryResponse<PaymentByIdResponse>>
{
    /// <summary>
    /// ID do pagamento
    /// </summary>
    public Guid Id { get; set; } = id;
}