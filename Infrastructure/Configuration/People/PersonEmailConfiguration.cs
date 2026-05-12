using System;
using Domain.Entities.People;
using Domain.ValueObjects.PersonEmails;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration.People;

public sealed class PersonEmailConfiguration : IEntityTypeConfiguration<PersonEmail>
{
    public void Configure(EntityTypeBuilder<PersonEmail> builder)
    {
        builder.ToTable("personemails");
        builder.HasKey(pe => pe.Id);
        builder.Property(pe => pe.Id).HasColumnName("id");
        builder.Property(pe => pe.PersonId).HasColumnName("person_id");
        builder.Property(pe => pe.EmailUser)
            .HasColumnName("email_user")
            .HasMaxLength(100)
            .IsRequired()
            .HasConversion(
                v => v.Value,
                v => EmailUser.Create(v));
        builder.Property(pe => pe.EmailDomainId).HasColumnName("email_domain_id");
        builder.Property(pe => pe.IsPrimary).HasColumnName("is_primary").IsRequired();
        builder.HasOne(pe => pe.Person)
            .WithMany()
            .HasForeignKey(pe => pe.PersonId)
            .OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(pe => pe.EmailDomain)
            .WithMany()
            .HasForeignKey(pe => pe.EmailDomainId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
