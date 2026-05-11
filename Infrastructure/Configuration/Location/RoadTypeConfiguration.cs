using System;
using Domain.Entities.Location;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration.Location;

public sealed class RoadTypeConfiguration : IEntityTypeConfiguration<RoadType>
{
    public void Configure(EntityTypeBuilder<RoadType> builder)
    {
        builder.ToTable("roadtypes");
        builder.HasKey(r => r.Id);
        builder.Property(r => r.Id).HasColumnName("id");
        builder.Property(r => r.Name).HasColumnName("name").HasMaxLength(50).IsRequired();
    }
}