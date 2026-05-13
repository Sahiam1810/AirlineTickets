using System;
using Domain.Entities.Auth;
using Domain.ValueObjects.Auth;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration.Auth;

public sealed class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("users");
        builder.HasKey(u => u.Id);
        builder.Property(u => u.Id).HasColumnName("id");
        builder.Property(u => u.Username)
            .HasConversion(
                username => username.Value,
                value => Username.Create(value))
            .HasColumnName("username")
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(u => u.PasswordHash)
            .HasConversion(
                hash => hash.Value,
                value => PasswordHash.Create(value))
            .HasColumnName("password_hash")
            .HasMaxLength(255)
            .IsRequired();
        builder.Property(u => u.PersonId).HasColumnName("person_id");
        builder.Property(u => u.RoleId).HasColumnName("role_id");
        builder.Property(u => u.IsActive).HasColumnName("is_active");
        builder.Property(u => u.LastAccess).HasColumnName("last_access");
        builder.Property(u => u.CreatedAt).HasColumnName("created_at");
        builder.Property(u => u.UpdatedAt).HasColumnName("updated_at");
        builder.HasIndex(u => u.Username).IsUnique();
        builder.HasIndex(u => u.PersonId).IsUnique();
        builder.HasOne(u => u.Person)
            .WithMany()
            .HasForeignKey(u => u.PersonId);
        builder.HasOne(u => u.SystemRole)
            .WithMany(sr => sr.Users)
            .HasForeignKey(u => u.RoleId);
    }
}