// src/DeveloperStore.Sales.Infrastructure/Context/Configurations/CustomerConfiguration.cs

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using DeveloperStore.Sales.Domain.Entities;

namespace DeveloperStore.Sales.Infrastructure.Context.Configurations
{
    public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.ToTable("Customers");

            builder.HasKey(c => c.Id);
            builder.Property(c => c.Id)
                   .HasColumnName("Id")
                   .IsRequired()
                   .ValueGeneratedNever(); // Guid gerado no ctor

            builder.Property(c => c.Name)
                   .HasColumnName("Name")
                   .HasMaxLength(200)
                   .IsRequired();

            builder.HasIndex(c => c.Name)
                   .IsUnique();

            // agora a relação 1:N compila corretamente:
            builder.HasMany(c => c.Sales)
                   .WithOne(s => s.Customer)
                   .HasForeignKey(s => s.CustomerId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
