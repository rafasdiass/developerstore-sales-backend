
using DeveloperStore.Sales.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DeveloperStore.Sales.Infrastructure.Mappings
{
    public class BranchMapping : IEntityTypeConfiguration<Branch>
    {
        public void Configure(EntityTypeBuilder<Branch> builder)
        {
            builder.ToTable("Branches");

            builder.HasKey(b => b.Id);

            // o Id é gerado no construtor do Branch (ValueGeneratedNever por padrão)
            builder.Property(b => b.Id)
                   .ValueGeneratedNever();

            builder.Property(b => b.Name)
                   .IsRequired()
                   .HasMaxLength(100);
        }
    }
}
