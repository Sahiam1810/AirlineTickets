using System;
using Domain.Entities.Aircraft;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration.Aircraft;

public sealed class CabinConfigurationConfig : IEntityTypeConfiguration<CabinConfiguration>
{
    public void Configure(EntityTypeBuilder<CabinConfiguration> builder)
    {
        builder.ToTable("cabinconfiguration");
        builder.HasKey(cc => cc.Id);
        builder.Property(cc => cc.Id).HasColumnName("id");
        builder.Property(cc => cc.AircraftUnitId).HasColumnName("aircraft_unit_id");
        builder.Property(cc => cc.CabinTypeId).HasColumnName("cabin_type_id");
        builder.Property(cc => cc.StartRow).HasColumnName("start_row");
        builder.Property(cc => cc.EndRow).HasColumnName("end_row");
        builder.Property(cc => cc.SeatsPerRow).HasColumnName("seats_per_row");
        builder.Property(cc => cc.SeatLetters).HasColumnName("seat_letters").HasMaxLength(10).IsRequired();
        builder.HasIndex(cc => new { cc.AircraftUnitId, cc.CabinTypeId }).IsUnique();
        builder.HasOne(cc => cc.AircraftUnit)
            .WithMany(a => a.CabinConfigurations)
            .HasForeignKey(cc => cc.AircraftUnitId);
        builder.HasOne(cc => cc.CabinType)
            .WithMany(ct => ct.CabinConfigurations)
            .HasForeignKey(cc => cc.CabinTypeId);
    }
}