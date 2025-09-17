using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata;

using Avvo.Domain.Entities;
using Avvo.Core.Commons.Extensions;
using Avvo.Core.Commons.Interfaces;

namespace Avvo.Infra.Context;

public partial class RepositoryDbContext : DbContext
{
    private readonly IUserIdentity _userIdentity;

    public RepositoryDbContext(DbContextOptions<RepositoryDbContext> options, IUserIdentity userIdentity)
        : base(options)
    {
        _userIdentity = userIdentity;
    }

    /// <summary>
    ///     OnModelCreating.
    /// </summary>
    /// <param name="modelBuilder">modelbuilder.</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        foreach (Microsoft.EntityFrameworkCore.Metadata.IMutableEntityType entity in modelBuilder.Model.GetEntityTypes())
        {
            foreach (Microsoft.EntityFrameworkCore.Metadata.IMutableProperty property in entity.GetProperties())
            {
                if (property.ClrType == typeof(string))
                {
                    var hasMaxLength = property.GetMaxLength() != null;
                    if (!hasMaxLength)
                    {
                        property.SetMaxLength(255);
                    }
                    property.SetCollation("SQL_Latin1_General_CP1_CI_AS");
                }

                if (property.ClrType == typeof(decimal) || property.ClrType == typeof(decimal?))
                {
                    property.SetPrecision(13);
                    property.SetScale(2);
                }
            }
        }

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(RepositoryDbContext).Assembly);
        modelBuilder.SetQueryFilterOnAllEntities<ITenantEntity>(e => !GetTenantId().HasValue || e.TenantId == GetTenantId());
        modelBuilder.SetQueryFilterOnAllEntities<IBusinessEntity>(e => !GetBusinessId().HasValue || e.BusinessId == GetBusinessId());

        base.OnModelCreating(modelBuilder);
    }

    protected Guid GetTenantId()
    {
        return _userIdentity is null ? Guid.Empty : _userIdentity.SubscriptionId;
    }

    protected Guid GetBusinessId()
    {
        return _userIdentity is null ? Guid.Empty : _userIdentity.BusinessId;
    }
}
