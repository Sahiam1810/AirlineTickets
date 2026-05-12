using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration.Routes;

public sealed class RouteConfiguration : IEntityTypeConfiguration<Domain.Entities.Routes.Route>
{
    public void Configure(EntityTypeBuilder<Domain.Entities.Routes.Route> builder)
    {
        builder.ToTable("routes");
        builder.HasKey(r => r.Id);
        builder.Property(r => r.Id).HasColumnName("id");
        builder.Property(r => r.OriginAirportId).HasColumnName("origin_airport_id");
        builder.Property(r => r.DestinationAirportId).HasColumnName("destination_airport_id");
        builder.Property(r => r.DistanceKm).HasColumnName("distance_km");
        builder.Property(r => r.EstimatedDurationMin).HasColumnName("estimated_duration_min");
        builder.HasIndex(r => new { r.OriginAirportId, r.DestinationAirportId }).IsUnique();
        builder.HasOne(r => r.OriginAirport)
            .WithMany()
            .HasForeignKey(r => r.OriginAirportId);
        builder.HasOne(r => r.DestinationAirport)
            .WithMany()
            .HasForeignKey(r => r.DestinationAirportId);
    }
}