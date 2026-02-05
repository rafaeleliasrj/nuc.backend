using Refit;
using NautiHub.Infrastructure.Gateways.Asaas.DTOs;

namespace NautiHub.Infrastructure.Gateways.Asaas;

/// <summary>
/// Interface para comunicação com API de Clientes do Asaas usando Refit
/// </summary>
public interface IAsaasCustomersApi
{
    /// <summary>
    /// Criar novo cliente
    /// </summary>
    [Post("/customers")]
    Task<ApiResponse<AsaasCustomer>> CreateCustomerAsync([Body] AsaasCreateCustomerRequest request);

    /// <summary>
    /// Buscar cliente por ID
    /// </summary>
    [Get("/customers/{id}")]
    Task<ApiResponse<AsaasCustomer>> GetCustomerAsync(string id);

    /// <summary>
    /// Listar clientes
    /// </summary>
    [Get("/customers")]
    Task<ApiResponse<List<AsaasCustomer>>> ListCustomersAsync([Query] int limit = 10, [Query] int offset = 0);

    /// <summary>
    /// Atualizar cliente
    /// </summary>
    [Post("/customers/{id}")]
    Task<ApiResponse<AsaasCustomer>> UpdateCustomerAsync(string id, [Body] AsaasCreateCustomerRequest request);

    /// <summary>
    /// Excluir cliente
    /// </summary>
    [Delete("/customers/{id}")]
    Task<ApiResponse<object>> DeleteCustomerAsync(string id);
}