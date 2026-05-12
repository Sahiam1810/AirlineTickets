using System;
using Domain.Entities.Tickets;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration.Tickets;

public sealed class TicketStatusConfiguration : IEntityTypeConfiguration<TicketStatus>
{
    public void Configure(EntityTypeBuilder<TicketStatus> builder)
    {
        builder.ToTable("ticketstatuses");
        builder.HasKey(ts => ts.Id);
        builder.Property(ts => ts.Id).HasColumnName("id");
        builder.Property(ts => ts.Name).HasColumnName("name").HasMaxLength(50).IsRequired();
        builder.HasIndex(ts => ts.Name).IsUnique();
    }
}
