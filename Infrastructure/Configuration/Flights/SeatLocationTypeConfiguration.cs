using System;
using Domain.Entities.Flights;
using Domain.ValueObjects.SeatLocationTypes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Infrastructure.Configuration.Flights;

public sealed class SeatLocationTypeConfiguration : IEntityTypeConfiguration<SeatLocationType>
{
    public void Configure(EntityTypeBuilder<SeatLocationType> builder)
    {
        builder.ToTable("seatlocationtypes");
        builder.HasKey(slt => slt.Id);
        builder.Property(slt => slt.Id).HasColumnName("id");

        var nameConverter = new ValueConverter<SeatLocationTypeName, string>(
            name => name.Value,
            value => SeatLocationTypeName.Create(value));

        builder.Property(slt => slt.Name)
            .HasColumnName("name")
            .HasConversion(nameConverter)
            .HasMaxLength(50)
            .IsRequired();

        builder.HasIndex(slt => slt.Name).IsUnique();
    }
}
