using System;
using Domain.Entities.Auth;
using Domain.ValueObjects.Auth;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration.Auth;

public sealed class SessionConfiguration : IEntityTypeConfiguration<Session>
{
    public void Configure(EntityTypeBuilder<Session> builder)
    {
        builder.ToTable("sessions");
        builder.HasKey(s => s.Id);
        builder.Property(s => s.Id).HasColumnName("id");
        builder.Property(s => s.UserId).HasColumnName("user_id");
        builder.Property(s => s.StartedAt).HasColumnName("started_at");
        builder.Property(s => s.EndedAt).HasColumnName("closed_at");
        builder.Property(s => s.IpAddress)
            .HasConversion(
                ip => ip == null ? null : ip.Value,
                value => value == null ? null : IpAddress.Create(value))
            .HasColumnName("ip_origin")
            .HasMaxLength(45);
        builder.Property(s => s.IsActive).HasColumnName("is_active");
        builder.HasOne(s => s.User)
            .WithMany(u => u.Sessions)
            .HasForeignKey(s => s.UserId);
    }
}