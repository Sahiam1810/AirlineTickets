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

        builder.Property(rf => rf.ReservationId)
            .HasColumnName("reservation_id")
            .IsRequired();

        builder.Property(rf => rf.FlightId)
            .HasColumnName("flight_id")
            .IsRequired();

        builder.Property(rf => rf.PartialValue)
            .HasColumnName("partial_value")
            .HasPrecision(18, 2)
            .IsRequired();

        builder.ToTable(t => t.HasCheckConstraint("chk_reservationflight_partial_value", "partial_value >= 0"));

        builder.Property(rf => rf.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired()
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.Property(rf => rf.UpdatedAt)
            .HasColumnName("updated_at")
            .IsRequired()
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.HasIndex(rf => new { rf.ReservationId, rf.FlightId }).IsUnique();

        builder.HasOne(rf => rf.Reservation)
            .WithMany(r => r.ReservationFlights)
            .HasForeignKey(rf => rf.ReservationId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(rf => rf.Flight)
            .WithMany()
            .HasForeignKey(rf => rf.FlightId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
