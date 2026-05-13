using Domain.Entities.Payments;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration.Payments;

public sealed class InvoiceConfiguration : IEntityTypeConfiguration<Invoice>
{
    public void Configure(EntityTypeBuilder<Invoice> builder)
    {
        builder.ToTable("invoices");

        builder.HasKey(i => i.Id);
        builder.Property(i => i.Id).HasColumnName("id");

        builder.Property(i => i.ReservationId)
            .HasColumnName("reservation_id")
            .IsRequired();

        builder.Property(i => i.InvoiceNumber)
            .HasColumnName("invoice_number")
            .HasMaxLength(30)
            .IsRequired();

        builder.Property(i => i.IssuedAt)
            .HasColumnName("issued_at")
            .IsRequired();

        builder.Property(i => i.Subtotal)
            .HasColumnName("subtotal")
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(i => i.Taxes)
            .HasColumnName("taxes")
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(i => i.Total)
            .HasColumnName("total")
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(i => i.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired()
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.Property(i => i.UpdatedAt)
            .HasColumnName("updated_at")
            .IsRequired()
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.ToTable(t => t.HasCheckConstraint("chk_invoice_subtotal", "subtotal >= 0"));
        builder.ToTable(t => t.HasCheckConstraint("chk_invoice_taxes", "taxes >= 0"));
        builder.ToTable(t => t.HasCheckConstraint("chk_invoice_total", "total >= 0"));
        builder.ToTable(t => t.HasCheckConstraint("chk_invoice_total_amount", "total = subtotal + taxes"));

        builder.HasIndex(i => i.ReservationId)
            .IsUnique();

        builder.HasIndex(i => i.InvoiceNumber)
            .IsUnique();

        builder.HasOne(i => i.Reservation)
            .WithOne()
            .HasForeignKey<Invoice>(i => i.ReservationId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
