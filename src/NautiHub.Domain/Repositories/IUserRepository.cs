using NautiHub.Core.Data;
using NautiHub.Core.Messages.Models;
using NautiHub.Domain.Entities;
using System.Threading.Tasks;

namespace NautiHub.Domain.Repositories;

/// <summary>
/// Interface do repositório de usuários
/// </summary>
public interface IUserRepository : IRepository<User>
{
    /// <summary>
    /// Lista usuários paginado
    /// </summary>
    /// <param name="search"></param>
    /// <param name="dateCreatedStart"></param>
    /// <param name="dateCreatedEnd"></param>
    /// <param name="dateUpdatedStart"></param>
    /// <param name="dateUpdatedEnd"></param>
    /// <param name="page"></param>
    /// <param name="perPage"></param>
    /// <param name="orderBy"></param>
    /// <returns></returns>
    public Task<ListPaginationResponse<User>> ListAsync(
        string? search = null,
        DateTime? dateCreatedStart = null,
        DateTime? dateCreatedEnd = null,
        DateTime? dateUpdatedStart = null,
        DateTime? dateUpdatedEnd = null,
        int page = 1,
        int perPage = 10,
        string? orderBy = null
    );

    /// <summary>
    /// Buscar usuário por ID
    /// </summary>
    Task<User> GetByIdAsync(Guid id);

    /// <summary>
    /// Buscar usuário por email
    /// </summary>
    Task<User> GetByEmailAsync(string email);

    /// <summary>
    /// Buscar usuário por username
    /// </summary>
    Task<User> GetByUserNameAsync(string userName);

    /// <summary>
    /// Verificar se email já existe
    /// </summary>
    Task<bool> EmailExistsAsync(string email);

    /// <summary>
    /// Verificar se username já existe
    /// </summary>
    Task<bool> UserNameExistsAsync(string userName);

    /// <summary>
    /// Buscar usuários por tipo
    /// </summary>
    Task<System.Collections.Generic.List<User>> GetByUserTypeAsync(Domain.Enums.UserType userType);
}