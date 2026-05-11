using System;
using Domain.Entities.People;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration.People;

public sealed class PassengerConfiguration : IEntityTypeConfiguration<Passenger>
{
    public void Configure(EntityTypeBuilder<Passenger> builder)
    {
        builder.ToTable("passengers");
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Id).HasColumnName("id");
        builder.Property(p => p.PersonId).HasColumnName("person_id");
        builder.Property(p => p.PassengerTypeId).HasColumnName("passenger_type_id");
        builder.HasIndex(p => p.PersonId).IsUnique();
        builder.HasOne(p => p.Person)
            .WithMany()
            .HasForeignKey(p => p.PersonId);
        builder.HasOne(p => p.PassengerType)
            .WithMany(pt => pt.Passengers)
            .HasForeignKey(p => p.PassengerTypeId);
    }
}