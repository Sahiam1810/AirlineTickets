using System;
using Domain.Entities.Airlines;
using Domain.ValueObjects.Airlines;
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
        builder.Property(a => a.Name)
            .HasColumnName("name")
            .HasMaxLength(150)
            .IsRequired()
            .HasConversion(
                v => v.Value,
                v => AirlineName.Create(v));
        builder.Property(a => a.IataCode)
            .HasColumnName("iata_code")
            .HasMaxLength(3)
            .IsRequired()
            .HasConversion(
                v => v.Value,
                v => IataCode.Create(v));
        builder.Property(a => a.CountryId).HasColumnName("country_id").IsRequired();
        builder.Property(a => a.IsActive).HasColumnName("is_active");
        builder.Property(a => a.CreatedAt).HasColumnName("created_at");
        builder.Property(a => a.UpdatedAt).HasColumnName("updated_at");
        builder.HasIndex(a => a.IataCode).IsUnique();
        builder.HasOne(a => a.Country)
            .WithMany()
            .HasForeignKey(a => a.CountryId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
