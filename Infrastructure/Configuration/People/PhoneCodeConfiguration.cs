using System;
using Domain.Entities.People;
using Domain.ValueObjects.PhoneCodes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration.People;

public sealed class PhoneCodeConfiguration : IEntityTypeConfiguration<PhoneCode>
{
    public void Configure(EntityTypeBuilder<PhoneCode> builder)
    {
        builder.ToTable("phonecodes");
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Id).HasColumnName("id");
        builder.Property(p => p.CountryCode)
            .HasColumnName("country_code")
            .HasMaxLength(5)
            .IsRequired()
            .HasConversion(
                v => v.Value,
                v => CountryCode.Create(v));

        builder.Property(p => p.CountryName)
            .HasColumnName("country_name")
            .HasMaxLength(100)
            .IsRequired()
            .HasConversion(
                v => v.Value,
                v => CountryName.Create(v));

        builder.HasIndex(p => p.CountryCode).IsUnique();
    }
}
