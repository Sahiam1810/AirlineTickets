using System;
using Domain.Entities.People;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration.People;

public sealed class PersonPhoneConfiguration : IEntityTypeConfiguration<PersonPhone>
{
    public void Configure(EntityTypeBuilder<PersonPhone> builder)
    {
        builder.ToTable("personphones");
        builder.HasKey(pp => pp.Id);
        builder.Property(pp => pp.Id).HasColumnName("id");
        builder.Property(pp => pp.PersonId).HasColumnName("person_id");
        builder.Property(pp => pp.PhoneCodeId).HasColumnName("phone_code_id");
        builder.Property(pp => pp.PhoneNumber).HasColumnName("phone_number").HasMaxLength(20).IsRequired();
        builder.Property(pp => pp.IsPrimary).HasColumnName("is_primary");
        builder.HasOne(pp => pp.Person)
            .WithMany(p => p.Phones)
            .HasForeignKey(pp => pp.PersonId);
        builder.HasOne(pp => pp.PhoneCode)
            .WithMany(pc => pc.PersonPhones)
            .HasForeignKey(pp => pp.PhoneCodeId);
    }
}