using System.Diagnostics;
using Avvo.Core.Commons.Exceptions;
using Avvo.Core.Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Avvo.Core.Data.EntityFramework.Repositories;

/// <summary>
/// Classe base para atualização de múltiplas entidades.
/// </summary>
/// <typeparam name="TEntity">O tipo da entidade.</typeparam>
public abstract class UpdateRangeRepositoryBase<TEntity> : IUpdateRangeRepository<TEntity> where TEntity : class
{
    protected ILogger Logger { get; }
    protected ActivitySource ActivitySource { get; }

    /// <summary>
    /// Inicializa uma nova instância de <see cref="UpdateRangeRepositoryBase{TEntity}"/>.
    /// </summary>
    /// <param name="logger">O logger para registro de eventos.</param>
    /// <param name="activitySource">A fonte de atividades para rastreamento.</param>
    /// <exception cref="ArgumentNullException">Lançada se logger ou activitySource forem nulos.</exception>
    protected UpdateRangeRepositoryBase(ILogger logger, ActivitySource activitySource)
    {
        Logger = logger ?? throw new ArgumentNullException(nameof(logger));
        ActivitySource = activitySource ?? throw new ArgumentNullException(nameof(activitySource));
    }

    /// <summary>
    /// Atualiza múltiplas entidades no banco de dados.
    /// </summary>
    /// <param name="dbContext">O contexto do banco de dados.</param>
    /// <param name="entities">A lista de entidades a serem atualizadas.</param>
    /// <returns>Um resultado com o número de registros afetados.</returns>
    public virtual async Task<int> ExecuteAsync(DbContext dbContext, IEnumerable<TEntity> entities)
    {
        if (dbContext == null)
            throw new ArgumentNullException(nameof(dbContext), "O contexto do banco de dados não pode ser nulo.");
        if (entities == null || !entities.Any())
            throw new HttpStatusException(System.Net.HttpStatusCode.BadRequest, "A lista de entidades não pode ser nula ou vazia.", "E400");

        using var activity = ActivitySource.StartActivity($"{GetType().Name}_{nameof(ExecuteAsync)}", ActivityKind.Internal);
        activity?.AddTag("entity_type", typeof(TEntity).Name);
        activity?.AddTag("entity_count", entities.Count().ToString());

        try
        {
            dbContext.UpdateRange(entities);
            var result = await dbContext.SaveChangesAsync();
            return result;
        }
        catch (Exception ex)
        {
            var errorMessage = $"Erro ao atualizar múltiplas entidades do tipo {typeof(TEntity).Name}: {ex.Message}";
            Logger.LogError(ex, errorMessage);
            activity?.SetStatus(ActivityStatusCode.Error, errorMessage);
            throw new DataBaseException(errorMessage, ex);
        }
    }
}
