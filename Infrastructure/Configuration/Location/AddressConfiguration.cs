using System;
using Domain.Entities.Location;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration.Location;

public sealed class AddressConfiguration : IEntityTypeConfiguration<Address>
{
    public void Configure(EntityTypeBuilder<Address> builder)
    {
        builder.ToTable("addresses");
        builder.HasKey(a => a.Id);
        builder.Property(a => a.Id).HasColumnName("id");
        builder.Property(a => a.RoadTypeId).HasColumnName("road_type_id");
        builder.Property(a => a.RoadName).HasColumnName("road_name").HasMaxLength(100).IsRequired();
        builder.Property(a => a.Number).HasColumnName("number").HasMaxLength(20);
        builder.Property(a => a.Complement).HasColumnName("complement").HasMaxLength(100);
        builder.Property(a => a.CityId).HasColumnName("city_id");
        builder.Property(a => a.PostalCode).HasColumnName("postal_code").HasMaxLength(20);
        builder.HasOne(a => a.RoadType)
            .WithMany(r => r.Addresses)
            .HasForeignKey(a => a.RoadTypeId);
        builder.HasOne(a => a.City)
            .WithMany()
            .HasForeignKey(a => a.CityId);
    }
}