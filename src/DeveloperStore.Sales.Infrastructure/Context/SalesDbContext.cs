using Microsoft.EntityFrameworkCore;
using DeveloperStore.Sales.Domain.Entities;

namespace DeveloperStore.Sales.Infrastructure.Context
{
    /// <summary>
    /// DbContext principal responsável por mapear as entidades do domínio de Vendas.
    /// </summary>
    public class SalesDbContext : DbContext
    {
        public DbSet<Sale> Sales { get; set; }
        public DbSet<SaleItem> SaleItems { get; set; }

        public SalesDbContext(DbContextOptions<SalesDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Aplica os mapeamentos de forma modular
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(SalesDbContext).Assembly);

            base.OnModelCreating(modelBuilder);
        }
    }
}
