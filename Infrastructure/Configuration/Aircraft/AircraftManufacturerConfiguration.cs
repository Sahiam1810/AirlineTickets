using System;
using Domain.Entities.Aircraft;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration.Aircraft;

public sealed class AircraftManufacturerConfiguration : IEntityTypeConfiguration<AircraftManufacturer>
{
    public void Configure(EntityTypeBuilder<AircraftManufacturer> builder)
    {
        builder.ToTable("aircraftmanufacturers");
        builder.HasKey(am => am.Id);
        builder.Property(am => am.Id).HasColumnName("id");
        builder.Property(am => am.Name).HasColumnName("name").HasMaxLength(100).IsRequired();
        builder.Property(am => am.Country).HasColumnName("country").HasMaxLength(100).IsRequired();
    }
}