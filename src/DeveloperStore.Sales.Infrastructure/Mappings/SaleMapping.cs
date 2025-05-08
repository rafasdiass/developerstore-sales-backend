// src/DeveloperStore.Sales.Infrastructure/Mappings/SaleMapping.cs
using DeveloperStore.Sales.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DeveloperStore.Sales.Infrastructure.Mappings
{
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

                     // Ignorar propriedade calculada
                     builder.Ignore(s => s.TotalAmount);

                     builder.HasMany(s => s.Items)
                            .WithOne(i => i.Sale)
                            .HasForeignKey(i => i.SaleId)
                            .OnDelete(DeleteBehavior.Cascade);
              }
       }
}
