using System.Diagnostics;
using Avvo.Core.Commons.Entities;
using Avvo.Core.Commons.Exceptions;
using Avvo.Core.Commons.Extensions;
using Avvo.Core.Commons.Interfaces;
using Avvo.Core.Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Avvo.Core.Data.EntityFramework.Repositories;

/// <summary>
/// Classe base para atualização de entidades.
/// </summary>
/// <typeparam name="TEntity">O tipo da entidade.</typeparam>
public abstract class UpdateRepositoryBase<TEntity> : IUpdateRepository<TEntity> where TEntity : class
{
    protected ILogger Logger { get; }
    protected ActivitySource ActivitySource { get; }
    private readonly ICrudEventService _crudEventService;

    /// <summary>
    /// Inicializa uma nova instância de <see cref="UpdateRepositoryBase{TEntity}"/>.
    /// </summary>
    /// <param name="logger">O logger para registro de eventos.</param>
    /// <param name="activitySource">A fonte de atividades para rastreamento.</param>
    /// <param name="crudEventService">O serviço para propagação de eventos CRUD.</param>
    /// <exception cref="ArgumentNullException">Lançada se logger, activitySource ou crudEventService forem nulos.</exception>
    protected UpdateRepositoryBase(ILogger logger, ActivitySource activitySource, ICrudEventService crudEventService)
    {
        Logger = logger ?? throw new ArgumentNullException(nameof(logger));
        ActivitySource = activitySource ?? throw new ArgumentNullException(nameof(activitySource));
        _crudEventService = crudEventService ?? throw new ArgumentNullException(nameof(crudEventService));
    }

    /// <summary>
    /// Atualiza uma entidade no banco de dados com base em seu ID e propaga um evento CRUD.
    /// </summary>
    /// <param name="dbContext">O contexto do banco de dados.</param>
    /// <param name="id">O ID da entidade a ser atualizada.</param>
    /// <param name="entity">A entidade com os novos valores.</param>
    /// <returns>Um resultado com o número de registros afetados.</returns>
    public virtual async Task<int> ExecuteAsync(DbContext dbContext, object id, TEntity entity)
    {
        if (dbContext == null)
            throw new HttpStatusException(System.Net.HttpStatusCode.BadRequest, "O contexto do banco de dados não pode ser nulo.", "E400");
        if (id == null)
            throw new HttpStatusException(System.Net.HttpStatusCode.BadRequest, "O ID não pode ser nulo.", "E400");
        if (entity == null)
            throw new HttpStatusException(System.Net.HttpStatusCode.BadRequest, "A entidade não pode ser nula.", "E400");

        using var activity = ActivitySource.StartActivity($"{GetType().Name}_{nameof(ExecuteAsync)}", ActivityKind.Internal);
        activity?.AddTag("entity_type", typeof(TEntity).Name);
        activity?.AddTag("entity_id", id.ToString());

        try
        {
            var existing = await dbContext.Set<TEntity>().FindAsync(id);
            if (existing == null)
                throw new NotFoundException($"Entidade do tipo {typeof(TEntity).Name} com ID {id} não encontrada.");

            var entityClone = _crudEventService.DeepClone(entity);

            dbContext.Entry(existing).CurrentValues.SetValues(entity);
            var result = await dbContext.SaveChangesAsync();

            await _crudEventService.ExecuteAsync(entityClone, CrudEventOperationEnum.Update);

            _crudEventService.CleanEventPropagation(entity);
            return result;
        }
        catch (NotFoundException ex)
        {
            Logger.LogError(ex, $"Erro ao atualizar entidade do tipo {typeof(TEntity).Name}: {ex.Message}");
            activity?.SetStatus(ActivityStatusCode.Error, ex.Message);
            throw;
        }
        catch (Exception ex)
        {
            var errorMessage = $"Erro ao atualizar entidade do tipo {typeof(TEntity).Name} com ID {id}: {ex.Message}";
            Logger.LogError(ex, errorMessage);
            activity?.SetStatus(ActivityStatusCode.Error, errorMessage);
            throw new DataBaseException(errorMessage, ex);
        }
    }
}
