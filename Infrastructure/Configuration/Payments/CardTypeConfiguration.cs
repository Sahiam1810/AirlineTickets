using Domain.Entities.Payments;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration.Payments;

public sealed class CardTypeConfiguration : IEntityTypeConfiguration<CardType>
{
    public void Configure(EntityTypeBuilder<CardType> builder)
    {
        builder.ToTable("cardtypes");
        builder.HasKey(ct => ct.Id);
        builder.Property(ct => ct.Id).HasColumnName("id");
        builder.Property(ct => ct.Name)
            .HasColumnName("name")
            .HasMaxLength(50)
            .IsRequired();

        builder.HasIndex(ct => ct.Name)
            .IsUnique();
    }
}
