using Domain.Entities.Payments;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration.Payments;

public sealed class CardIssuerConfiguration : IEntityTypeConfiguration<CardIssuer>
{
    public void Configure(EntityTypeBuilder<CardIssuer> builder)
    {
        builder.ToTable("cardissuers");
        builder.HasKey(ci => ci.Id);
        builder.Property(ci => ci.Id).HasColumnName("id");
        builder.Property(ci => ci.Name)
            .HasColumnName("name")
            .HasMaxLength(50)
            .IsRequired();

        builder.HasIndex(ci => ci.Name)
            .IsUnique();
    }
}
