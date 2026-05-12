using System;
using Domain.Entities.Flights;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration.Flights;

public sealed class SeatLocationTypeConfiguration : IEntityTypeConfiguration<SeatLocationType>
{
    public void Configure(EntityTypeBuilder<SeatLocationType> builder)
    {
        builder.ToTable("seatlocationtypes");
        builder.HasKey(slt => slt.Id);
        builder.Property(slt => slt.Id).HasColumnName("id");
        builder.Property(slt => slt.Name).HasColumnName("name").HasMaxLength(50).IsRequired();
        builder.HasIndex(slt => slt.Name).IsUnique();
    }
}