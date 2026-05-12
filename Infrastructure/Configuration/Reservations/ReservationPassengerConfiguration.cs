using System;
using Domain.Entities.Reservations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration.Reservations;

public sealed class ReservationPassengerConfiguration : IEntityTypeConfiguration<ReservationPassenger>
{
    public void Configure(EntityTypeBuilder<ReservationPassenger> builder)
    {
        builder.ToTable("reservationpassengers");
        builder.HasKey(rp => rp.Id);
        builder.Property(rp => rp.Id).HasColumnName("id");
        builder.Property(rp => rp.ReservationFlightId).HasColumnName("reservation_flight_id");
        builder.Property(rp => rp.PassengerId).HasColumnName("passenger_id");
        builder.HasIndex(rp => new { rp.ReservationFlightId, rp.PassengerId }).IsUnique();
        builder.HasOne(rp => rp.ReservationFlight)
            .WithMany(rf => rf.ReservationPassengers)
            .HasForeignKey(rp => rp.ReservationFlightId);
        builder.HasOne(rp => rp.Passenger)
            .WithMany()
            .HasForeignKey(rp => rp.PassengerId);
    }
}