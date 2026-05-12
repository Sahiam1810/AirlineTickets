using Domain.Entities.Geography;
using Domain.ValueObjects.Geography;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration.Geography;

public sealed class RegionConfiguration : IEntityTypeConfiguration<Region>
{
    public void Configure(EntityTypeBuilder<Region> builder)
    {
        builder.ToTable("regions");
        builder.HasKey(r => r.Id);
        builder.Property(r => r.Id).HasColumnName("id");
        builder.Property(r => r.Name)
            .HasColumnName("name")
            .HasMaxLength(100)
            .IsRequired()
            .HasConversion(v => v.Value, v => RegionName.Create(v));
        builder.Property(r => r.Type).HasColumnName("type").HasMaxLength(30).IsRequired();
        builder.Property(r => r.CountryId).HasColumnName("country_id");
        builder.HasOne(r => r.Country)
            .WithMany(c => c.Regions)
            .HasForeignKey(r => r.CountryId);
    }
}