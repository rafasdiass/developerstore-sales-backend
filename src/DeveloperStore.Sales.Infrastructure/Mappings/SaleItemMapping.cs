using DeveloperStore.Sales.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DeveloperStore.Sales.Infrastructure.Mappings
{
    /// <summary>
    /// Configuração do mapeamento da entidade SaleItem no banco de dados.
    /// </summary>
    public class SaleItemMapping : IEntityTypeConfiguration<SaleItem>
    {
        public void Configure(EntityTypeBuilder<SaleItem> builder)
        {
            builder.ToTable("SaleItems");

            builder.HasKey(i => i.Id);

            builder.Property(i => i.ProductId)
                   .IsRequired();

            builder.Property(i => i.ProductName)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(i => i.Quantity)
                   .IsRequired();

            builder.Property(i => i.UnitPrice)
                   .HasColumnType("decimal(18,2)")
                   .IsRequired();

            builder.Property(i => i.Discount)
                   .HasColumnType("decimal(18,2)")
                   .IsRequired();

            builder.Property(i => i.IsCancelled)
                   .IsRequired();
        }
    }
}
