using System;
using Domain.Entities.People;
using Domain.ValueObjects.PassengerTypes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Infrastructure.Configuration.People;

public sealed class PassengerTypeConfiguration : IEntityTypeConfiguration<PassengerType>
{
    public void Configure(EntityTypeBuilder<PassengerType> builder)
    {
        var ageConverter = new ValueConverter<Age?, int?>(
            v => v == null ? null : v.Value,
            v => Age.Create(v));

        builder.ToTable("passengertypes");
        builder.HasKey(pt => pt.Id);
        builder.Property(pt => pt.Id).HasColumnName("id");
        builder.Property(pt => pt.Name)
            .HasColumnName("name")
            .HasMaxLength(50)
            .IsRequired()
            .HasConversion(
                v => v.Value,
                v => PassengerTypeName.Create(v));
        builder.Property(pt => pt.AgeMin)
            .HasColumnName("age_min")
            .HasConversion(ageConverter);
        builder.Property(pt => pt.AgeMax)
            .HasColumnName("age_max")
            .HasConversion(ageConverter);
        builder.HasIndex(pt => pt.Name).IsUnique();
    }
}
