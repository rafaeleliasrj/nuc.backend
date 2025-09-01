using Avvo.Core.Commons.Exceptions;
using Avvo.Core.Commons.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;

namespace Avvo.Core.Data.Interceptors
{
    /// <summary>
    /// Interceptor para operações de salvamento no Entity Framework Core, gerenciando soft delete, auditoria e multi-tenancy.
    /// </summary>
    public class RepositoryDbContextSaveChangesInterceptor(ILogger<RepositoryDbContextSaveChangesInterceptor> logger, IUserIdentity userIdentity) : ISaveChangesInterceptor
    {
        private readonly ILogger<RepositoryDbContextSaveChangesInterceptor> _logger = logger;
        private readonly IUserIdentity _userIdentity = userIdentity;

        #region SavingChanges

        /// <summary>
        /// Intercepta operações de salvamento síncronas, aplicando soft delete e preenchendo propriedades de auditoria e tenant.
        /// </summary>
        /// <param name="eventData">Dados do evento de contexto do EF Core.</param>
        /// <param name="result">Resultado da operação de salvamento.</param>
        /// <returns>O resultado da operação, sem alterações.</returns>
        public InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
        {
            ProcessChanges(eventData);
            return result;
        }

        /// <summary>
        /// Intercepta operações de salvamento assíncronas, aplicando soft delete e preenchendo propriedades de auditoria e tenant.
        /// </summary>
        /// <param name="eventData">Dados do evento de contexto do EF Core.</param>
        /// <param name="result">Resultado da operação de salvamento.</param>
        /// <param name="cancellationToken">Token para cancelamento da operação assíncrona.</param>
        /// <returns>Uma tarefa com o resultado da operação, sem alterações.</returns>
        public ValueTask<InterceptionResult<int>> SavingChangesAsync(
            DbContextEventData eventData,
            InterceptionResult<int> result,
            CancellationToken cancellationToken = default)
        {
            ProcessChanges(eventData);
            return ValueTask.FromResult(result);
        }

        #endregion

        #region SavedChanges

        /// <summary>
        /// Intercepta o evento de salvamento concluído síncrono, sem alterações adicionais.
        /// </summary>
        /// <param name="eventData">Dados do evento de salvamento concluído.</param>
        /// <param name="result">Número de registros afetados.</param>
        /// <returns>O número de registros afetados, sem alterações.</returns>
        public int SavedChanges(SaveChangesCompletedEventData eventData, int result) => result;

        /// <summary>
        /// Intercepta o evento de salvamento concluído assíncrono, sem alterações adicionais.
        /// </summary>
        /// <param name="eventData">Dados do evento de salvamento concluído.</param>
        /// <param name="result">Número de registros afetados.</param>
        /// <param name="cancellationToken">Token para cancelamento da operação assíncrona.</param>
        /// <returns>Uma tarefa com o número de registros afetados, sem alterações.</returns>
        public ValueTask<int> SavedChangesAsync(SaveChangesCompletedEventData eventData, int result, CancellationToken cancellationToken = default)
            => ValueTask.FromResult(result);

        #endregion

        #region SaveChangesFailed

        /// <summary>
        /// Intercepta falhas em operações de salvamento síncronas, enriquecendo a exceção com informações detalhadas.
        /// </summary>
        /// <param name="eventData">Dados do erro ocorrido durante o salvamento.</param>
        public void SaveChangesFailed(DbContextErrorEventData eventData) => EnrichExceptionData(eventData);

        /// <summary>
        /// Intercepta falhas em operações de salvamento assíncronas, enriquecendo a exceção com informações detalhadas.
        /// </summary>
        /// <param name="eventData">Dados do erro ocorrido durante o salvamento.</param>
        /// <param name="cancellationToken">Token para cancelamento da operação assíncrona.</param>
        /// <returns>Uma tarefa representando o processamento da falha.</returns>
        public Task SaveChangesFailedAsync(DbContextErrorEventData eventData, CancellationToken cancellationToken = default)
        {
            EnrichExceptionData(eventData);
            return Task.CompletedTask;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Processa as alterações no ChangeTracker, aplicando soft delete, auditoria e validação de tenant.
        /// </summary>
        /// <param name="eventData">Dados do evento de contexto do EF Core.</param>
        private void ProcessChanges(DbContextEventData eventData)
        {
            var entries = eventData.Context!.ChangeTracker.Entries().ToList();
            var now = DateTimeOffset.UtcNow;

            foreach (var entry in entries)
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        if (entry.Entity is IBaseEntity baseEntityAdd)
                        {
                            baseEntityAdd.Id = Guid.NewGuid();
                            baseEntityAdd.CreateDate = now.UtcDateTime;
                            baseEntityAdd.CreateUserId = _userIdentity?.UserId;
                        }
                        if (entry.Entity is ITenantEntity tenantEntityAdd)
                        {
                            var tenantId = _userIdentity?.SubscriptionId ?? Guid.Empty;
                            if (tenantEntityAdd.TenantId == Guid.Empty)
                            {
                                tenantEntityAdd.TenantId = tenantId;
                                _logger.LogWarning("TenantId auto preenchido. Entidade: {EntityType}, TenantId: {TenantId}", entry.Entity.GetType().Name, tenantId);
                            }
                        }
                        break;

                    case EntityState.Modified:
                        if (entry.Entity is IBaseEntity baseEntityMod)
                        {
                            baseEntityMod.UpdateDate = now.UtcDateTime;
                            baseEntityMod.UpdateUserId = userIdentity?.UserId;
                        }
                        if (entry.Entity is ITenantEntity tenantEntityMod)
                        {
                            var currentTenantId = _userIdentity?.SubscriptionId ?? Guid.Empty;
                            if (currentTenantId != Guid.Empty && tenantEntityMod.TenantId != Guid.Empty && tenantEntityMod.TenantId != currentTenantId)
                            {
                                var errorMessage = $"TenantId inválido. Entidade: {entry.Entity.GetType().Name}, TenantId esperado: {currentTenantId}, Atual: {tenantEntityMod.TenantId}";
                                _logger.LogError(errorMessage);
                                throw new DataBaseException(errorMessage, new Exception());
                            }
                            if (tenantEntityMod.TenantId == Guid.Empty)
                            {
                                tenantEntityMod.TenantId = currentTenantId;
                                _logger.LogWarning("TenantId auto preenchido. Entidade: {EntityType}, TenantId: {TenantId}", entry.Entity.GetType().Name, currentTenantId);
                            }
                        }
                        break;

                    case EntityState.Deleted when entry.Entity is ISoftDelete softDelete:
                        entry.State = EntityState.Modified;
                        softDelete.IsDeleted = true;
                        softDelete.DeletedAt = now;
                        softDelete.DeletedUserId = _userIdentity?.UserId;
                        _logger.LogInformation("Soft delete aplicado. Entidade: {EntityType}, Id: {EntityId}", entry.Entity.GetType().Name, (entry.Entity as IBaseEntity)?.Id);
                        break;
                }
            }
        }

        /// <summary>
        /// Enriquece a exceção com informações detalhadas sobre as entidades com erro durante o salvamento.
        /// </summary>
        /// <param name="eventData">Dados do erro ocorrido durante o salvamento.</param>
        private void EnrichExceptionData(DbContextErrorEventData eventData)
        {
            if (eventData.Exception is DbUpdateException dbUpdateException)
            {
                var entitiesWithError = dbUpdateException.Entries.Select(entry => new
                {
                    Type = entry.Entity.GetType().FullName,
                    Entity = entry.Entity
                }).ToList();

                dbUpdateException.Data["EntitiesWithError"] = entitiesWithError;
                _logger.LogError(dbUpdateException, "Falha ao salvar alterações. Entidades com erro: {EntityCount}", entitiesWithError.Count);
            }
        }

        #endregion
    }
}
