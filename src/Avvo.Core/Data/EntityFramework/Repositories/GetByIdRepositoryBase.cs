using System.Diagnostics;
using Avvo.Core.Commons.Exceptions;
using Avvo.Core.Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Avvo.Core.Data.EntityFramework.Repositories;

/// <summary>
/// Classe base para obtenção de entidades por ID.
/// </summary>
/// <typeparam name="TEntity">O tipo da entidade.</typeparam>
public abstract class GetByIdRepositoryBase<TEntity> : IGetByIdRepository<TEntity> where TEntity : class
{
    protected ILogger Logger { get; }
    protected ActivitySource ActivitySource { get; }

    /// <summary>
    /// Inicializa uma nova instância de <see cref="GetByIdRepositoryBase{TEntity}"/>.
    /// </summary>
    /// <param name="logger">O logger para registro de eventos.</param>
    /// <param name="activitySource">A fonte de atividades para rastreamento.</param>
    /// <exception cref="ArgumentNullException">Lançada se logger ou activitySource forem nulos.</exception>
    protected GetByIdRepositoryBase(ILogger logger, ActivitySource activitySource)
    {
        Logger = logger ?? throw new ArgumentNullException(nameof(logger));
        ActivitySource = activitySource ?? throw new ArgumentNullException(nameof(activitySource));
    }

    /// <summary>
    /// Obtém uma entidade por seu ID.
    /// </summary>
    /// <param name="dbContext">O contexto do banco de dados.</param>
    /// <param name="id">O ID da entidade.</param>
    /// <returns>Um resultado com a entidade encontrada ou um erro se não for encontrada.</returns>
    public virtual async Task<TEntity> ExecuteAsync(DbContext dbContext, object id)
    {
        if (dbContext == null)
            throw new ArgumentNullException(nameof(dbContext), "O contexto do banco de dados não pode ser nulo.");
        if (id == null)
            throw new ArgumentNullException(nameof(id), "O ID não pode ser nulo.");

        using var activity = ActivitySource.StartActivity($"{GetType().Name}_{nameof(ExecuteAsync)}", ActivityKind.Internal);
        activity?.AddTag("entity_type", typeof(TEntity).Name);
        activity?.AddTag("entity_id", id.ToString());

        try
        {
            var entity = await dbContext.Set<TEntity>().FindAsync(id);
            if (entity == null)
                throw new NotFoundException($"Entidade do tipo {typeof(TEntity).Name} com ID {id} não encontrada.");

            return entity;
        }
        catch (Exception ex)
        {
            var errorMessage = $"Erro ao obter entidade do tipo {typeof(TEntity).Name} com ID {id}: {ex.Message}";
            Logger.LogError(ex, errorMessage);
            activity?.SetStatus(ActivityStatusCode.Error, errorMessage);
            throw new DataBaseException(errorMessage, ex);
        }
    }
}
