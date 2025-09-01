using FluentValidation;
using Avvo.Core.Commons.Exceptions;
using Avvo.Core.Data.Context;
using Avvo.Core.Data.Interfaces;
using Avvo.Core.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Transactions;

namespace Avvo.Core.Services.Services
{
    public abstract class UpdateServiceBase<TEntity, TContext> : IUpdateService<TEntity>
        where TEntity : class
        where TContext : DbContext
    {
        protected ILogger Logger { get; }
        protected IUpdateRepository<TEntity> UpdateRepository { get; }
        protected IValidateService<TEntity> ValidateService { get; }
        protected ActivitySource _activitySource { get; }
        protected TContext _dbContext { get; }

        protected UpdateServiceBase(ILogger logger, IValidateService<TEntity> validateService, IUpdateRepository<TEntity> updateRepository, ActivitySource activitySource, TContext dbContext)
        {
            Logger = logger;
            ValidateService = validateService;
            UpdateRepository = updateRepository;
            _activitySource = activitySource;
            _dbContext = dbContext;

        }

        public virtual async Task<int> ExecuteAsync(object id, TEntity entity)
        {
            using var activity = _activitySource.StartActivity($"{this.GetType().Name}_{nameof(ExecuteAsync)}", ActivityKind.Internal)!;

            try
            {
                await ValidateService.ExecuteAsync(entity);
                return await UpdateRepository.ExecuteAsync(_dbContext, id, entity);
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
            catch (NotFoundException ex)
            {
                activity.SetStatus(ActivityStatusCode.Error, ex.Message);
                throw;
            }
            catch (ValidationException ex)
            {
                Logger.LogError(ex, ex.Message);
                activity.SetStatus(ActivityStatusCode.Error, ex.Message);
                throw;
            }
            catch (ArgumentNullException ex)
            {
                activity.SetStatus(ActivityStatusCode.Error, ex.Message);
                Logger.LogError(ex, ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                var errorMessage = $"{this.GetType().Name}_ExecuteAsync Couldn't update entity: {ex.Message}";
                Logger.LogError(ex, errorMessage);
                activity.SetStatus(ActivityStatusCode.Error, errorMessage);
                throw new ServiceException(errorMessage, ex);
            }
        }

        public async Task<int> ExecuteTransactionAsync(object id, TEntity entity)
        {

            return await ResilientTransaction.New(_dbContext).ExecuteAsync(async () =>
            {
                var result = await ExecuteAsync(id, entity);
                return result;
            });
        }
    }
}
