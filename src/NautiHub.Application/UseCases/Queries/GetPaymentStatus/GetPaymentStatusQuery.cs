using NautiHub.Core.Messages.Queries;

namespace NautiHub.Application.UseCases.Models.Responses;

/// <summary>
/// Query para obter status de pagamento
/// </summary>
public class GetPaymentStatusQuery(Guid id) : Query<QueryResponse<GetPaymentStatusResponse>>
{
    /// <summary>
    /// ID do pagamento
    /// </summary>
    public Guid Id { get; set; } = id;
}