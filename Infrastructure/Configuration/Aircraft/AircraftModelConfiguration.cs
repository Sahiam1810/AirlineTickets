using System;
using Domain.Entities.Aircraft;
using Domain.ValueObjects.Aircraft;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Infrastructure.Configuration.Aircraft;

public sealed class AircraftModelConfiguration : IEntityTypeConfiguration<AircraftModel>
{
    public void Configure(EntityTypeBuilder<AircraftModel> builder)
    {
        var weightKgConverter = new ValueConverter<WeightKg?, decimal?>(
            v => v == null ? null : v.Value,
            v => WeightKg.Create(v));

        var fuelConsumptionConverter = new ValueConverter<FuelConsumption?, decimal?>(
            v => v == null ? null : v.Value,
            v => FuelConsumption.Create(v));

        var speedKmhConverter = new ValueConverter<SpeedKmh?, int?>(
            v => v == null ? null : v.Value,
            v => SpeedKmh.Create(v));

        var altitudeFtConverter = new ValueConverter<AltitudeFt?, int?>(
            v => v == null ? null : v.Value,
            v => AltitudeFt.Create(v));

        builder.ToTable("aircraftmodels");
        builder.HasKey(am => am.Id);
        builder.Property(am => am.Id).HasColumnName("id");
        builder.Property(am => am.ManufacturerId).HasColumnName("manufacturer_id").IsRequired();
        builder.Property(am => am.ModelName)
            .HasColumnName("model_name")
            .HasMaxLength(100)
            .IsRequired()
            .HasConversion(
                v => v.Value,
                v => ModelName.Create(v));
        builder.Property(am => am.MaxCapacity)
            .HasColumnName("max_capacity")
            .IsRequired()
            .HasConversion(
                v => v.Value,
                v => MaxCapacity.Create(v));
        builder.Property(am => am.MaxTakeoffWeightKg)
            .HasColumnName("max_takeoff_weight_kg")
            .HasPrecision(10, 2)
            .HasConversion(weightKgConverter)
            .IsRequired(false);
        builder.Property(am => am.FuelConsumptionKgPerHour)
            .HasColumnName("fuel_consumption_kg_h")
            .HasPrecision(8, 2)
            .HasConversion(fuelConsumptionConverter)
            .IsRequired(false);
        builder.Property(am => am.CruiseSpeedKmh)
            .HasColumnName("cruising_speed_kmh")
            .HasConversion(speedKmhConverter)
            .IsRequired(false);
        builder.Property(am => am.CruiseAltitudeFt)
            .HasColumnName("cruising_altitude_ft")
            .HasConversion(altitudeFtConverter)
            .IsRequired(false);
        builder.HasOne(am => am.Manufacturer)
            .WithMany()
            .HasForeignKey(am => am.ManufacturerId)
            .OnDelete(DeleteBehavior.Restrict);
        builder.HasIndex(am => new { am.ManufacturerId, am.ModelName }).IsUnique();
    }
}
