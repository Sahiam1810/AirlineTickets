using System;
using Domain.Entities.Staff;
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
        builder.Property(s => s.PersonId).HasColumnName("person_id");
        builder.Property(s => s.RoleId).HasColumnName("role_id");
        builder.Property(s => s.AirlineId).HasColumnName("airline_id");
        builder.Property(s => s.AirportId).HasColumnName("airport_id");
        builder.Property(s => s.HireDate).HasColumnName("hire_date");
        builder.Property(s => s.IsActive).HasColumnName("is_active");
        builder.Property(s => s.CreatedAt).HasColumnName("created_at");
        builder.Property(s => s.UpdatedAt).HasColumnName("updated_at");
        builder.HasIndex(s => s.PersonId).IsUnique();
        builder.HasOne(s => s.Person)
            .WithMany()
            .HasForeignKey(s => s.PersonId);
        builder.HasOne(s => s.Role)
            .WithMany(sr => sr.StaffMembers)
            .HasForeignKey(s => s.RoleId);
        builder.HasOne(s => s.Airline)
            .WithMany()
            .HasForeignKey(s => s.AirlineId);
        builder.HasOne(s => s.Airport)
            .WithMany()
            .HasForeignKey(s => s.AirportId);
    }
}