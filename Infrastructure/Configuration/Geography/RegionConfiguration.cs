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
        builder.Property(r => r.Type)
            .HasColumnName("type")
            .HasMaxLength(30)
            .IsRequired()
            .HasConversion(v => v.Value, v => RegionType.Create(v));
        builder.Property(r => r.CountryId)
            .HasColumnName("country_id")
            .IsRequired();
        builder.HasIndex(r => new { r.Name, r.CountryId }).IsUnique();
        builder.HasOne(r => r.Country)
            .WithMany()
            .HasForeignKey(r => r.CountryId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
