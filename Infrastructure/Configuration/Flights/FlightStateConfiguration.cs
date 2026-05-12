using System;
using Domain.Entities.Flights;
using Domain.ValueObjects.FlightStates;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration.Flights;

public sealed class FlightStateConfiguration : IEntityTypeConfiguration<FlightState>
{
    public void Configure(EntityTypeBuilder<FlightState> builder)
    {
        builder.ToTable("flightstates");
        builder.HasKey(fs => fs.Id);
        builder.Property(fs => fs.Id).HasColumnName("id");
        builder.Property(fs => fs.Name)
            .HasColumnName("name")
            .HasMaxLength(50)
            .IsRequired()
            .HasConversion(
                v => v.Value,
                v => FlightStateName.Create(v));
        builder.HasIndex(fs => fs.Name).IsUnique();
    }
}
