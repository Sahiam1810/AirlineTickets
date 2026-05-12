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
        builder.Property(fa => fa.StaffMemberId).HasColumnName("staff_member_id");
        builder.Property(fa => fa.FlightRoleId).HasColumnName("flight_role_id");
        builder.HasIndex(fa => new { fa.FlightId, fa.StaffMemberId }).IsUnique();
        builder.HasOne(fa => fa.Flight)
            .WithMany(f => f.FlightAssignments)
            .HasForeignKey(fa => fa.FlightId);
        builder.HasOne(fa => fa.StaffMember)
            .WithMany()
            .HasForeignKey(fa => fa.StaffMemberId);
        builder.HasOne(fa => fa.FlightRole)
            .WithMany(fr => fr.FlightAssignments)
            .HasForeignKey(fa => fa.FlightRoleId);
    }
}