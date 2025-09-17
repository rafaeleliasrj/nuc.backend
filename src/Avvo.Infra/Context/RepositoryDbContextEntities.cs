using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

using Avvo.Core.Commons.Utils;
using Avvo.Domain.Entities;

namespace Avvo.Infra.Context
{
    public partial class RepositoryDbContext : DbContext
    {
        public RepositoryDbContext(DbContextOptions<RepositoryDbContext> options)
            : base(options)
        {
        }

        public DbSet<Business> Business => Set<Business>();
        public DbSet<Customer> Customer => Set<Customer>();
        public DbSet<Payment> Payment => Set<Payment>();
        public DbSet<Product> Product => Set<Product>();
        public DbSet<ProductCategory> ProductCategory => Set<ProductCategory>();
        public DbSet<Sale> Sale => Set<Sale>();
        public DbSet<SaleItem> SaleItem => Set<SaleItem>();
        public DbSet<Stock> Stock => Set<Stock>();
        public DbSet<UnitOfMeasure> UnitOfMeasure => Set<UnitOfMeasure>();

        public void RunMigrate()
        {
            if (EnvironmentVariables.Get("ENVIRONMENT") == "test")
                this.Database.EnsureCreated();
            else
                this.Database.Migrate();
        }

        public void Drop()
        {
            this.Database.EnsureDeleted();
        }
    }
}
