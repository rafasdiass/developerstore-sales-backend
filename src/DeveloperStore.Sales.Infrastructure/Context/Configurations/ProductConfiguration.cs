// src/DeveloperStore.Sales.Infrastructure/Context/Configurations/ProductConfiguration.cs

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using DeveloperStore.Sales.Domain.Entities;

namespace DeveloperStore.Sales.Infrastructure.Context.Configurations
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable("Products");

            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id)
                   .IsRequired()
                   .ValueGeneratedNever(); // Gerado no ctor

            builder.Property(p => p.Name)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.HasIndex(p => p.Name)
                   .IsUnique();

            builder.Property(p => p.Price)
                   .HasColumnType("decimal(18,2)")
                   .IsRequired();
        }
    }
}
