using System;
using Domain.Entities.People;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration.People;
public sealed class EmailDomainConfiguration : IEntityTypeConfiguration<EmailDomain>
{
    public void Configure(EntityTypeBuilder<EmailDomain> builder)
    {
        builder.ToTable("emaildomains");
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).HasColumnName("id");
        builder.Property(e => e.Domain).HasColumnName("domain").HasMaxLength(100).IsRequired();
        builder.HasIndex(e => e.Domain).IsUnique();
    }
}