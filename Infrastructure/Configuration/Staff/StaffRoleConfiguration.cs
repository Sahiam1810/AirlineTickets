using System;
using Domain.Entities.Staff;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration.Staff;

public sealed class StaffRoleConfiguration : IEntityTypeConfiguration<StaffRole>
{
    public void Configure(EntityTypeBuilder<StaffRole> builder)
    {
        builder.ToTable("staffroles");
        builder.HasKey(sr => sr.Id);
        builder.Property(sr => sr.Id).HasColumnName("id");
        builder.Property(sr => sr.Name).HasColumnName("name").HasMaxLength(100).IsRequired();
        builder.HasIndex(sr => sr.Name).IsUnique();
    }
}