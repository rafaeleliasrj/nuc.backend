using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NautiHub.Core.Data;
using NautiHub.Core.DomainObjects;
using NautiHub.Core.Extensions;
using NautiHub.Core.Resources;
using NautiHub.Domain.Entities;
using NautiHub.Infrastructure.Entities;
using NautiHub.Infrastructure.Identity;
using NautiHub.Infrastructure.Services.Utilitarios;

namespace NautiHub.Infrastructure.DataContext;

public class DatabaseContext(
    DbContextOptions<DatabaseContext> contextOptions,
    ILogger<DatabaseContext> logger,
    INautiHubIdentity nautiHubIdentity,
    IHostEnvironment env,
    MessagesService messagesService
) : DbContext(contextOptions), IUnitOfWork
{
    private readonly ILogger<DatabaseContext> _logger = logger;
    private readonly INautiHubIdentity _nautiHubIdentity = nautiHubIdentity;
    private readonly IHostEnvironment _env = env;
    private readonly MessagesService _messagesService = messagesService;

    public DbSet<IdempotentCommand> ProcessCommand { get; set; }
    public DbSet<Boat> Boats { get; set; }
    public DbSet<Booking> Bookings { get; set; }
    public DbSet<Payment> Payments { get; set; }
    public DbSet<ChatMessage> ChatMessages { get; set; }
    public DbSet<Review> Reviews { get; set; }
    public DbSet<ScheduledTour> ScheduledTours { get; set; }

    // ASP.NET Core Identity entities
    public DbSet<UserIdentity> UserIdentities { get; set; }
    public DbSet<IdentityRole<Guid>> Roles { get; set; }

    public virtual async Task CommitAsync(bool ativarSoftDelete = true)
    {
        if (!ChangeTracker.HasChanges())
            return;

        if (ativarSoftDelete)
        {
            ChangeTracker.EnableSoftDelete();
        }

        foreach (EntityEntry entry in ChangeTracker.Entries())
        {
            if (entry.Entity is IEntity entidade)
            {
                switch (entry.State)
                {
                    case EntityState.Added:

                        if (entry.Entity is IEntity userEntity && userEntity.UserId == Guid.Empty)
                            userEntity.UserId = _nautiHubIdentity?.UserId ?? Guid.Empty;
                        break;
                }
            }
        }

        try
        {
            await base.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "[Commit] - {Message}: {InnerMessage}",
                _messagesService.Database_Commit_Error,
                ex.InnerException
            );
            throw;
        }
    }

    private static string GetConnectionString()
    {
        var connectionString = Environment.GetEnvironmentVariable("POSTGRESQL_CONNECTION");
        if (string.IsNullOrEmpty(connectionString))
            throw new ArgumentNullException();
        return connectionString;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var connectionString = GetConnectionString();

        optionsBuilder.UseNpgsql(connectionString);

if (_env.IsDevelopment())
        {
            optionsBuilder
                .LogTo(Console.WriteLine, LogLevel.Information)
                .EnableSensitiveDataLogging()
                .EnableDetailedErrors();
        }
        else
        {
            optionsBuilder
                .LogTo(_ => { });
        }

        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        foreach (var entity in modelBuilder.Model.GetEntityTypes())
        {
            foreach (var property in entity.GetProperties())
            {
                // === STRING ===
                if (property.ClrType == typeof(string))
                {
                    if (property.GetMaxLength() == null)
                    {
                        property.SetMaxLength(255);
                    }
                }

                // === DECIMAL ===
                if (property.ClrType == typeof(decimal) || property.ClrType == typeof(decimal?))
                {
                    property.SetPrecision(18);
                    property.SetScale(4);
                }

                // === DATETIME ===
                if (property.ClrType == typeof(DateTime) || property.ClrType == typeof(DateTime?))
                {
                    property.SetColumnType("timestamp with time zone");
                }

                // === DATEONLY ===
                if (property.ClrType == typeof(DateOnly) || property.ClrType == typeof(DateOnly?))
                {
                    property.SetColumnType("date");
                }

                // === TIMEONLY ===
                if (property.ClrType == typeof(TimeOnly) || property.ClrType == typeof(TimeOnly?))
                {
                    property.SetColumnType("time");
                }

                // === ENUM ===
                if (property.ClrType.IsEnum)
                {
                    property.SetColumnType("varchar(50)");
                }
            }
        }

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(DatabaseContext).Assembly);

        modelBuilder.SetQueryFilterOnAllEntities<IEntityUserControlAccess>(
            e => !GetUserId().HasValue || e.UserId == GetUserId()
        );

        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>(b =>
        {
            b.ToTable("Users");
            b.HasKey(u => u.Id);
        });

        modelBuilder.Entity<IdentityUserClaim<Guid>>(b =>
        {
            b.ToTable("UserClaims");
            b.HasKey(uc => uc.Id);
        });

        modelBuilder.Entity<IdentityUserLogin<Guid>>(b =>
        {
            b.ToTable("UserLogins");
            b.HasKey(l => new { l.LoginProvider, l.ProviderKey });
        });

        modelBuilder.Entity<IdentityUserToken<Guid>>(b =>
        {
            b.ToTable("UserTokens");
            b.HasKey(t => new { t.UserId, t.LoginProvider, t.Name });
        });

        modelBuilder.Entity<IdentityRole<Guid>>(b =>
        {
            b.ToTable("Roles");
            b.HasKey(r => r.Id);
        });

        modelBuilder.Entity<IdentityRoleClaim<Guid>>(b =>
        {
            b.ToTable("RoleClaims");
            b.HasKey(rc => rc.Id);
        });

        modelBuilder.Entity<IdentityUserRole<Guid>>(b =>
        {
            b.ToTable("UserRoles");
            b.HasKey(ur => new { ur.UserId, ur.RoleId });
        });
    }


    /// <summary>
    /// Método responsável por recuperar o UserId que é usada como userFilter nas consultas
    /// </summary>
    /// <returns></returns>
    private Guid? GetUserId() => _nautiHubIdentity?.UserId;
}

