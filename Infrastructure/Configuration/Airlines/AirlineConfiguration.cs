using System;
using Domain.Entities.Airlines;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration.Airlines;

public sealed class AirlineConfiguration : IEntityTypeConfiguration<Airline>
{
    public void Configure(EntityTypeBuilder<Airline> builder)
    {
        builder.ToTable("airlines");
        builder.HasKey(a => a.Id);
        builder.Property(a => a.Id).HasColumnName("id");
        builder.Property(a => a.Name).HasColumnName("name").HasMaxLength(150).IsRequired();
        builder.Property(a => a.IataCode).HasColumnName("iata_code").HasMaxLength(3).IsRequired();
        builder.Property(a => a.OriginCountryId).HasColumnName("origin_country_id");
        builder.Property(a => a.IsActive).HasColumnName("is_active");
        builder.Property(a => a.CreatedAt).HasColumnName("created_at");
        builder.Property(a => a.UpdatedAt).HasColumnName("updated_at");
        builder.HasIndex(a => a.IataCode).IsUnique();
        builder.HasOne(a => a.OriginCountry)
            .WithMany()
            .HasForeignKey(a => a.OriginCountryId);
    }
}