// src/DeveloperStore.Sales.Infrastructure/Context/SalesDbContext.cs

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

        /// <summary>
        /// Conjunto de clientes.
        /// </summary>
        public DbSet<Customer> Customers { get; set; } = null!;

        /// <summary>
        /// Aplica configurações de mapeamento via Fluent API.
        /// </summary>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Este método vai buscar todas as classes que implementam IEntityTypeConfiguration<T>
            // no assembly atual e aplicar automaticamente seus mapeamentos:
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(SalesDbContext).Assembly);

            base.OnModelCreating(modelBuilder);
        }
    }
}
