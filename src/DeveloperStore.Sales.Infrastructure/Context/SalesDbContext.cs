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
        /// Conjunto de produtos.
        /// </summary>
        public DbSet<Product> Products { get; set; } = null!;

        /// <summary>
        /// Aplica configurações de mapeamento via Fluent API.
        /// </summary>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuração explícita da entidade Sale
            modelBuilder.Entity<Sale>(builder =>
            {
                builder.HasKey(s => s.Id);
                builder.Property(s => s.SaleNumber).IsRequired().HasMaxLength(50);
                builder.Property(s => s.SaleDate).IsRequired();
                builder.Property(s => s.CustomerId).IsRequired();
                builder.Property(s => s.CustomerName).IsRequired().HasMaxLength(100);
                builder.Property(s => s.BranchId).IsRequired();
                builder.Property(s => s.BranchName).IsRequired().HasMaxLength(100);
                builder.Property(s => s.TotalAmount).HasColumnType("decimal(18,2)");

                builder.HasMany(s => s.Items)
                       .WithOne(i => i.Sale)
                       .HasForeignKey(i => i.SaleId)
                       .OnDelete(DeleteBehavior.Cascade);
            });

            // Configuração explícita da entidade SaleItem
            modelBuilder.Entity<SaleItem>(builder =>
            {
                builder.HasKey(i => i.Id);
                builder.Property(i => i.ProductId).IsRequired();
                builder.Property(i => i.ProductName).IsRequired().HasMaxLength(100);
                builder.Property(i => i.Quantity).IsRequired();
                builder.Property(i => i.UnitPrice).HasColumnType("decimal(18,2)");
                builder.Property(i => i.Discount).HasColumnType("decimal(18,2)");
                builder.Property(i => i.TotalItemAmount).HasColumnType("decimal(18,2)");
            });

            // Configuração explícita da entidade Branch
            modelBuilder.Entity<Branch>(builder =>
            {
                builder.HasKey(b => b.Id);
                builder.Property(b => b.Name).IsRequired().HasMaxLength(100);
            });

            // Configuração explícita da entidade Customer
            modelBuilder.Entity<Customer>(builder =>
            {
                builder.HasKey(c => c.Id);
                builder.Property(c => c.Name).IsRequired().HasMaxLength(100);
                builder.Property(c => c.Email).IsRequired().HasMaxLength(150);
            });

            // Configuração explícita da entidade Product
            modelBuilder.Entity<Product>(builder =>
            {
                builder.HasKey(p => p.Id);
                builder.Property(p => p.Name).IsRequired().HasMaxLength(100);
                builder.Property(p => p.Price).HasColumnType("decimal(18,2)");
            });

            // Aplica automaticamente outras configurações presentes no assembly
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(SalesDbContext).Assembly);
        }
    }
}
