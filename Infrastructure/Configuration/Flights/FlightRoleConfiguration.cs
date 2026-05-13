using System;
using Domain.Entities.Flights;
using Domain.ValueObjects.FlightRoles;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration.Flights;

public sealed class FlightRoleConfiguration : IEntityTypeConfiguration<FlightRole>
{
    public void Configure(EntityTypeBuilder<FlightRole> builder)
    {
        builder.ToTable("flightroles");

        builder.HasKey(fr => fr.Id);

        builder.Property(fr => fr.Id)
               .HasColumnName("id");

        builder.Property(fr => fr.Name)
               .HasColumnName("name")
               .HasMaxLength(100)
               .IsRequired()
                .HasConversion(
                    v => v.Value,
                    v => FlightRoleName.Create(v)
                );

        builder.HasIndex(fr => fr.Name).IsUnique();
    }
}