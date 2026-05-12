using System;
using Domain.Entities.Flights;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration.Flights;

public sealed class FlightSeatConfiguration : IEntityTypeConfiguration<FlightSeat>
{
    public void Configure(EntityTypeBuilder<FlightSeat> builder)
    {
        builder.ToTable("flightseats");
        builder.HasKey(fs => fs.Id);
        builder.Property(fs => fs.Id).HasColumnName("id");
        builder.Property(fs => fs.FlightId).HasColumnName("flight_id");
        builder.Property(fs => fs.SeatCode).HasColumnName("seat_code").HasMaxLength(5).IsRequired();
        builder.Property(fs => fs.CabinTypeId).HasColumnName("cabin_type_id");
        builder.Property(fs => fs.SeatLocationTypeId).HasColumnName("seat_location_type_id");
        builder.Property(fs => fs.IsOccupied).HasColumnName("is_occupied");
        builder.HasIndex(fs => new { fs.FlightId, fs.SeatCode }).IsUnique();
        builder.HasOne(fs => fs.Flight)
            .WithMany(f => f.FlightSeats)
            .HasForeignKey(fs => fs.FlightId);
        builder.HasOne(fs => fs.CabinType)
            .WithMany()
            .HasForeignKey(fs => fs.CabinTypeId);
        builder.HasOne(fs => fs.SeatLocationType)
            .WithMany(slt => slt.FlightSeats)
            .HasForeignKey(fs => fs.SeatLocationTypeId);
    }
}