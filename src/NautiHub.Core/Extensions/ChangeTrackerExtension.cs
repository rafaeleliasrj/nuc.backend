using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace NautiHub.Core.Extensions;

public static class ChangeTrackerExtension
{
    /// <summary>
    /// Habilita exclusão de registros com soft delete, quando chamado dentro do método de Commit do DBContext
    /// </summary>
    public static void EnableSoftDelete(this ChangeTracker changeTracker)
    {
        foreach (
            EntityEntry? entry in changeTracker
                .Entries()
                .Where(entry => entry.Entity.GetType().GetProperty("UpdatedAt") != null)
        )
        {
            if (entry.State == EntityState.Added)
            {
                entry.Property("CreatedAt").CurrentValue = DateTime.UtcNow;
                entry.Property("UpdatedAt").CurrentValue = DateTime.UtcNow;
            }

            if (entry.State == EntityState.Modified)
            {
                entry.Property("CreatedAt").IsModified = false;
                entry.Property("UpdatedAt").CurrentValue = DateTime.UtcNow;
            }
            if (entry.State == EntityState.Deleted)
            {
                entry.State = EntityState.Unchanged;
                entry.Property("IsDeleted").CurrentValue = true;
            }
        }
    }
}
