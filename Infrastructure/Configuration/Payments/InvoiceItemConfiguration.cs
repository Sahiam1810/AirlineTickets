using Domain.Entities.Payments;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration.Payments;

public sealed class InvoiceItemConfiguration : IEntityTypeConfiguration<InvoiceItem>
{
    public void Configure(EntityTypeBuilder<InvoiceItem> builder)
    {
        builder.ToTable("invoice_items");
        
        builder.HasKey(i => i.Id);
        builder.Property(i => i.Id).HasColumnName("id");
        
        builder.Property(i => i.InvoiceId)
            .HasColumnName("invoice_id")
            .IsRequired();

        builder.Property(i => i.InvoiceItemTypeId)
            .HasColumnName("invoice_item_type_id")
            .IsRequired();

        builder.Property(i => i.Description)
            .HasColumnName("description")
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(i => i.Quantity)
            .HasColumnName("quantity")
            .IsRequired();

        builder.Property(i => i.UnitPrice)
            .HasColumnName("unit_price")
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(i => i.Subtotal)
            .HasColumnName("subtotal")
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(i => i.ReservationPassengerId)
            .HasColumnName("reservation_passenger_id");

        builder.Property(i => i.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired()
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.Property(i => i.UpdatedAt)
            .HasColumnName("updated_at")
            .IsRequired()
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.HasOne(i => i.Invoice)
            .WithMany(i => i.InvoiceItems)
            .HasForeignKey(i => i.InvoiceId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(i => i.InvoiceItemType)
            .WithMany()
            .HasForeignKey(i => i.InvoiceItemTypeId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(i => i.ReservationPassenger)
            .WithMany()
            .HasForeignKey(i => i.ReservationPassengerId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
