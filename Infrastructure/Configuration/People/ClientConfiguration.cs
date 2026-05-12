using System;
using Domain.Entities.People;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration.People;

public sealed class ClientConfiguration : IEntityTypeConfiguration<Client>
{
    public void Configure(EntityTypeBuilder<Client> builder)
    {
        builder.ToTable("clients");
        builder.HasKey(c => c.Id);
        builder.Property(c => c.Id).HasColumnName("id");
        builder.Property(c => c.PersonId).HasColumnName("person_id").IsRequired();
        builder.Property(c => c.CreatedAt).HasColumnName("created_at");
        builder.HasIndex(c => c.PersonId).IsUnique();
        builder.HasOne(c => c.Person)
            .WithOne()
            .HasForeignKey<Client>(c => c.PersonId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
