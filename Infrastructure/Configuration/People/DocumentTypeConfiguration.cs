using System;
using Domain.Entities.People;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration.People;
public sealed class DocumentTypeConfiguration : IEntityTypeConfiguration<DocumentType>
{
    public void Configure(EntityTypeBuilder<DocumentType> builder)
    {
        builder.ToTable("documenttypes");
        builder.HasKey(d => d.Id);
        builder.Property(d => d.Id).HasColumnName("id");
        builder.Property(d => d.Name).HasColumnName("name").HasMaxLength(50).IsRequired();
        builder.Property(d => d.Code).HasColumnName("code").HasMaxLength(10).IsRequired();
        builder.HasIndex(d => d.Code).IsUnique();
    }
}