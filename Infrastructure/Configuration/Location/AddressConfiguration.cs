using System;
using Domain.Entities.Location;
using Domain.ValueObjects.Location;
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
        builder.Property(a => a.RoadTypeId)
            .HasColumnName("road_type_id")
            .IsRequired();
        builder.Property(a => a.StreetName)
            .HasColumnName("street_name")
            .HasMaxLength(100)
            .IsRequired()
            .HasConversion(v => v.Value, v => StreetName.Create(v));
        builder.Property(a => a.Number)
            .HasColumnName("number")
            .HasMaxLength(20)
            .HasConversion(v => v == null ? null : v.Value, v => AddressNumber.Create(v));
        builder.Property(a => a.Complement)
            .HasColumnName("complement")
            .HasMaxLength(100)
            .HasConversion(v => v == null ? null : v.Value, v => AddressComplement.Create(v));
        builder.Property(a => a.CityId)
            .HasColumnName("city_id")
            .IsRequired();
        builder.Property(a => a.PostalCode)
            .HasColumnName("postal_code")
            .HasMaxLength(20)
            .HasConversion(v => v == null ? null : v.Value, v => PostalCode.Create(v));
        builder.HasOne(a => a.RoadType)
            .WithMany()
            .HasForeignKey(a => a.RoadTypeId)
            .OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(a => a.City)
            .WithMany()
            .HasForeignKey(a => a.CityId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
