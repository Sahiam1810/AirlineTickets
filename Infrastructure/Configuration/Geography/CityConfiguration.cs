using Domain.Entities.Geography;
using Domain.ValueObjects.Geography;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration.Geography;

public sealed class CityConfiguration : IEntityTypeConfiguration<City>
{
    public void Configure(EntityTypeBuilder<City> builder)
    {
        builder.ToTable("cities");
        builder.HasKey(c => c.Id);
        builder.Property(c => c.Id).HasColumnName("id");
        builder.Property(c => c.Name)
            .HasColumnName("name")
            .HasMaxLength(100)
            .IsRequired()
            .HasConversion(v => v.Value, v => CityName.Create(v));
        builder.Property(c => c.RegionId).HasColumnName("region_id");
        builder.HasOne(c => c.Region)
            .WithMany(r => r.Cities)
            .HasForeignKey(c => c.RegionId);
    }
}