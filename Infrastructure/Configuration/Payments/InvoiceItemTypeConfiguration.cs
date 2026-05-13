using Domain.Entities.Payments;
using Domain.ValueObjects.Payments;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration.Payments;

public sealed class InvoiceItemTypeConfiguration : IEntityTypeConfiguration<InvoiceItemType>
{
    public void Configure(EntityTypeBuilder<InvoiceItemType> builder)
    {
        builder.ToTable("invoice_item_types");
        
        builder.HasKey(c => c.Id);
        builder.Property(c => c.Id).HasColumnName("id");
        
        builder.Property(c => c.Name)
            .HasColumnName("name")
            .HasMaxLength(100)
            .IsRequired()
            .HasConversion(
                v => v.Value,
                v => InvoiceItemTypeName.Create(v));
                
        builder.HasIndex(c => c.Name).IsUnique();

        builder.Property(c => c.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired()
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.Property(c => c.UpdatedAt)
            .HasColumnName("updated_at")
            .IsRequired()
            .HasDefaultValueSql("CURRENT_TIMESTAMP");
    }
}
