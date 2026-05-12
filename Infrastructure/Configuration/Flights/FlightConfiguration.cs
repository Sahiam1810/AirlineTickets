using System;
using Domain.Entities.Flights;
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
        builder.Property(f => f.FlightCode).HasColumnName("flight_code").HasMaxLength(10).IsRequired();
        builder.Property(f => f.AirlineId).HasColumnName("airline_id");
        builder.Property(f => f.RouteId).HasColumnName("route_id");
        builder.Property(f => f.AircraftUnitId).HasColumnName("aircraft_unit_id");
        builder.Property(f => f.DepartureDate).HasColumnName("departure_date");
        builder.Property(f => f.EstimatedArrivalDate).HasColumnName("estimated_arrival_date");
        builder.Property(f => f.TotalCapacity).HasColumnName("total_capacity");
        builder.Property(f => f.AvailableSeats).HasColumnName("available_seats");
        builder.Property(f => f.FlightStateId).HasColumnName("flight_state_id");
        builder.Property(f => f.RescheduledAt).HasColumnName("rescheduled_at");
        builder.Property(f => f.CreatedAt).HasColumnName("created_at");
        builder.Property(f => f.UpdatedAt).HasColumnName("updated_at");
        builder.HasIndex(f => f.FlightCode).IsUnique();
        builder.HasOne(f => f.Airline)
            .WithMany()
            .HasForeignKey(f => f.AirlineId);
        builder.HasOne(f => f.Route)
            .WithMany()
            .HasForeignKey(f => f.RouteId);
        builder.HasOne(f => f.AircraftUnit)
            .WithMany()
            .HasForeignKey(f => f.AircraftUnitId);
        builder.HasOne(f => f.FlightState)
            .WithMany(fs => fs.Flights)
            .HasForeignKey(f => f.FlightStateId);
    }
}