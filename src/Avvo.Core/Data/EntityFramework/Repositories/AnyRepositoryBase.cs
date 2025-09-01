using System.Diagnostics;
using System.Linq.Expressions;
using Avvo.Core.Commons.Exceptions;
using Avvo.Core.Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Avvo.Core.Data.EntityFramework.Repositories;

/// <summary>
/// Classe base para verificação de existência de entidades.
/// </summary>
/// <typeparam name="TEntity">O tipo da entidade.</typeparam>
public abstract class AnyRepositoryBase<TEntity> : IAnyRepository<TEntity> where TEntity : class
{
    protected ILogger Logger { get; }
    protected ActivitySource ActivitySource { get; }

    /// <summary>
    /// Inicializa uma nova instância de <see cref="AnyRepositoryBase{TEntity}"/>.
    /// </summary>
    /// <param name="logger">O logger para registro de eventos.</param>
    /// <param name="activitySource">A fonte de atividades para rastreamento.</param>
    /// <exception cref="ArgumentNullException">Lançada se logger ou activitySource forem nulos.</exception>
    protected AnyRepositoryBase(ILogger logger, ActivitySource activitySource)
    {
        Logger = logger ?? throw new ArgumentNullException(nameof(logger));
        ActivitySource = activitySource ?? throw new ArgumentNullException(nameof(activitySource));
    }

    /// <summary>
    /// Verifica a existência de entidades com base em um predicado.
    /// </summary>
    /// <param name="dbContext">O contexto do banco de dados.</param>
    /// <param name="predicate">O predicado para filtrar entidades (opcional).</param>
    /// <returns>Um resultado indicando se existe alguma entidade que satisfaça o predicado.</returns>
    public virtual async Task<bool> ExecuteAsync(DbContext dbContext, Expression<Func<TEntity, bool>>? predicate = null)
    {
        if (dbContext == null)
            throw new HttpStatusException(System.Net.HttpStatusCode.BadRequest, "O contexto do banco de dados não pode ser nulo.", "E400");

        using var activity = ActivitySource.StartActivity($"{GetType().Name}_{nameof(ExecuteAsync)}", ActivityKind.Internal);
        activity?.AddTag("entity_type", typeof(TEntity).Name);

        try
        {
            var result = predicate != null
                ? await dbContext.Set<TEntity>().AnyAsync(predicate)
                : await dbContext.Set<TEntity>().AnyAsync();

            return result;
        }
        catch (Exception ex)
        {
            var errorMessage = $"Erro ao verificar existência de entidades do tipo {typeof(TEntity).Name}: {ex.Message}";
            Logger.LogError(ex, errorMessage);
            activity?.SetStatus(ActivityStatusCode.Error, errorMessage);
            throw new DataBaseException(errorMessage, ex);
        }
    }
}
