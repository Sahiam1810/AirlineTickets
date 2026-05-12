using System;
using Domain.Entities.Aircraft;
using Domain.ValueObjects.Aircraft;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration.Aircraft;

public sealed class CabinTypeConfiguration : IEntityTypeConfiguration<CabinType>
{
    public void Configure(EntityTypeBuilder<CabinType> builder)
    {
        builder.ToTable("cabintypes");
        builder.HasKey(ct => ct.Id);
        builder.Property(ct => ct.Id).HasColumnName("id");
        builder.Property(ct => ct.Name)
            .HasColumnName("name")
            .HasMaxLength(50)
            .IsRequired()
            .HasConversion(
                v => v.Value,
                v => CabinTypeName.Create(v));
        builder.HasIndex(ct => ct.Name).IsUnique();
    }
}
