using System;
using Domain.Entities.Reservations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration.Reservations;

public sealed class ReservationFlightConfiguration : IEntityTypeConfiguration<ReservationFlight>
{
    public void Configure(EntityTypeBuilder<ReservationFlight> builder)
    {
        builder.ToTable("reservationflights");
        builder.HasKey(rf => rf.Id);
        builder.Property(rf => rf.Id).HasColumnName("id");
        builder.Property(rf => rf.ReservationId).HasColumnName("reservation_id");
        builder.Property(rf => rf.FlightId).HasColumnName("flight_id");
        builder.Property(rf => rf.PartialValue).HasColumnName("partial_value").HasPrecision(18, 2);
        builder.HasIndex(rf => new { rf.ReservationId, rf.FlightId }).IsUnique();
        builder.HasOne(rf => rf.Reservation)
            .WithMany(r => r.ReservationFlights)
            .HasForeignKey(rf => rf.ReservationId);
        builder.HasOne(rf => rf.Flight)
            .WithMany()
            .HasForeignKey(rf => rf.FlightId);
    }
}