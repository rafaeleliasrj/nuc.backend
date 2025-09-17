using Microsoft.EntityFrameworkCore;

namespace Avvo.Core.Data.Context
{
    public class ResilientTransaction
    {
        private DbContext _dbContext;
        private ResilientTransaction(DbContext context) =>
            _dbContext = context ?? throw new ArgumentNullException(nameof(context));

        public static ResilientTransaction New(DbContext context) =>
            new ResilientTransaction(context);

        public async Task<T> ExecuteAsync<T>(Func<Task<T>> action)
        {
            // Use of an EF Core resiliency strategy when using multiple DbContexts
            // within an explicit BeginTransaction():
            // https://learn.microsoft.com/ef/core/miscellaneous/connection-resiliency
            var strategy = _dbContext.Database.CreateExecutionStrategy();

            return await strategy.ExecuteAsync(async () =>
            {
                await using var transaction = await _dbContext.Database.BeginTransactionAsync();
                var result = await action();
                await transaction.CommitAsync();
                return result;
            });
        }
        public async Task ExecuteAsync(Func<Task> action)
        {
            // Use of an EF Core resiliency strategy when using multiple DbContexts
            // within an explicit BeginTransaction():
            // https://learn.microsoft.com/ef/core/miscellaneous/connection-resiliency
            var strategy = _dbContext.Database.CreateExecutionStrategy();

            await strategy.ExecuteAsync(async () =>
            {
                await using var transaction = await _dbContext.Database.BeginTransactionAsync();
                await action();
                await transaction.CommitAsync();
            });
        }
    }
}
