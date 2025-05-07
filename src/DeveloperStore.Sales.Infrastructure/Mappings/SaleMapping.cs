using DeveloperStore.Sales.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DeveloperStore.Sales.Infrastructure.Mappings
{
    /// <summary>
    /// Configuração do mapeamento da entidade Sale no banco de dados.
    /// </summary>
    public class SaleMapping : IEntityTypeConfiguration<Sale>
    {
        public void Configure(EntityTypeBuilder<Sale> builder)
        {
            builder.ToTable("Sales");

            builder.HasKey(s => s.Id);

            builder.Property(s => s.SaleNumber)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(s => s.SaleDate)
                   .IsRequired();

            builder.Property(s => s.CustomerId)
                   .IsRequired();

            builder.Property(s => s.CustomerName)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(s => s.BranchId)
                   .IsRequired();

            builder.Property(s => s.BranchName)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(s => s.IsCancelled)
                   .IsRequired();

            builder.HasMany(s => s.Items)
                   .WithOne()
                   .HasForeignKey("SaleId")
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
