using System;
using Domain.Entities.Flights;
using Domain.ValueObjects.FlightSeats;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Infrastructure.Configuration.Flights;

public sealed class FlightSeatConfiguration : IEntityTypeConfiguration<FlightSeat>
{
    public void Configure(EntityTypeBuilder<FlightSeat> builder)
    {
        builder.ToTable("flightseats");
        builder.HasKey(fs => fs.Id);
        builder.Property(fs => fs.Id).HasColumnName("id");
        builder.Property(fs => fs.FlightId).HasColumnName("flight_id");

        var seatCodeConverter = new ValueConverter<SeatCode, string>(
            seatCode => seatCode.Value,
            value => SeatCode.Create(value));

        builder.Property(fs => fs.SeatCode)
            .HasColumnName("seat_code")
            .HasConversion(seatCodeConverter)
            .HasMaxLength(5)
            .IsRequired();

        builder.Property(fs => fs.CabinTypeId).HasColumnName("cabin_type_id");
        builder.Property(fs => fs.SeatLocationTypeId).HasColumnName("seat_location_type_id");
        builder.Property(fs => fs.IsOccupied).HasColumnName("is_occupied").IsRequired();
        builder.HasIndex(fs => new { fs.FlightId, fs.SeatCode }).IsUnique();

        builder.HasOne(fs => fs.Flight)
            .WithMany()
            .HasForeignKey(fs => fs.FlightId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(fs => fs.CabinType)
            .WithMany()
            .HasForeignKey(fs => fs.CabinTypeId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(fs => fs.SeatLocationType)
            .WithMany()
            .HasForeignKey(fs => fs.SeatLocationTypeId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
