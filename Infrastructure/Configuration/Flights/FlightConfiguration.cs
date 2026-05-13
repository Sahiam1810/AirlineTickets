using System;
using Domain.Entities.Flights;
using Domain.ValueObjects.Flights;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration.Flights;

public sealed class FlightConfiguration : IEntityTypeConfiguration<Flight>
{
    public void Configure(EntityTypeBuilder<Flight> builder)
    {
        builder.ToTable("flights");
        builder.HasKey(f => f.Id);
        builder.Property(f => f.Id).HasColumnName("id");
        builder.Property(f => f.FlightCode)
            .HasColumnName("flight_code")
            .HasMaxLength(10)
            .IsRequired()
            .HasConversion(
                v => v.Value,
                v => FlightCode.Create(v));
        builder.Property(f => f.AirlineId).HasColumnName("airline_id");
        builder.Property(f => f.RouteId).HasColumnName("route_id");
        builder.Property(f => f.AircraftId).HasColumnName("aircraft_id");
        builder.Property(f => f.DepartureDate).HasColumnName("departure_date");
        builder.Property(f => f.EstimatedArrivalDate).HasColumnName("estimated_arrival_date");
        builder.Property(f => f.TotalCapacity)
            .HasColumnName("total_capacity")
            .IsRequired()
            .HasConversion(
                v => v.Value,
                v => Capacity.Create(v));
        builder.Property(f => f.AvailableSeats)
            .HasColumnName("available_seats")
            .IsRequired()
            .HasConversion(
                v => v.Value,
                v => AvailableSeats.Create(v, Capacity.Create(int.MaxValue)));
        builder.Property(f => f.FlightStateId).HasColumnName("flight_state_id");
        builder.Property(f => f.RescheduledAt).HasColumnName("rescheduled_at");
        builder.Property(f => f.CreatedAt).HasColumnName("created_at");
        builder.Property(f => f.UpdatedAt).HasColumnName("updated_at");
        builder.HasIndex(f => f.FlightCode).IsUnique();
        builder.HasOne(f => f.Airline)
            .WithMany()
            .HasForeignKey(f => f.AirlineId)
            .OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(f => f.Route)
            .WithMany()
            .HasForeignKey(f => f.RouteId)
            .OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(f => f.Aircraft)
            .WithMany()
            .HasForeignKey(f => f.AircraftId)
            .OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(f => f.FlightState)
            .WithMany(fs => fs.Flights)
            .HasForeignKey(f => f.FlightStateId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
