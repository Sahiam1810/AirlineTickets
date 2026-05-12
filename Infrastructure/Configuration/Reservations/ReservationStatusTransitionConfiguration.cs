using System;
using Domain.Entities.Reservations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration.Reservations;

public sealed class ReservationStatusTransitionConfiguration : IEntityTypeConfiguration<ReservationStatusTransition>
{
    public void Configure(EntityTypeBuilder<ReservationStatusTransition> builder)
    {
        builder.ToTable("reservationstatustransitions");
        builder.HasKey(rst => rst.Id);
        builder.Property(rst => rst.Id).HasColumnName("id");
        builder.Property(rst => rst.OriginStatusId).HasColumnName("origin_status_id");
        builder.Property(rst => rst.DestinationStatusId).HasColumnName("destination_status_id");
        builder.HasIndex(rst => new { rst.OriginStatusId, rst.DestinationStatusId }).IsUnique();
        builder.HasOne(rst => rst.OriginStatus)
            .WithMany(rs => rs.OriginTransitions)
            .HasForeignKey(rst => rst.OriginStatusId);
        builder.HasOne(rst => rst.DestinationStatus)
            .WithMany(rs => rs.DestinationTransitions)
            .HasForeignKey(rst => rst.DestinationStatusId);
    }
}