using System;
using Domain.Entities.Tickets;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration.Tickets;

public sealed class CheckInStatusConfiguration : IEntityTypeConfiguration<CheckInStatus>
{
    public void Configure(EntityTypeBuilder<CheckInStatus> builder)
    {
        builder.ToTable("checkinstatuses");
        builder.HasKey(cs => cs.Id);
        builder.Property(cs => cs.Id).HasColumnName("id");
        builder.Property(cs => cs.Name).HasColumnName("name").HasMaxLength(50).IsRequired();
        builder.HasIndex(cs => cs.Name).IsUnique();
    }
}