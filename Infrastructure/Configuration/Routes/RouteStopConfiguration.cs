using System;
using Domain.Entities.Routes;
using Domain.ValueObjects.Routes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration.Routes;

public sealed class RouteStopConfiguration : IEntityTypeConfiguration<RouteStop>
{
    public void Configure(EntityTypeBuilder<RouteStop> builder)
    {
        builder.ToTable("routestops");
        builder.HasKey(rs => rs.Id);
        builder.Property(rs => rs.Id).HasColumnName("id");
        builder.Property(rs => rs.RouteId).HasColumnName("route_id").IsRequired();
        builder.Property(rs => rs.StopAirportId).HasColumnName("stop_airport_id").IsRequired();
        builder.Property(rs => rs.Order)
            .HasColumnName("order")
            .IsRequired()
            .HasConversion(
                v => v.Value,
                v => StopOrder.Create(v));
        builder.Property(rs => rs.StopDurationMinutes)
            .HasColumnName("stop_duration_min")
            .IsRequired()
            .HasConversion(
                v => v.Value,
                v => StopDurationMinutes.Create(v));
        builder.HasIndex(rs => new { rs.RouteId, rs.Order }).IsUnique();
        builder.HasOne(rs => rs.Route)
            .WithMany(r => r.RouteStops)
            .HasForeignKey(rs => rs.RouteId)
            .OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(rs => rs.StopAirport)
            .WithMany(a => a.RouteStops)
            .HasForeignKey(rs => rs.StopAirportId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
