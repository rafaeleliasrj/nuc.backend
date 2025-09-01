using System.Diagnostics;
using System.Linq.Expressions;
using Avvo.Core.Commons.Exceptions;
using Avvo.Core.Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Avvo.Core.Data.EntityFramework.Repositories;

/// <summary>
/// Classe base para obtenção da primeira entidade ou padrão com base em um predicado.
/// </summary>
/// <typeparam name="TEntity">O tipo da entidade.</typeparam>
public abstract class FirstOrDefaultRepositoryBase<TEntity> : IFirstOrDefaultRepository<TEntity> where TEntity : class
{
    protected ILogger Logger { get; }
    protected ActivitySource ActivitySource { get; }

    /// <summary>
    /// Inicializa uma nova instância de <see cref="FirstOrDefaultRepositoryBase{TEntity}"/>.
    /// </summary>
    /// <param name="logger">O logger para registro de eventos.</param>
    /// <param name="activitySource">A fonte de atividades para rastreamento.</param>
    /// <exception cref="ArgumentNullException">Lançada se logger ou activitySource forem nulos.</exception>
    protected FirstOrDefaultRepositoryBase(ILogger logger, ActivitySource activitySource)
    {
        Logger = logger ?? throw new ArgumentNullException(nameof(logger));
        ActivitySource = activitySource ?? throw new ArgumentNullException(nameof(activitySource));
    }

    /// <summary>
    /// Obtém a primeira entidade que satisfaz o predicado ou retorna nulo.
    /// </summary>
    /// <param name="dbContext">O contexto do banco de dados.</param>
    /// <param name="predicate">O predicado para filtrar entidades.</param>
    /// <returns>Um resultado com a entidade encontrada ou nulo.</returns>
    public virtual async Task<TEntity?> ExecuteAsync(DbContext dbContext, Expression<Func<TEntity, bool>> predicate)
    {
        if (dbContext == null)
            throw new HttpStatusException(System.Net.HttpStatusCode.BadRequest, "O contexto do banco de dados não pode ser nulo.", "E400");
        if (predicate == null)
            throw new HttpStatusException(System.Net.HttpStatusCode.BadRequest, "O predicado não pode ser nulo.", "E400");

        using var activity = ActivitySource.StartActivity($"{GetType().Name}_{nameof(ExecuteAsync)}", ActivityKind.Internal);
        activity?.AddTag("entity_type", typeof(TEntity).Name);

        try
        {
            var entity = await dbContext.Set<TEntity>().FirstOrDefaultAsync(predicate);
            return entity;
        }
        catch (Exception ex)
        {
            var errorMessage = $"Erro ao obter primeira entidade do tipo {typeof(TEntity).Name}: {ex.Message}";
            Logger.LogError(ex, errorMessage);
            activity?.SetStatus(ActivityStatusCode.Error, errorMessage);
            throw new DataBaseException(errorMessage, ex);
        }
    }
}
