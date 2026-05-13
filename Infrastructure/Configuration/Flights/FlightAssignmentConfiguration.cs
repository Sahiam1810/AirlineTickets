using System;
using Domain.Entities.Flights;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration.Flights;

public sealed class FlightAssignmentConfiguration : IEntityTypeConfiguration<FlightAssignment>
{
    public void Configure(EntityTypeBuilder<FlightAssignment> builder)
    {
        builder.ToTable("flightassignments");
        builder.HasKey(fa => fa.Id);
        builder.Property(fa => fa.Id).HasColumnName("id");
        builder.Property(fa => fa.FlightId).HasColumnName("flight_id");
        builder.Property(fa => fa.StaffId).HasColumnName("staff_id");
        builder.Property(fa => fa.FlightRoleId).HasColumnName("flight_role_id");
        builder.HasIndex(fa => new { fa.FlightId, fa.StaffId }).IsUnique();

        builder.HasOne(fa => fa.Flight)
            .WithMany()
            .HasForeignKey(fa => fa.FlightId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(fa => fa.Staff)
            .WithMany()
            .HasForeignKey(fa => fa.StaffId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(fa => fa.FlightRole)
            .WithMany()
            .HasForeignKey(fa => fa.FlightRoleId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
