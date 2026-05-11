using System;
using Domain.Entities.People;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration.People;

public sealed class PassengerTypeConfiguration : IEntityTypeConfiguration<PassengerType>
{
    public void Configure(EntityTypeBuilder<PassengerType> builder)
    {
        builder.ToTable("passengertypes");
        builder.HasKey(pt => pt.Id);
        builder.Property(pt => pt.Id).HasColumnName("id");
        builder.Property(pt => pt.Name).HasColumnName("name").HasMaxLength(50).IsRequired();
        builder.Property(pt => pt.MinAge).HasColumnName("min_age");
        builder.Property(pt => pt.MaxAge).HasColumnName("max_age");
        builder.HasIndex(pt => pt.Name).IsUnique();
    }
}