using System;
using Domain.Entities.Airlines;
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
        builder.Property(aa => aa.Terminal).HasColumnName("terminal").HasMaxLength(20);
        builder.Property(aa => aa.StartDate).HasColumnName("start_date");
        builder.Property(aa => aa.EndDate).HasColumnName("end_date");
        builder.Property(aa => aa.IsActive).HasColumnName("is_active");
        builder.HasIndex(aa => new { aa.AirportId, aa.AirlineId }).IsUnique();
        builder.HasOne(aa => aa.Airport)
            .WithMany(a => a.AirportAirlines)
            .HasForeignKey(aa => aa.AirportId);
        builder.HasOne(aa => aa.Airline)
            .WithMany(a => a.AirportAirlines)
            .HasForeignKey(aa => aa.AirlineId);
    }
}