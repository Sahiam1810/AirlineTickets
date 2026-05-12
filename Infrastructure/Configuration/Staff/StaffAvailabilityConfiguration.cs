using System;
using Domain.Entities.Staff;
using Domain.ValueObjects.Staff;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration.Staff;

public sealed class StaffAvailabilityConfiguration : IEntityTypeConfiguration<StaffAvailability>
{
    public void Configure(EntityTypeBuilder<StaffAvailability> builder)
    {
        builder.ToTable("staffavailabilities");
        builder.HasKey(sa => sa.Id);
        builder.Property(sa => sa.Id).HasColumnName("id");
        builder.Property(sa => sa.StaffId).HasColumnName("staff_id").IsRequired();
        builder.Property(sa => sa.AvailabilityStatusId).HasColumnName("availability_status_id").IsRequired();
        builder.Property(sa => sa.StartDate).HasColumnName("start_date").IsRequired();
        builder.Property(sa => sa.EndDate).HasColumnName("end_date").IsRequired();
        builder.Property(sa => sa.Notes)
            .HasColumnName("notes")
            .HasMaxLength(255)
            .HasConversion(
                v => v == null ? null : v.Value,
                v => StaffAvailabilityNotes.Create(v))
            .IsRequired(false);
        builder.HasOne(sa => sa.Staff)
            .WithMany(s => s.Availabilities)
            .HasForeignKey(sa => sa.StaffId)
            .OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(sa => sa.AvailabilityStatus)
            .WithMany(a => a.StaffAvailabilities)
            .HasForeignKey(sa => sa.AvailabilityStatusId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
