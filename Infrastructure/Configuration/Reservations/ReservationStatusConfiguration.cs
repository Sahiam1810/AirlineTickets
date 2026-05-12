using System;
using Domain.Entities.Reservations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration.Reservations;

public sealed class ReservationStatusConfiguration : IEntityTypeConfiguration<ReservationStatus>
{
    public void Configure(EntityTypeBuilder<ReservationStatus> builder)
    {
        builder.ToTable("reservationstatuses");
        builder.HasKey(rs => rs.Id);
        builder.Property(rs => rs.Id).HasColumnName("id");
        builder.Property(rs => rs.Name).HasColumnName("name").HasMaxLength(50).IsRequired();
        builder.HasIndex(rs => rs.Name).IsUnique();
    }
}