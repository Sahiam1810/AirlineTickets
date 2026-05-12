using System;
using Domain.Entities.Staff;
using Domain.ValueObjects.Staff;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration.Staff;

public sealed class StaffMemberConfiguration : IEntityTypeConfiguration<StaffMember>
{
    public void Configure(EntityTypeBuilder<StaffMember> builder)
    {
        builder.ToTable("staff");
        builder.HasKey(s => s.Id);
        builder.Property(s => s.Id).HasColumnName("id");
        builder.Property(s => s.PersonId).HasColumnName("person_id").IsRequired();
        builder.Property(s => s.StaffRoleId).HasColumnName("staff_role_id").IsRequired();
        builder.Property(s => s.AirlineId).HasColumnName("airline_id");
        builder.Property(s => s.AirportId).HasColumnName("airport_id");
        builder.Property(s => s.HireDate)
            .HasColumnName("hire_date")
            .IsRequired()
            .HasConversion(
                v => v.Value,
                v => HireDate.Create(v));
        builder.Property(s => s.IsActive).HasColumnName("is_active").IsRequired();
        builder.Property(s => s.CreatedAt).HasColumnName("created_at");
        builder.Property(s => s.UpdatedAt).HasColumnName("updated_at");
        builder.HasIndex(s => s.PersonId).IsUnique();
        builder.HasOne(s => s.Person)
            .WithOne()
            .HasForeignKey<StaffMember>(s => s.PersonId)
            .OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(s => s.StaffRole)
            .WithMany()
            .HasForeignKey(s => s.StaffRoleId)
            .OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(s => s.Airline)
            .WithMany()
            .HasForeignKey(s => s.AirlineId)
            .OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(s => s.Airport)
            .WithMany()
            .HasForeignKey(s => s.AirportId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
