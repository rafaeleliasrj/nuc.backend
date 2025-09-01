using System;
using System.Diagnostics;
using Avvo.Core.Commons.Exceptions;
using Avvo.Core.Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Avvo.Core.Data.EntityFramework.Repositories
{
    /// <summary>
    /// Classe base para repositórios que recuperam o DbSet de uma entidade.
    /// </summary>
    /// <typeparam name="TEntity">Tipo da entidade a ser manipulada.</typeparam>
    public abstract class GetDbSetRepositoryBase<TEntity>(ILogger logger, ActivitySource activitySource) : IGetDbSetRepository<TEntity>
        where TEntity : class
    {
        protected ILogger Logger { get; } = logger;
        protected ActivitySource ActivitySource { get; } = activitySource;

        /// <summary>
        /// Recupera o DbSet da entidade do contexto do banco de dados.
        /// </summary>
        /// <param name="dbContext">Contexto do banco de dados Entity Framework.</param>
        /// <returns>O DbSet da entidade.</returns>
        public virtual DbSet<TEntity> Execute(DbContext dbContext)
        {
            using var activity = ActivitySource.StartActivity($"{GetType().Name}_{nameof(Execute)}", ActivityKind.Internal)!;

            try
            {
                return dbContext.Set<TEntity>();
            }
            catch (Exception ex)
            {
                var errorMessage = $"{GetType().Name}_Execute: Não foi possível obter o DbSet: {ex.Message}";
                Logger.LogError(ex, errorMessage);
                activity.SetStatus(ActivityStatusCode.Error, errorMessage);
                throw new DataBaseException(errorMessage, ex);
            }
        }
    }
}
