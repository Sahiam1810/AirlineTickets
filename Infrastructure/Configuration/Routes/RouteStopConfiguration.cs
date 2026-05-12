using System;
using Domain.Entities.Routes;
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
        builder.Property(rs => rs.RouteId).HasColumnName("route_id");
        builder.Property(rs => rs.StopAirportId).HasColumnName("stop_airport_id");
        builder.Property(rs => rs.Order).HasColumnName("order");
        builder.Property(rs => rs.StopDurationMin).HasColumnName("stop_duration_min");
        builder.HasIndex(rs => new { rs.RouteId, rs.Order }).IsUnique();
        builder.HasOne(rs => rs.Route)
            .WithMany(r => r.RouteStops)
            .HasForeignKey(rs => rs.RouteId);
        builder.HasOne(rs => rs.StopAirport)
            .WithMany()
            .HasForeignKey(rs => rs.StopAirportId);
    }
}