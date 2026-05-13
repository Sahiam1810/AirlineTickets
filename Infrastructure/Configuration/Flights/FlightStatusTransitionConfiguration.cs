using System;
using Domain.Entities.Flights;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration.Flights;

public sealed class FlightStatusTransitionConfiguration : IEntityTypeConfiguration<FlightStatusTransition>
{
    public void Configure(EntityTypeBuilder<FlightStatusTransition> builder)
    {
        builder.ToTable("flightstatustransitions");
        builder.HasKey(fst => fst.Id);
        builder.Property(fst => fst.Id).HasColumnName("id");
        builder.Property(fst => fst.FromStateId).HasColumnName("from_state_id");
        builder.Property(fst => fst.ToStateId).HasColumnName("to_state_id");
        builder.HasIndex(fst => new { fst.FromStateId, fst.ToStateId }).IsUnique();
        builder.HasOne(fst => fst.FromState)
            .WithMany()
            .HasForeignKey(fst => fst.FromStateId)
            .OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(fst => fst.ToState)
            .WithMany()
            .HasForeignKey(fst => fst.ToStateId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
