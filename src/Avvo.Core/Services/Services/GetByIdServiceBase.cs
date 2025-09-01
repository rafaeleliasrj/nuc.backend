using Avvo.Core.Commons.Consts;
using Avvo.Core.Commons.Exceptions;
using Avvo.Core.Data.Interfaces;
using Avvo.Core.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Avvo.Core.Services.Services
{
    public abstract class GetByIdServiceBase<TEntity, TContext> : IGetByIdService<TEntity> where TEntity : class where TContext : DbContext
    {
        protected ILogger Logger { get; }
        protected IGetByIdRepository<TEntity> GetByIdRepository { get; }
        protected ActivitySource _activitySource { get; }
        protected TContext _dbContext { get; }

        protected GetByIdServiceBase(ILogger logger, IGetByIdRepository<TEntity> getByIdRepository, ActivitySource activitySource, TContext dbContext)
        {
            Logger = logger;
            GetByIdRepository = getByIdRepository;
            _activitySource = activitySource;
            _dbContext = dbContext;
        }

        public virtual async Task<TEntity> ExecuteAsync(object id)
        {
            using var activity = _activitySource.StartActivity($"{this.GetType().Name}_{nameof(ExecuteAsync)}", ActivityKind.Internal)!;

            try
            {
                var result = await GetByIdRepository.ExecuteAsync(_dbContext, id);

                if (result is null) throw new NotFoundException($"{this.GetType().Name}_ExecuteAsync Entity not fount by id: {id}", ExceptionLayers.Service);

                return result;
            }
            catch (NotFoundException ex)
            {
                Logger.LogError(ex, ex.Message);
                activity.SetStatus(ActivityStatusCode.Error, ex.Message);
                throw;
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
                var errorMessage = $"{this.GetType().Name}_ExecuteAsync Couldn't get entity by id: {ex.Message}";
                Logger.LogError(ex, errorMessage);
                activity.SetStatus(ActivityStatusCode.Error, errorMessage);
                throw new ServiceException(errorMessage, ex);
            }
        }
    }
}
