using Domain.Entities.Payments;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration.Payments;

public sealed class PaymentMethodTypeConfiguration : IEntityTypeConfiguration<PaymentMethodType>
{
    public void Configure(EntityTypeBuilder<PaymentMethodType> builder)
    {
        builder.ToTable("paymentmethodtypes");
        builder.HasKey(pmt => pmt.Id);
        builder.Property(pmt => pmt.Id).HasColumnName("id");
        builder.Property(pmt => pmt.Name)
            .HasColumnName("name")
            .HasMaxLength(50)
            .IsRequired();

        builder.HasIndex(pmt => pmt.Name)
            .IsUnique();
    }
}
