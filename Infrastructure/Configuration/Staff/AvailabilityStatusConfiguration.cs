using System;
using Domain.Entities.Staff;
using Domain.ValueObjects.Staff;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration.Staff;

public sealed class AvailabilityStatusConfiguration : IEntityTypeConfiguration<AvailabilityStatus>
{
    public void Configure(EntityTypeBuilder<AvailabilityStatus> builder)
    {
        builder.ToTable("availabilitystatuses");
        builder.HasKey(a => a.Id);
        builder.Property(a => a.Id).HasColumnName("id");
        builder.Property(a => a.Name)
            .HasColumnName("name")
            .HasMaxLength(50)
            .IsRequired()
            .HasConversion(
                v => v.Value,
                v => AvailabilityStatusName.Create(v));
        builder.HasIndex(a => a.Name).IsUnique();
    }
}
