using System;
using Domain.Entities.Aircraft;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration.Aircraft;

public sealed class AircraftUnitConfiguration : IEntityTypeConfiguration<AircraftUnit>
{
    public void Configure(EntityTypeBuilder<AircraftUnit> builder)
    {
        builder.ToTable("aircraft");
        builder.HasKey(a => a.Id);
        builder.Property(a => a.Id).HasColumnName("id");
        builder.Property(a => a.ModelId).HasColumnName("model_id");
        builder.Property(a => a.AirlineId).HasColumnName("airline_id");
        builder.Property(a => a.Registration).HasColumnName("registration").HasMaxLength(20).IsRequired();
        builder.Property(a => a.ManufactureDate).HasColumnName("manufacture_date");
        builder.Property(a => a.IsActive).HasColumnName("is_active");
        builder.HasIndex(a => a.Registration).IsUnique();
        builder.HasOne(a => a.Model)
            .WithMany(m => m.AircraftUnits)
            .HasForeignKey(a => a.ModelId);
        builder.HasOne(a => a.Airline)
            .WithMany()
            .HasForeignKey(a => a.AirlineId);
    }
}