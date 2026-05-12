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
        builder.Property(fst => fst.OriginStateId).HasColumnName("origin_state_id");
        builder.Property(fst => fst.DestinationStateId).HasColumnName("destination_state_id");
        builder.HasIndex(fst => new { fst.OriginStateId, fst.DestinationStateId }).IsUnique();
        builder.HasOne(fst => fst.OriginState)
            .WithMany(fs => fs.OriginTransitions)
            .HasForeignKey(fst => fst.OriginStateId);
        builder.HasOne(fst => fst.DestinationState)
            .WithMany(fs => fs.DestinationTransitions)
            .HasForeignKey(fst => fst.DestinationStateId);
    }
}