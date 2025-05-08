using Microsoft.EntityFrameworkCore;
using DeveloperStore.Sales.Domain.Entities;

namespace DeveloperStore.Sales.Infrastructure.Context
{
    /// <summary>
    /// DbContext principal responsável por mapear as entidades do domínio de Vendas.
    /// </summary>
    public class SalesDbContext : DbContext
    {
        /// <summary>
        /// Construtor que recebe as opções configuradas em startup.
        /// </summary>
        public SalesDbContext(DbContextOptions<SalesDbContext> options)
            : base(options)
        {
        }

        /// <summary>
        /// Conjunto de vendas.
        /// </summary>
        public DbSet<Sale> Sales { get; set; } = null!;

        /// <summary>
        /// Conjunto de itens de venda.
        /// </summary>
        public DbSet<SaleItem> SaleItems { get; set; } = null!;

        /// <summary>
        /// Conjunto de filiais.
        /// </summary>
        public DbSet<Branch> Branches { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Aplica todos os mapeamentos de IEntityTypeConfiguration<T> do assembly
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(SalesDbContext).Assembly);

            base.OnModelCreating(modelBuilder);
        }
    }
}
