using System;
using Domain.ValueObjects.Routes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Infrastructure.Configuration.Routes;

public sealed class RouteConfiguration : IEntityTypeConfiguration<Domain.Entities.Routes.Route>
{
    public void Configure(EntityTypeBuilder<Domain.Entities.Routes.Route> builder)
    {
        var distanceKmConverter = new ValueConverter<DistanceKm?, int?>(
            v => v == null ? null : v.Value,
            v => DistanceKm.Create(v));

        var durationMinutesConverter = new ValueConverter<DurationMinutes?, int?>(
            v => v == null ? null : v.Value,
            v => DurationMinutes.Create(v));

        builder.ToTable("routes");
        builder.HasKey(r => r.Id);
        builder.Property(r => r.Id).HasColumnName("id");
        builder.Property(r => r.OriginAirportId).HasColumnName("origin_airport_id").IsRequired();
        builder.Property(r => r.DestinationAirportId).HasColumnName("destination_airport_id").IsRequired();
        builder.Property(r => r.DistanceKm)
            .HasColumnName("distance_km")
            .HasConversion(distanceKmConverter)
            .IsRequired(false);
        builder.Property(r => r.EstimatedDurationMinutes)
            .HasColumnName("estimated_duration_min")
            .HasConversion(durationMinutesConverter)
            .IsRequired(false);
        builder.HasIndex(r => new { r.OriginAirportId, r.DestinationAirportId }).IsUnique();
        builder.HasOne(r => r.OriginAirport)
            .WithMany()
            .HasForeignKey(r => r.OriginAirportId)
            .OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(r => r.DestinationAirport)
            .WithMany()
            .HasForeignKey(r => r.DestinationAirportId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
