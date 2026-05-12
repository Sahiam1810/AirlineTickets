using System;
using Domain.Entities.Routes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration.Routes;

public sealed class FareConfiguration : IEntityTypeConfiguration<Fare>
{
    public void Configure(EntityTypeBuilder<Fare> builder)
    {
        builder.ToTable("fares");
        builder.HasKey(f => f.Id);
        builder.Property(f => f.Id).HasColumnName("id");
        builder.Property(f => f.RouteId).HasColumnName("route_id");
        builder.Property(f => f.CabinTypeId).HasColumnName("cabin_type_id");
        builder.Property(f => f.PassengerTypeId).HasColumnName("passenger_type_id");
        builder.Property(f => f.SeasonId).HasColumnName("season_id");
        builder.Property(f => f.BasePrice).HasColumnName("base_price").HasPrecision(18, 2);
        builder.Property(f => f.ValidFrom).HasColumnName("valid_from");
        builder.Property(f => f.ValidUntil).HasColumnName("valid_until");
        builder.HasOne(f => f.Route)
            .WithMany()
            .HasForeignKey(f => f.RouteId);
        builder.HasOne(f => f.CabinType)
            .WithMany()
            .HasForeignKey(f => f.CabinTypeId);
        builder.HasOne(f => f.PassengerType)
            .WithMany()
            .HasForeignKey(f => f.PassengerTypeId);
        builder.HasOne(f => f.Season)
            .WithMany(s => s.Fares)
            .HasForeignKey(f => f.SeasonId);
    }
}