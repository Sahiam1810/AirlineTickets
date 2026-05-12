using Domain.Entities.Geography;
using Domain.ValueObjects.Continents;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration.Geography;

public sealed class ContinentConfiguration : IEntityTypeConfiguration<Continent>
{
    public void Configure(EntityTypeBuilder<Continent> builder)
    {
        builder.ToTable("continents");
        builder.HasKey(c => c.Id);
        builder.Property(c => c.Id).HasColumnName("id");
        builder.Property(c => c.Name)
            .HasColumnName("name")
            .HasMaxLength(100)
            .IsRequired()
            .HasConversion(
                v => v.Value,
                v => ContinentName.Create(v));
        builder.HasIndex(c => c.Name).IsUnique();

        
    }
}