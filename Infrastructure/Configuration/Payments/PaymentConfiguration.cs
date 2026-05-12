using System;
using Domain.Entities.Payments;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration.Payments;

public sealed class PaymentConfiguration : IEntityTypeConfiguration<Payment>
{
    public void Configure(EntityTypeBuilder<Payment> builder)
    {
        builder.ToTable("payments");
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Id).HasColumnName("id");
        builder.Property(p => p.ReservationId).HasColumnName("reservation_id");
        builder.Property(p => p.Amount).HasColumnName("amount").HasPrecision(18, 2);
        builder.Property(p => p.PaymentDate).HasColumnName("payment_date");
        builder.Property(p => p.PaymentStateId).HasColumnName("payment_state_id");
        builder.Property(p => p.PaymentMethodId).HasColumnName("payment_method_id");
        builder.Property(p => p.CreatedAt).HasColumnName("created_at");
        builder.Property(p => p.UpdatedAt).HasColumnName("updated_at");
        builder.HasOne(p => p.Reservation)
            .WithMany()
            .HasForeignKey(p => p.ReservationId);
        builder.HasOne(p => p.PaymentState)
            .WithMany(ps => ps.Payments)
            .HasForeignKey(p => p.PaymentStateId);
        builder.HasOne(p => p.PaymentMethod)
            .WithMany(pm => pm.Payments)
            .HasForeignKey(p => p.PaymentMethodId);
    }
}