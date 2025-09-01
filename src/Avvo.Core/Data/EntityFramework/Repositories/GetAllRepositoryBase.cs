using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Avvo.Core.Commons.Exceptions;
using Avvo.Core.Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Avvo.Core.Data.EntityFramework.Repositories
{
    /// <summary>
    /// Classe base para repositórios que implementam operações de recuperação de múltiplas entidades.
    /// </summary>
    /// <typeparam name="TEntity">Tipo da entidade a ser manipulada.</typeparam>
    public abstract class GetAllRepositoryBase<TEntity>(ILogger logger, ActivitySource activitySource) : IGetAllRepository<TEntity>
        where TEntity : class
    {
        protected ILogger Logger { get; } = logger;
        protected ActivitySource ActivitySource { get; } = activitySource;

        /// <summary>
        /// Executa a consulta assíncrona para recuperar entidades com filtros, ordenação e paginação opcionais.
        /// </summary>
        /// <param name="dbContext">Contexto do banco de dados Entity Framework.</param>
        /// <param name="predicate">Expressão de filtro opcional para as entidades.</param>
        /// <param name="skip">Número de registros a pular para paginação (default: 0).</param>
        /// <param name="take">Número máximo de registros a retornar (default: 10; use 0 para sem limite).</param>
        /// <param name="orderBy">Dicionário de expressões de ordenação com direção ("ASC" ou "DESC").</param>
        /// <param name="cancellationToken">Token para cancelamento da operação assíncrona.</param>
        /// <returns>Uma tarefa que representa a lista de entidades recuperadas.</returns>
        public virtual async Task<IEnumerable<TEntity>> ExecuteAsync(
            DbContext dbContext,
            Expression<Func<TEntity, bool>>? predicate = null,
            int limit = 10,
            IDictionary<Expression<Func<TEntity, object>>, string>? orderBy = null,
            CancellationToken cancellationToken = default)
        {
            using var activity = ActivitySource.StartActivity($"{GetType().Name}_{nameof(ExecuteAsync)}", ActivityKind.Internal)!;

            try
            {
                var query = predicate != null ? dbContext.Set<TEntity>().Where(predicate) : dbContext.Set<TEntity>();
                query = PrepareQuery(query);

                if (orderBy != null)
                    foreach (var item in orderBy)
                        query = item.Value.ToUpper() == "DESC" ? query.OrderByDescending(item.Key) : query.OrderBy(item.Key);

                if (limit > 0) query = query.Take(limit);

                return await query.ToListAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                var errorMessage = $"{GetType().Name}_ExecuteAsync: Não foi possível recuperar as entidades: {ex.Message}";
                Logger.LogError(ex, errorMessage);
                activity.SetStatus(ActivityStatusCode.Error, errorMessage);
                throw new DataBaseException(errorMessage, ex);
            }
        }

        /// <summary>
        /// Método virtual para personalizar a query base, permitindo adicionar includes para eager loading (ex: query.Include(x => x.TabelaReferencia)).
        /// <para>
        /// A ordenação e paginação são aplicadas automaticamente após esta preparação.
        /// </para>
        /// </summary>
        /// <param name="query">Query inicial das entidades.</param>
        /// <returns>Query modificada (sem ordenação ou paginação).</returns>
        protected virtual IQueryable<TEntity> PrepareQuery(IQueryable<TEntity> query) => query;
    }
}
