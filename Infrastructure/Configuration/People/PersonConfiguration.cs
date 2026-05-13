using System;
using Domain.Entities.People;
using Domain.ValueObjects.People;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration.People;

public sealed class PersonConfiguration : IEntityTypeConfiguration<Person>
{
    public void Configure(EntityTypeBuilder<Person> builder)
    {
        builder.ToTable("people");
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Id).HasColumnName("id");
        builder.Property(p => p.DocumentTypeId)
            .HasColumnName("document_type_id")
            .IsRequired();
        builder.Property(p => p.DocumentNumber)
            .HasColumnName("document_number")
            .HasMaxLength(30)
            .IsRequired()
            .HasConversion(v => v.Value, v => DocumentNumber.Create(v));
        builder.Property(p => p.FirstName)
            .HasColumnName("first_name")
            .HasMaxLength(100)
            .IsRequired()
            .HasConversion(v => v.Value, v => PersonName.Create(v));
        builder.Property(p => p.LastName)
            .HasColumnName("last_name")
            .HasMaxLength(100)
            .IsRequired()
            .HasConversion(v => v.Value, v => PersonName.Create(v));
        builder.Property(p => p.BirthDate).HasColumnName("birth_date");
        builder.Property(p => p.Gender)
            .HasColumnName("gender")
            .HasMaxLength(1)
            .HasConversion(v => v == null ? null : v.Value, v => Gender.Create(v));
        builder.Property(p => p.AddressId).HasColumnName("address_id");
        builder.Property(p => p.CreatedAt).HasColumnName("created_at");
        builder.Property(p => p.UpdatedAt).HasColumnName("updated_at");
        builder.HasIndex(p => new { p.DocumentTypeId, p.DocumentNumber }).IsUnique();
        builder.HasOne(p => p.DocumentType)
            .WithMany(dt => dt.People)
            .HasForeignKey(p => p.DocumentTypeId)
            .OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(p => p.Address)
            .WithMany()
            .HasForeignKey(p => p.AddressId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
