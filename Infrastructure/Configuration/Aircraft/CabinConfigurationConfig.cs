using System;
using Domain.Entities.Aircraft;
using Domain.ValueObjects.Aircraft;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration.Aircraft;

public sealed class CabinConfigurationConfiguration : IEntityTypeConfiguration<CabinConfiguration>
{
    public void Configure(EntityTypeBuilder<CabinConfiguration> builder)
    {
        builder.ToTable("cabinconfigurations");
        builder.HasKey(cc => cc.Id);
        builder.Property(cc => cc.Id).HasColumnName("id");
        builder.Property(cc => cc.AircraftId).HasColumnName("aircraft_id").IsRequired();
        builder.Property(cc => cc.CabinTypeId).HasColumnName("cabin_type_id").IsRequired();
        builder.Property(cc => cc.RowStart)
            .HasColumnName("row_start")
            .IsRequired()
            .HasConversion(
                v => v.Value,
                v => RowNumber.Create(v));
        builder.Property(cc => cc.RowEnd)
            .HasColumnName("row_end")
            .IsRequired()
            .HasConversion(
                v => v.Value,
                v => RowNumber.Create(v));
        builder.Property(cc => cc.SeatsPerRow)
            .HasColumnName("seats_per_row")
            .IsRequired()
            .HasConversion(
                v => v.Value,
                v => SeatsPerRow.Create(v));
        builder.Property(cc => cc.SeatLetters)
            .HasColumnName("seat_letters")
            .HasMaxLength(10)
            .IsRequired()
            .HasConversion(
                v => v.Value,
                v => SeatLetters.Create(v));
        builder.HasIndex(cc => new { cc.AircraftId, cc.CabinTypeId }).IsUnique();
        builder.HasOne(cc => cc.Aircraft)
            .WithMany(a => a.CabinConfigurations)
            .HasForeignKey(cc => cc.AircraftId)
            .OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(cc => cc.CabinType)
            .WithMany(ct => ct.CabinConfigurations)
            .HasForeignKey(cc => cc.CabinTypeId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
