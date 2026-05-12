using System;
using Domain.Entities.Airlines;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration.Airlines;

public sealed class AirportConfiguration : IEntityTypeConfiguration<Airport>
{
    public void Configure(EntityTypeBuilder<Airport> builder)
    {
        builder.ToTable("airports");
        builder.HasKey(a => a.Id);
        builder.Property(a => a.Id).HasColumnName("id");
        builder.Property(a => a.Name).HasColumnName("name").HasMaxLength(150).IsRequired();
        builder.Property(a => a.IataCode).HasColumnName("iata_code").HasMaxLength(3).IsRequired();
        builder.Property(a => a.IcaoCode).HasColumnName("icao_code").HasMaxLength(4);
        builder.Property(a => a.CityId).HasColumnName("city_id");
        builder.HasIndex(a => a.IataCode).IsUnique();
        builder.HasIndex(a => a.IcaoCode).IsUnique();
        builder.HasOne(a => a.City)
            .WithMany()
            .HasForeignKey(a => a.CityId);
    }
}