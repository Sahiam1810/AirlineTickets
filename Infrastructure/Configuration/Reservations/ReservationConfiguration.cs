using Domain.Entities.Reservations;
using Domain.ValueObjects.Reservations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration.Reservations;

public sealed class ReservationConfiguration : IEntityTypeConfiguration<Reservation>
{
    public void Configure(EntityTypeBuilder<Reservation> builder)
    {
        builder.ToTable("reservations");
        builder.HasKey(c => c.Id);
        builder.Property(c => c.Id).HasColumnName("id");
        
        builder.Property(c => c.ReservationCode)
            .HasColumnName("reservation_code")
            .HasMaxLength(30)
            .IsRequired()
            .HasConversion(
                v => v.Value,
                v => ReservationCode.Create(v));
                
        builder.HasIndex(c => c.ReservationCode).IsUnique();

        builder.Property(c => c.ClientId)
            .HasColumnName("client_id")
            .IsRequired();

        builder.Property(c => c.ReservationDate)
            .HasColumnName("reservation_date")
            .IsRequired()
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.Property(c => c.ReservationStatusId)
            .HasColumnName("reservation_status_id")
            .IsRequired();

        builder.Property(c => c.TotalValue)
            .HasColumnName("total_value")
            .HasPrecision(18, 2)
            .IsRequired()
            .HasConversion(
                v => v.Value,
                v => TotalValue.Create(v));

        builder.ToTable(t => t.HasCheckConstraint("chk_valor_total", "total_value >= 0"));

        builder.Property(c => c.ExpiresAt)
            .HasColumnName("expires_at")
            .IsRequired(false);

        builder.Property(c => c.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired()
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.Property(c => c.UpdatedAt)
            .HasColumnName("updated_at")
            .IsRequired()
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.HasOne(r => r.Client)
            .WithMany()
            .HasForeignKey(r => r.ClientId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(r => r.ReservationStatus)
            .WithMany(rs => rs.Reservations)
            .HasForeignKey(r => r.ReservationStatusId)
            .OnDelete(DeleteBehavior.Restrict);
            
        builder.HasMany(r => r.ReservationFlights)
            .WithOne(rf => rf.Reservation)
            .HasForeignKey(rf => rf.ReservationId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
