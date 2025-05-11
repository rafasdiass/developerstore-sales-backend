using Microsoft.EntityFrameworkCore;
using DeveloperStore.Sales.Domain.Entities;

namespace DeveloperStore.Sales.Infrastructure.Context
{
    /// <summary>
    /// DbContext principal responsável por mapear as entidades do domínio de Vendas.
    /// </summary>
    public class SalesDbContext : DbContext
    {
        public SalesDbContext(DbContextOptions<SalesDbContext> options)
            : base(options)
        {
        }

        public DbSet<Sale> Sales { get; set; } = null!;
        public DbSet<SaleItem> SaleItems { get; set; } = null!;
        public DbSet<Branch> Branches { get; set; } = null!;
        public DbSet<Customer> Customers { get; set; } = null!;
        public DbSet<Product> Products { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Aplica todas as configurações da camada Infrastructure.Context.Configurations
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(SalesDbContext).Assembly);
        }
    }
}
