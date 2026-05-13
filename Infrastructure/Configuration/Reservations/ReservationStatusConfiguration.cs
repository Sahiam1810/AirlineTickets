using System;
using Domain.Entities.Reservations;
using Domain.ValueObjects.ReservationStatuses;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Infrastructure.Configuration.Reservations;

public sealed class ReservationStatusConfiguration : IEntityTypeConfiguration<ReservationStatus>
{
    public void Configure(EntityTypeBuilder<ReservationStatus> builder)
    {
        builder.ToTable("reservationstatuses");
        builder.HasKey(rs => rs.Id);
        builder.Property(rs => rs.Id).HasColumnName("id");

        var nameConverter = new ValueConverter<ReservationStatusName, string>(
            name => name.Value,
            value => ReservationStatusName.Create(value));

        builder.Property(rs => rs.Name)
            .HasColumnName("name")
            .HasConversion(nameConverter)
            .HasMaxLength(50)
            .IsRequired();

        builder.HasIndex(rs => rs.Name).IsUnique();
    }
}
