using Avvo.Core.Commons.Exceptions;
using Avvo.Core.Data.Context;
using Avvo.Core.Data.Interfaces;
using Avvo.Core.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Transactions;

namespace Avvo.Core.Services.Services
{
    public abstract class DeleteServiceBase<TEntity, TContext> : IDeleteService<TEntity>
        where TEntity : class
        where TContext : DbContext
    {
        protected ILogger Logger { get; }
        protected IDeleteRepository<TEntity> DeleteRepository { get; }
        protected ActivitySource _activitySource { get; }
        protected TContext _dbContext { get; }

        protected DeleteServiceBase(ILogger logger, IDeleteRepository<TEntity> deleteRepository, ActivitySource activitySource, TContext dbContext)
        {
            Logger = logger;
            DeleteRepository = deleteRepository;
            _activitySource = activitySource;
            _dbContext = dbContext;

        }

        public virtual async Task<int> ExecuteAsync(object id, bool propagateEvent = false, List<int> propagateDestination = null, string overrideEntityName = null)
        {
            using var activity = _activitySource.StartActivity($"{this.GetType().Name}_{nameof(ExecuteAsync)}", ActivityKind.Internal)!;

            try
            {
                return await DeleteRepository.ExecuteAsync(_dbContext, id, propagateEvent, propagateDestination, overrideEntityName);
            }
            catch (DataBaseException ex)
            {
                Logger.LogError(ex, ex.Message);
                activity.SetStatus(ActivityStatusCode.Error, ex.Message);
                throw;
            }
            catch (ServiceException ex)
            {
                activity?.SetStatus(ActivityStatusCode.Error, ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                var errorMessage = $"{this.GetType().Name}_ExecuteAsync Couldn't delete entity: {ex.Message}";
                Logger.LogError(ex, errorMessage);
                activity.SetStatus(ActivityStatusCode.Error, errorMessage);
                throw new ServiceException(errorMessage, ex);
            }
        }

        public async Task<int> ExecuteTransactionAsync(object id, bool propagateEvent = false, List<int> propagateDestination = null, string overrideEntityName = null)
        {

            return await ResilientTransaction.New(_dbContext).ExecuteAsync(async () =>
            {
                var result = await ExecuteAsync(id, propagateEvent, propagateDestination, overrideEntityName);
                return result;
            });
        }
    }
}
