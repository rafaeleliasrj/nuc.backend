using FluentValidation;
using Avvo.Core.Commons.Exceptions;
using Avvo.Core.Services.Interfaces;
using Avvo.Core.Data.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Transactions;
using Avvo.Core.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace Avvo.Core.Services.Services
{
    public abstract class AddServiceBase<TEntity, TContext> : IAddService<TEntity> where TEntity : class where TContext : DbContext
    {
        protected ILogger Logger { get; }
        protected IValidateService<TEntity> ValidateService { get; }
        protected IAddRepository<TEntity> AddBusinessesRepository { get; }
        protected ActivitySource _activitySource { get; }
        protected TContext _dbContext { get; }

        protected AddServiceBase(ILogger logger, IValidateService<TEntity> validateService, IAddRepository<TEntity> addBusinessesRepository, ActivitySource activitySource, TContext dbContext)
        {
            Logger = logger;
            ValidateService = validateService;
            AddBusinessesRepository = addBusinessesRepository;
            _activitySource = activitySource;
            _dbContext = dbContext;
        }

        public virtual async Task<TEntity> ExecuteTransactionAsync(TEntity entity)
        {
            return await ResilientTransaction.New(_dbContext).ExecuteAsync(async () =>
            {
                var result = await ExecuteAsync(entity);
                return result;
            });
        }
        public virtual async Task<TEntity> ExecuteAsync(TEntity entity)
        {
            using var activity = _activitySource.StartActivity($"{this.GetType().Name}_{nameof(ExecuteAsync)}", ActivityKind.Internal)!;

            try
            {
                await ValidateService.ExecuteAsync(entity);
                return await AddBusinessesRepository.ExecuteAsync(_dbContext, entity);
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
            catch (ValidationException ex)
            {
                Logger.LogError(ex, ex.Message);
                activity.SetStatus(ActivityStatusCode.Error, ex.Message);
                throw;
            }
            catch (ArgumentNullException ex)
            {
                Logger.LogError(ex, ex.Message);
                activity.SetStatus(ActivityStatusCode.Error, ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                var errorMessage = $"{this.GetType().Name}_ExecuteAsync Couldn't add entity: {ex.Message}";
                Logger.LogError(ex, errorMessage);
                activity.SetStatus(ActivityStatusCode.Error, errorMessage);
                throw new ServiceException(errorMessage, ex);
            }
        }
    }
}
