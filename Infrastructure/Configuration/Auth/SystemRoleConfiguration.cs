using System;
using Domain.Entities.Auth;
using Domain.ValueObjects.Auth;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration.Auth;

public sealed class SystemRoleConfiguration : IEntityTypeConfiguration<SystemRole>
{
    public void Configure(EntityTypeBuilder<SystemRole> builder)
    {
        builder.ToTable("systemroles");
        builder.HasKey(sr => sr.Id);
        builder.Property(sr => sr.Id).HasColumnName("id");
        builder.Property(sr => sr.Name)
            .HasConversion(
                name => name.Value,
                value => SystemRoleName.Create(value))
            .HasColumnName("name")
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(sr => sr.Description)
            .HasColumnName("description")
            .HasMaxLength(150);

        builder.HasIndex(sr => sr.Name).IsUnique();
    }
}