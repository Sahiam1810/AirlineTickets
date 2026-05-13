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
        builder.Property(rst => rst.FromStatusId).HasColumnName("from_status_id");
        builder.Property(rst => rst.ToStatusId).HasColumnName("to_status_id");
        builder.HasIndex(rst => new { rst.FromStatusId, rst.ToStatusId }).IsUnique();

        builder.HasOne(rst => rst.FromStatus)
            .WithMany()
            .HasForeignKey(rst => rst.FromStatusId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(rst => rst.ToStatus)
            .WithMany()
            .HasForeignKey(rst => rst.ToStatusId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
