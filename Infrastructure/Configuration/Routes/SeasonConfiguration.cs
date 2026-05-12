using System;
using Domain.Entities.Routes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration.Routes;

public sealed class SeasonConfiguration : IEntityTypeConfiguration<Season>
{
    public void Configure(EntityTypeBuilder<Season> builder)
    {
        builder.ToTable("seasons");
        builder.HasKey(s => s.Id);
        builder.Property(s => s.Id).HasColumnName("id");
        builder.Property(s => s.Name).HasColumnName("name").HasMaxLength(50).IsRequired();
        builder.Property(s => s.Description).HasColumnName("description").HasMaxLength(150);
        builder.Property(s => s.PriceFactor).HasColumnName("price_factor").HasPrecision(5, 4);
        builder.HasIndex(s => s.Name).IsUnique();
    }
}