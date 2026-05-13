using System;
using Domain.Entities.Aircraft;
using Domain.ValueObjects.Aircraft;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Infrastructure.Configuration.Aircraft;

public sealed class AircraftUnitConfiguration : IEntityTypeConfiguration<AircraftUnit>
{
    public void Configure(EntityTypeBuilder<AircraftUnit> builder)
    {
        var manufactureDateConverter = new ValueConverter<ManufactureDate?, DateOnly?>(
            v => v == null ? null : v.Value,
            v => ManufactureDate.Create(v));

        builder.ToTable("aircraft");
        builder.HasKey(a => a.Id);
        builder.Property(a => a.Id).HasColumnName("id");
        builder.Property(a => a.AircraftModelId).HasColumnName("model_id").IsRequired();
        builder.Property(a => a.AirlineId).HasColumnName("airline_id").IsRequired();
        builder.Property(a => a.Registration)
            .HasColumnName("registration")
            .HasMaxLength(20)
            .IsRequired()
            .HasConversion(
                v => v.Value,
                v => Registration.Create(v));
        builder.Property(a => a.ManufactureDate)
            .HasColumnName("manufacture_date")
            .HasConversion(manufactureDateConverter)
            .IsRequired(false);
        builder.Property(a => a.IsActive).HasColumnName("is_active").IsRequired();
        builder.HasIndex(a => a.Registration).IsUnique();
        builder.HasOne(a => a.AircraftModel)
            .WithMany( am => am.AircraftUnits)
            .HasForeignKey(a => a.AircraftModelId)
            .OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(a => a.Airline)
            .WithMany(al => al.AircraftUnits)
            .HasForeignKey(a => a.AirlineId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
