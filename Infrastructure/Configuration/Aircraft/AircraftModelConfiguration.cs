using System;
using Domain.Entities.Aircraft;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration.Aircraft;

public sealed class AircraftModelConfiguration : IEntityTypeConfiguration<AircraftModel>
{
    public void Configure(EntityTypeBuilder<AircraftModel> builder)
    {
        builder.ToTable("aircraftmodels");
        builder.HasKey(am => am.Id);
        builder.Property(am => am.Id).HasColumnName("id");
        builder.Property(am => am.ManufacturerId).HasColumnName("manufacturer_id");
        builder.Property(am => am.ModelName).HasColumnName("model_name").HasMaxLength(100).IsRequired();
        builder.Property(am => am.MaxCapacity).HasColumnName("max_capacity");
        builder.Property(am => am.MaxTakeoffWeightKg).HasColumnName("max_takeoff_weight_kg").HasPrecision(10, 2);
        builder.Property(am => am.FuelConsumptionKgH).HasColumnName("fuel_consumption_kg_h").HasPrecision(8, 2);
        builder.Property(am => am.CruisingSpeedKmh).HasColumnName("cruising_speed_kmh");
        builder.Property(am => am.CruisingAltitudeFt).HasColumnName("cruising_altitude_ft");
        builder.HasOne(am => am.Manufacturer)
            .WithMany(m => m.AircraftModels)
            .HasForeignKey(am => am.ManufacturerId);
    }
}