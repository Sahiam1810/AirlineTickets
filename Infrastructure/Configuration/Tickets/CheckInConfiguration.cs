using System;
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
        builder.Property(ci => ci.TicketId).HasColumnName("ticket_id");
        builder.Property(ci => ci.StaffId).HasColumnName("staff_id");
        builder.Property(ci => ci.FlightSeatId).HasColumnName("flight_seat_id");
        builder.Property(ci => ci.CheckInDate).HasColumnName("checkin_date");
        builder.Property(ci => ci.CheckInStatusId).HasColumnName("checkin_status_id");
        builder.Property(ci => ci.BoardingCardNumber).HasColumnName("boarding_card_number").HasMaxLength(20).IsRequired();
        builder.Property(ci => ci.HasCheckedBaggage).HasColumnName("has_checked_baggage");
        builder.Property(ci => ci.BaggageWeightKg).HasColumnName("baggage_weight_kg").HasPrecision(5, 2);
        builder.HasIndex(ci => ci.TicketId).IsUnique();
        builder.HasIndex(ci => ci.FlightSeatId).IsUnique();
        builder.HasIndex(ci => ci.BoardingCardNumber).IsUnique();
        builder.HasOne(ci => ci.Ticket)
            .WithOne(t => t.CheckIn)
            .HasForeignKey<CheckIn>(ci => ci.TicketId);
        builder.HasOne(ci => ci.Staff)
            .WithMany()
            .HasForeignKey(ci => ci.StaffId);
        builder.HasOne(ci => ci.FlightSeat)
            .WithMany()
            .HasForeignKey(ci => ci.FlightSeatId);
        builder.HasOne(ci => ci.CheckInStatus)
            .WithMany(cs => cs.CheckIns)
            .HasForeignKey(ci => ci.CheckInStatusId);
    }
}