using System;
using Domain.Entities.Payments;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration.Payments;

public sealed class PaymentMethodConfiguration : IEntityTypeConfiguration<PaymentMethod>
{
    public void Configure(EntityTypeBuilder<PaymentMethod> builder)
    {
        builder.ToTable("paymentmethods");
        builder.HasKey(pm => pm.Id);
        builder.Property(pm => pm.Id).HasColumnName("id");
        builder.Property(pm => pm.PaymentMethodTypeId).HasColumnName("payment_method_type_id");
        builder.Property(pm => pm.CardTypeId).HasColumnName("card_type_id");
        builder.Property(pm => pm.CardIssuerId).HasColumnName("card_issuer_id");
        builder.Property(pm => pm.CommercialName).HasColumnName("commercial_name").HasMaxLength(50).IsRequired();
        builder.Property(pm => pm.CreatedAt).HasColumnName("created_at").IsRequired().HasDefaultValueSql("CURRENT_TIMESTAMP");
        builder.Property(pm => pm.UpdatedAt).HasColumnName("updated_at").IsRequired().HasDefaultValueSql("CURRENT_TIMESTAMP");
        builder.HasIndex(pm => pm.CommercialName).IsUnique();
        builder.HasOne(pm => pm.PaymentMethodType)
            .WithMany(pmt => pmt.PaymentMethods)
            .HasForeignKey(pm => pm.PaymentMethodTypeId)
            .OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(pm => pm.CardType)
            .WithMany(ct => ct.PaymentMethods)
            .HasForeignKey(pm => pm.CardTypeId)
            .OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(pm => pm.CardIssuer)
            .WithMany(ci => ci.PaymentMethods)
            .HasForeignKey(pm => pm.CardIssuerId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
