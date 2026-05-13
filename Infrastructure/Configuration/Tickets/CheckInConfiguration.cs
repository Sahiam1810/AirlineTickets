using Domain.Entities.Tickets;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration.Tickets;

public sealed class CheckInConfiguration : IEntityTypeConfiguration<CheckIn>
{
    public void Configure(EntityTypeBuilder<CheckIn> builder)
    {
        builder.ToTable("checkins");
        builder.HasKey(ci => ci.Id);
        builder.Property(ci => ci.Id).HasColumnName("id");

        builder.Property(ci => ci.TicketId)
            .HasColumnName("ticket_id")
            .IsRequired();

        builder.Property(ci => ci.StaffId)
            .HasColumnName("staff_id")
            .IsRequired();

        builder.Property(ci => ci.FlightSeatId)
            .HasColumnName("flight_seat_id")
            .IsRequired();

        builder.Property(ci => ci.CheckInDate)
            .HasColumnName("checkin_date")
            .IsRequired();

        builder.Property(ci => ci.CheckInStatusId)
            .HasColumnName("checkin_status_id")
            .IsRequired();

        builder.Property(ci => ci.BoardingPassNumber)
            .HasColumnName("boarding_pass_number")
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(ci => ci.HasCheckedBaggage)
            .HasColumnName("has_checked_baggage")
            .IsRequired();

        builder.Property(ci => ci.CheckedBaggageWeightKg)
            .HasColumnName("checked_baggage_weight_kg")
            .HasPrecision(5, 2)
            .IsRequired();

        builder.ToTable(t => t.HasCheckConstraint("chk_checkin_checked_baggage_weight", "checked_baggage_weight_kg >= 0"));

        builder.Property(ci => ci.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired()
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.Property(ci => ci.UpdatedAt)
            .HasColumnName("updated_at")
            .IsRequired()
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.HasIndex(ci => ci.TicketId).IsUnique();
        builder.HasIndex(ci => ci.FlightSeatId).IsUnique();
        builder.HasIndex(ci => ci.BoardingPassNumber).IsUnique();

        builder.HasOne(ci => ci.Ticket)
            .WithOne(t => t.CheckIn)
            .HasForeignKey<CheckIn>(ci => ci.TicketId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(ci => ci.Staff)
            .WithMany()
            .HasForeignKey(ci => ci.StaffId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(ci => ci.FlightSeat)
            .WithOne()
            .HasForeignKey<CheckIn>(ci => ci.FlightSeatId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(ci => ci.CheckInStatus)
            .WithMany()
            .HasForeignKey(ci => ci.CheckInStatusId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
