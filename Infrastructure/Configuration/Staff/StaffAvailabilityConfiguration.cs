using System;
using Domain.Entities.Staff;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration.Staff;

public sealed class StaffAvailabilityConfiguration : IEntityTypeConfiguration<StaffAvailability>
{
    public void Configure(EntityTypeBuilder<StaffAvailability> builder)
    {
        builder.ToTable("staffavailability");
        builder.HasKey(sa => sa.Id);
        builder.Property(sa => sa.Id).HasColumnName("id");
        builder.Property(sa => sa.StaffMemberId).HasColumnName("staff_member_id");
        builder.Property(sa => sa.AvailabilityStatusId).HasColumnName("availability_status_id");
        builder.Property(sa => sa.StartDate).HasColumnName("start_date");
        builder.Property(sa => sa.EndDate).HasColumnName("end_date");
        builder.Property(sa => sa.Observation).HasColumnName("observation").HasMaxLength(255);
        builder.HasOne(sa => sa.StaffMember)
            .WithMany(s => s.Availabilities)
            .HasForeignKey(sa => sa.StaffMemberId);
        builder.HasOne(sa => sa.AvailabilityStatus)
            .WithMany(a => a.StaffAvailabilities)
            .HasForeignKey(sa => sa.AvailabilityStatusId);
    }
}