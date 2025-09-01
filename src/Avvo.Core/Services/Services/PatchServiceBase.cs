using Avvo.Core.Commons.Exceptions;
using Avvo.Core.Commons.Extensions;
using Avvo.Core.Data.Context;
using Avvo.Core.Services.Interfaces;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Transactions;

namespace Avvo.Core.Services.Services
{
    public abstract class PatchServiceBase<TEntity, TContext> : IPatchService<TEntity> where TEntity : class where TContext : DbContext
    {
        protected ILogger Logger { get; }
        protected IUpdateService<TEntity> UpdateService { get; }
        protected IGetByIdService<TEntity> GetByIdService { get; }
        protected ActivitySource _activitySource { get; }
        protected TContext _dbContext { get; }

        protected PatchServiceBase(ILogger logger, IGetByIdService<TEntity> getByIdService, IUpdateService<TEntity> updateService, ActivitySource activitySource, TContext dbContext)
        {
            Logger = logger;
            GetByIdService = getByIdService;
            UpdateService = updateService;
            _activitySource = activitySource;
            _dbContext = dbContext;
        }

        public virtual async Task<int> ExecuteAsync(object id, JsonPatchDocument<TEntity> attributes)
        {
            using var activity = _activitySource.StartActivity($"{this.GetType().Name}_{nameof(ExecuteAsync)}", ActivityKind.Internal)!;
            var (_, numChanges) = await ExecuteWithReturnEntityAsync(id, attributes);
            return numChanges;
        }

        public virtual async Task<(TEntity entity, int numEntitiesChanged)> ExecuteWithReturnEntityAsync(object id, JsonPatchDocument<TEntity> attributes)
        {
            using var activity = _activitySource.StartActivity($"{this.GetType().Name}_{nameof(ExecuteWithReturnEntityAsync)}", ActivityKind.Internal)!;

            try
            {
                var existing = await GetByIdService.ExecuteAsync(id);

                if (existing == null)
                    throw new NotFoundException("Entity is not exists", Commons.Consts.ExceptionLayers.Data);

                var entity = existing.Clone<TEntity>();

                attributes.ApplyTo(entity);

                return (existing, await UpdateService.ExecuteAsync(id, entity));
            }
            catch (DataBaseException ex)
            {
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
                var errorMessage = $"{this.GetType().Name}_ExecuteAsync Couldn't update part of entity: {ex.Message}";
                Logger.LogError(ex, errorMessage);
                activity.SetStatus(ActivityStatusCode.Error, errorMessage);
                throw new ServiceException(errorMessage, ex);
            }
        }

        public async Task<int> ExecuteTransactionAsync(object id, JsonPatchDocument<TEntity> attributes)
        {
            return await ResilientTransaction.New(_dbContext).ExecuteAsync(async () =>
            {
                var result = await ExecuteAsync(id, attributes);
                return result;
            });
        }

        public async Task<(TEntity entity, int numEntitiesChanged)> ExecuteTransactionWithReturnEntityAsync(object id, JsonPatchDocument<TEntity> attributes)
        {
            return await ResilientTransaction.New(_dbContext).ExecuteAsync(async () =>
            {
                var result = await ExecuteWithReturnEntityAsync(id, attributes);
                return result;
            });
        }
    }
}
