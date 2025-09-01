using System.Diagnostics;
using System.Net;
using Avvo.Core.Commons.Entities;
using Avvo.Core.Commons.Exceptions;
using Avvo.Core.Commons.Extensions;
using Avvo.Core.Commons.Interfaces;
using Avvo.Core.Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Avvo.Core.Data.EntityFramework.Repositories;

/// <summary>
/// Classe base para adição de entidades.
/// </summary>
/// <typeparam name="TEntity">O tipo da entidade.</typeparam>
public abstract class AddRepositoryBase<TEntity> : IAddRepository<TEntity> where TEntity : class
{
    protected ILogger Logger { get; }
    protected ActivitySource ActivitySource { get; }
    private readonly ICrudEventService _crudEventService;

    /// <summary>
    /// Inicializa uma nova instância de <see cref="AddRepositoryBase{TEntity}"/>.
    /// </summary>
    /// <param name="logger">O logger para registro de eventos.</param>
    /// <param name="activitySource">A fonte de atividades para rastreamento.</param>
    /// <param name="crudEventService">O serviço para propagação de eventos CRUD.</param>
    /// <exception cref="ArgumentNullException">Lançada se logger, activitySource ou crudEventService forem nulos.</exception>
    protected AddRepositoryBase(ILogger logger, ActivitySource activitySource, ICrudEventService crudEventService)
    {
        Logger = logger ?? throw new ArgumentNullException(nameof(logger));
        ActivitySource = activitySource ?? throw new ArgumentNullException(nameof(activitySource));
        _crudEventService = crudEventService ?? throw new ArgumentNullException(nameof(crudEventService));
    }

    /// <summary>
    /// Adiciona uma entidade ao banco de dados e propaga um evento CRUD.
    /// </summary>
    /// <param name="dbContext">O contexto do banco de dados.</param>
    /// <param name="entity">A entidade a ser adicionada.</param>
    /// <returns>Um resultado com a entidade adicionada.</returns>
    public virtual async Task<TEntity> ExecuteAsync(DbContext dbContext, TEntity entity)
    {
        if (dbContext == null)
            throw new HttpStatusException(HttpStatusCode.BadRequest, "O contexto do banco de dados não pode ser nulo.", "E400");
        if (entity == null)
            throw new HttpStatusException(HttpStatusCode.BadRequest, "A entidade não pode ser nula.", "E400");

        using var activity = ActivitySource.StartActivity($"{GetType().Name}_{nameof(ExecuteAsync)}", ActivityKind.Internal);
        activity?.AddTag("entity_type", typeof(TEntity).Name);

        try
        {
            await dbContext.Set<TEntity>().AddAsync(entity);
            await dbContext.SaveChangesAsync();

            var entityClone = _crudEventService.DeepClone(entity);

            await _crudEventService.ExecuteAsync(entityClone, CrudEventOperationEnum.Create);

            _crudEventService.CleanEventPropagation(entity);
            return entity;
        }
        catch (Exception ex)
        {
            var errorMessage = $"Erro ao adicionar entidade do tipo {typeof(TEntity).Name}: {ex.Message}";
            Logger.LogError(ex, errorMessage);
            activity?.SetStatus(ActivityStatusCode.Error, errorMessage);
            throw new DataBaseException(errorMessage, ex);
        }
    }
}
