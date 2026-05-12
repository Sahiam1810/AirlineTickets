using System;
using Domain.Entities.Airlines;
using Domain.ValueObjects.AirportAirlines;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration.Airlines;

public sealed class AirportAirlineConfiguration : IEntityTypeConfiguration<AirportAirline>
{
    public void Configure(EntityTypeBuilder<AirportAirline> builder)
    {
        builder.ToTable("airportairline");
        builder.HasKey(aa => aa.Id);
        builder.Property(aa => aa.Id).HasColumnName("id");
        builder.Property(aa => aa.AirportId).HasColumnName("airport_id");
        builder.Property(aa => aa.AirlineId).HasColumnName("airline_id");
        builder.Property(aa => aa.Terminal)
            .HasColumnName("terminal")
            .HasMaxLength(20)
            .HasConversion(
                v => v == null ? null : v.Value,
                v => Terminal.Create(v));
        builder.Property(aa => aa.StartDate).HasColumnName("start_date");
        builder.Property(aa => aa.EndDate).HasColumnName("end_date");
        builder.Property(aa => aa.IsActive).HasColumnName("is_active").IsRequired();
        builder.HasIndex(aa => new { aa.AirportId, aa.AirlineId }).IsUnique();
        builder.HasOne(aa => aa.Airport)
            .WithMany()
            .HasForeignKey(aa => aa.AirportId)
            .OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(aa => aa.Airline)
            .WithMany()
            .HasForeignKey(aa => aa.AirlineId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
