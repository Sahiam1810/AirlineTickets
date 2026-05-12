using Domain.Entities.Geography;
using Domain.ValueObjects.Geography;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration.Geography;

public sealed class CountryConfiguration : IEntityTypeConfiguration<Country>
{
    public void Configure(EntityTypeBuilder<Country> builder)
    {
        builder.ToTable("countries");
        builder.HasKey(c => c.Id);
        builder.Property(c => c.Id).HasColumnName("id");
        builder.Property(c => c.Name)
            .HasColumnName("name")
            .HasMaxLength(100)
            .IsRequired()
            .HasConversion(v => v.Value, v => CountryName.Create(v));
        builder.Property(c => c.ContinentId)
            .HasColumnName("continent_id")
            .IsRequired();
        builder.HasIndex(c => c.Name).IsUnique();
        builder.HasOne(c => c.Continent)
            .WithMany(ct => ct.Countries)
            .HasForeignKey(c => c.ContinentId)
            .IsRequired();
    }
}
