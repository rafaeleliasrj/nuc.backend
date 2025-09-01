using System.Diagnostics;
using System.Linq.Expressions;
using Avvo.Core.Commons.Exceptions;
using Avvo.Core.Data.Interfaces;
using Avvo.Core.Data.Pagination;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Avvo.Core.Data.EntityFramework.Repositories;

/// <summary>
/// Classe base para obtenção paginada de entidades.
/// </summary>
/// <typeparam name="TEntity">O tipo da entidade.</typeparam>
public abstract class GetAllPagedRepositoryBase<TEntity> : IGetAllPagedRepository<TEntity> where TEntity : class
{
    protected ILogger Logger { get; }
    protected ActivitySource ActivitySource { get; }

    /// <summary>
    /// Inicializa uma nova instância de <see cref="GetAllPagedRepositoryBase{TEntity}"/>.
    /// </summary>
    /// <param name="logger">O logger para registro de eventos.</param>
    /// <param name="activitySource">A fonte de atividades para rastreamento.</param>
    /// <exception cref="ArgumentNullException">Lançada se logger ou activitySource forem nulos.</exception>
    protected GetAllPagedRepositoryBase(ILogger logger, ActivitySource activitySource)
    {
        Logger = logger ?? throw new ArgumentNullException(nameof(logger));
        ActivitySource = activitySource ?? throw new ArgumentNullException(nameof(activitySource));
    }

    /// <summary>
    /// Obtém uma lista paginada de entidades com base em um predicado e ordenação.
    /// </summary>
    /// <param name="dbContext">O contexto do banco de dados.</param>
    /// <param name="predicate">O predicado para filtrar entidades (opcional).</param>
    /// <param name="orderBy">Dicionário de ordenação (opcional).</param>
    /// <param name="page">Número da página (mínimo 1).</param>
    /// <param name="pageSize">Tamanho da página (mínimo 1).</param>
    /// <returns>Um resultado com a lista paginada de entidades.</returns>
    public virtual async Task<PagedResult<TEntity>> ExecuteAsync(DbContext dbContext, Expression<Func<TEntity, bool>>? predicate = null, IDictionary<Expression<Func<TEntity, object>>, string>? orderBy = null, int page = 1, int pageSize = 10)
    {
        if (dbContext == null)
            throw new HttpStatusException(System.Net.HttpStatusCode.BadRequest, "O contexto do banco de dados não pode ser nulo.", "E400");
        if (page < 1)
            throw new HttpStatusException(System.Net.HttpStatusCode.BadRequest, "O número da página deve ser maior que 0.", "E400");
        if (pageSize < 1)
            throw new HttpStatusException(System.Net.HttpStatusCode.BadRequest, "O tamanho da página deve ser maior que 0.", "E400");

        using var activity = ActivitySource.StartActivity($"{GetType().Name}_{nameof(ExecuteAsync)}", ActivityKind.Internal);
        activity?.AddTag("entity_type", typeof(TEntity).Name);
        activity?.AddTag("page", page.ToString());
        activity?.AddTag("page_size", pageSize.ToString());

        try
        {
            var query = predicate != null ? dbContext.Set<TEntity>().Where(predicate) : dbContext.Set<TEntity>();
            query = PrepareQuery(query);
            return await query.GetPagedAsync(orderBy, page, pageSize);
        }
        catch (Exception ex)
        {
            var errorMessage = $"Erro ao obter entidades paginadas do tipo {typeof(TEntity).Name}: {ex.Message}";
            Logger.LogError(ex, errorMessage);
            activity?.SetStatus(ActivityStatusCode.Error, errorMessage);
            throw new DataBaseException(errorMessage, ex);
        }
    }

    /// <summary>
    /// Personaliza a query base para incluir joins ou outras configurações.
    /// </summary>
    /// <param name="query">A query base.</param>
    /// <returns>A query personalizada (sem ordenação ou limite de dados).</returns>
    protected virtual IQueryable<TEntity> PrepareQuery(IQueryable<TEntity> query) => query;
}
