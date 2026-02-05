using Microsoft.EntityFrameworkCore;

namespace NautiHub.Core.Extensions;

public static class DbUpdateConcurrencyExtension
{
    public static void UndoChangesToEntities(this DbUpdateConcurrencyException excecao)
    {
        DbContext? context = excecao.Entries.FirstOrDefault()?.Context;

        if (context == null)
        {
            return;
        }

        foreach (Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry entry in context.ChangeTracker.Entries())
        {
            switch (entry.State)
            {
                case EntityState.Modified:
                    entry.CurrentValues.SetValues(entry.OriginalValues);
                    entry.State = EntityState.Unchanged;
                    break;
                case EntityState.Added:
                    entry.State = EntityState.Detached;
                    break;
                case EntityState.Deleted:
                    entry.State = EntityState.Unchanged;
                    break;
            }
        }

        context.ChangeTracker.Clear();
    }
}
