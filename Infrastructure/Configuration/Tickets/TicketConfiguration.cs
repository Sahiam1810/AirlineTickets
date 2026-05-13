using Domain.Entities.Tickets;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration.Tickets;

public sealed class TicketConfiguration : IEntityTypeConfiguration<Ticket>
{
    public void Configure(EntityTypeBuilder<Ticket> builder)
    {
        builder.ToTable("tickets");
        builder.HasKey(t => t.Id);
        builder.Property(t => t.Id).HasColumnName("id");

        builder.Property(t => t.ReservationPassengerId)
            .HasColumnName("reservation_passenger_id")
            .IsRequired();

        builder.Property(t => t.TicketCode)
            .HasColumnName("ticket_code")
            .HasMaxLength(30)
            .IsRequired();

        builder.Property(t => t.IssuedAt)
            .HasColumnName("issued_at")
            .IsRequired();

        builder.Property(t => t.TicketStatusId)
            .HasColumnName("ticket_status_id")
            .IsRequired();

        builder.Property(t => t.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired()
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.Property(t => t.UpdatedAt)
            .HasColumnName("updated_at")
            .IsRequired()
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.HasIndex(t => t.ReservationPassengerId).IsUnique();
        builder.HasIndex(t => t.TicketCode).IsUnique();

        builder.HasOne(t => t.ReservationPassenger)
            .WithOne()
            .HasForeignKey<Ticket>(t => t.ReservationPassengerId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(t => t.TicketStatus)
            .WithMany()
            .HasForeignKey(t => t.TicketStatusId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
