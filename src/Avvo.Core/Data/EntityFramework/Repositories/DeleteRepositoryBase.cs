using System.Diagnostics;
using Avvo.Core.Commons.Entities;
using Avvo.Core.Commons.Exceptions;
using Avvo.Core.Commons.Interfaces;
using Avvo.Core.Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Avvo.Core.Data.EntityFramework.Repositories;

/// <summary>
/// Classe base para exclusão de entidades.
/// </summary>
/// <typeparam name="TEntity">O tipo da entidade.</typeparam>
public abstract class DeleteRepositoryBase<TEntity> : IDeleteRepository<TEntity> where TEntity : class
{
    protected ILogger Logger { get; }
    protected ActivitySource ActivitySource { get; }
    private readonly ICrudEventService _crudEventService;

    /// <summary>
    /// Inicializa uma nova instância de <see cref="DeleteRepositoryBase{TEntity}"/>.
    /// </summary>
    /// <param name="logger">O logger para registro de eventos.</param>
    /// <param name="activitySource">A fonte de atividades para rastreamento.</param>
    /// <param name="crudEventService">O serviço para propagação de eventos CRUD.</param>
    /// <exception cref="ArgumentNullException">Lançada se logger, activitySource ou crudEventService forem nulos.</exception>
    protected DeleteRepositoryBase(ILogger logger, ActivitySource activitySource, ICrudEventService crudEventService)
    {
        Logger = logger ?? throw new ArgumentNullException(nameof(logger));
        ActivitySource = activitySource ?? throw new ArgumentNullException(nameof(activitySource));
        _crudEventService = crudEventService ?? throw new ArgumentNullException(nameof(crudEventService));
    }

    /// <summary>
    /// Exclui uma entidade do banco de dados com base em seu ID e propaga um evento CRUD.
    /// </summary>
    /// <param name="dbContext">O contexto do banco de dados.</param>
    /// <param name="id">O ID da entidade a ser excluída.</param>
    /// <param name="propagateEvent">Indica se o evento CRUD deve ser propagado.</param>
    /// <param name="propagateDestination">Destinos para propagação do evento (opcional).</param>
    /// <param name="overrideEntityName">Nome da entidade a ser sobrescrito no evento (opcional).</param>
    /// <returns>Um resultado com o número de registros afetados.</returns>
    public virtual async Task<int> ExecuteAsync(DbContext dbContext, object id, bool propagateEvent = false, IList<int>? propagateDestination = null, string? overrideEntityName = null)
    {
        if (dbContext == null)
            throw new HttpStatusException(System.Net.HttpStatusCode.BadRequest, "O contexto do banco de dados não pode ser nulo.", "E400");
        if (id == null)
            throw new HttpStatusException(System.Net.HttpStatusCode.BadRequest, "O ID não pode ser nulo.", "E400");

        using var activity = ActivitySource.StartActivity($"{GetType().Name}_{nameof(ExecuteAsync)}", ActivityKind.Internal);
        activity?.AddTag("entity_type", typeof(TEntity).Name);
        activity?.AddTag("entity_id", id.ToString());

        try
        {
            var existing = await dbContext.Set<TEntity>().FindAsync(id);
            if (existing == null)
                throw new NotFoundException($"Entidade do tipo {typeof(TEntity).Name} com ID {id} não encontrada.");

            if (existing is ICrudEventProcessor eventEntity)
            {
                eventEntity.PropagateEvent = propagateEvent;
                eventEntity.PropagateDestination = propagateDestination;
                eventEntity.OverrideEntityName = overrideEntityName;
            }

            dbContext.Set<TEntity>().Remove(existing);
            var result = await dbContext.SaveChangesAsync();

            await _crudEventService.ExecuteAsync(existing, CrudEventOperationEnum.Delete);

            return result;
        }
        catch (NotFoundException ex)
        {
            Logger.LogError(ex, $"Erro ao excluir entidade do tipo {typeof(TEntity).Name}: {ex.Message}");
            activity?.SetStatus(ActivityStatusCode.Error, ex.Message);
            throw;
        }
        catch (Exception ex)
        {
            var errorMessage = $"Erro ao excluir entidade do tipo {typeof(TEntity).Name} com ID {id}: {ex.Message}";
            Logger.LogError(ex, errorMessage);
            activity?.SetStatus(ActivityStatusCode.Error, errorMessage);
            throw new DataBaseException(errorMessage, ex);
        }
    }
}
